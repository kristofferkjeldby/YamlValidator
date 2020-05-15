namespace YamlValidator.Analyzers
{
    using Models.Analyzer;
    using Models.Sfs;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using YamlValidator.Extensions;
    using YamlValidator.Services;

    public class FilePathAnalyzer : Analyzer
    {
        public override string Message => "Validating serialization file paths";

        public override void Execute(SfsFileSystem filesystem)
        {
            foreach (var predicate in filesystem.Predicates.Where(predicate => predicate.HasRoot()))
            {
                TreeItem rootTreeItem = ParseTree(predicate);

                ResolveExpectedFilePaths(rootTreeItem, predicate);

                foreach (var treeItem in rootTreeItem.Decendants)
                {
                    var expectedFilePath = treeItem.ExpectedFilePath;
                    var actualFilePath = treeItem.Item.FilePath;

                    if (!expectedFilePath.Equals(actualFilePath, StringComparison.InvariantCultureIgnoreCase))
                    {
                        treeItem.Item.Annotations.Add(new FilePathAnnotation(actualFilePath, expectedFilePath, treeItem.Item));
                    }
                }
            }
        }

        private static TreeItem ParseTree(SfsPredicate predicate)
        {
            var items = predicate.Items.Where(item => !predicate.HasAnnotation<DuplicateIdAnnotation>()).OrderBy(item => item.SitecoreItemPath);

            var rootTreeItem = new TreeItem(null, null, predicate.FilePath + Settings.SfsFileExtension);

            foreach (var item in items)
            {
                var parentTreeItem = rootTreeItem.Decendants.FirstOrDefault(treeItem => treeItem.Item.SitecoreId.Equals(item.SitecoreParentId, StringComparison.InvariantCultureIgnoreCase));
                
                if (parentTreeItem == null)
                {
                    if (item.SitecoreItemPath.Substring(predicate.GetRoot().Length).Contains(Settings.SitecoreSeperator))
                    {
                        item.Annotations.Add(new FilePathSparseSerializationTreeAnnotation(item.SitecoreItemPath));
                        continue;
                    }
                    parentTreeItem = rootTreeItem;
                }

                parentTreeItem.AddChild(item);
            }

            return rootTreeItem;
        }

        private void ResolveExpectedFilePaths(TreeItem treeItem, SfsPredicate predicate)
        {
            if (treeItem.Item != null)
            {
                treeItem.ExpectedFilePath = ResolveExpectedFilePath(treeItem, predicate);
            }

            treeItem.Children.ForEach(child => ResolveExpectedFilePaths(child, predicate));
        }

        private class TreeItem
        {
            public TreeItem(TreeItem parent, SfsItem item, string expectedFilePath = "")
            {
                Item = item;
                Children = new List<TreeItem>();
                Parent = parent;
            }

            public void AddChild(SfsItem item)
            {
                this.Children.Add(new TreeItem(this, item));
            }

            public string SuggestedFileName => RainbowService.PrepareItemNameForFileSystem(Item.SitecoreItemName);
            public string SuggestedSafeFileName => String.Concat(SuggestedFileName, "_", Item.SitecoreId);
            public string SuggestedDirectory => Path.ChangeExtension(Parent.Item.FilePath, null);
            public string SuggestedFilePath => Path.Combine(SuggestedDirectory, SuggestedFileName);
            public string SuggestedSafeFilePath => Path.Combine(SuggestedDirectory, SuggestedSafeFileName);

            public string ActualFileName => Item.FileNameWithoutExtension;

            public string ExpectedFilePath { get; set; }
            public SfsItem Item;
            public List<TreeItem> Children { get; set; }
            public TreeItem Parent { get; set; }
            public List<TreeItem> Decendants => Children.Union(Children.SelectMany(child => child.Decendants)).ToList();
        }

        private static string ResolveExpectedFilePath(TreeItem treeItem, SfsPredicate predicate)
        {
            var candidates = treeItem.Parent.Children.Where(candidate =>
            {
                return candidate.ActualFileName.Equals(treeItem.SuggestedFileName, StringComparison.OrdinalIgnoreCase) ||
                       candidate.ActualFileName.Equals(treeItem.SuggestedSafeFileName, StringComparison.OrdinalIgnoreCase);
            });

            string expectedFilePath = string.Empty;

            // This logic is taken from Rainbow to mimic the exact choosing of file name

            foreach (var candidate in candidates)
            {
                if (candidate.Item.SitecoreId == treeItem.Item.SitecoreId)
                {
                    expectedFilePath = candidate.Item.FilePath;
                    break;
                }
                if (candidate.Item.SitecoreId != treeItem.Item.SitecoreId)
                {
                    expectedFilePath = treeItem.SuggestedSafeFilePath + Settings.SfsFileExtension;
                }
            }

            if (string.IsNullOrEmpty(expectedFilePath))
                expectedFilePath = treeItem.SuggestedFilePath + Settings.SfsFileExtension;
            if (expectedFilePath.Substring(predicate.FilePath.Length).Length < Settings.SfsMaxRelativePathLength)
                return expectedFilePath;
            return Path.Combine(predicate.FilePath, treeItem.Parent.Item.SitecoreId, Path.GetFileName(expectedFilePath));
        }
    }
}
