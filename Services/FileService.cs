namespace YamlValidator.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Models.Sfs;

    public static class FileService
    {
        public static SfsFileSystem GetFileSystem(string sfsRoot, Action<int> callback)
        {
            var fileSystem = new SfsFileSystem();
            fileSystem.FilePath = sfsRoot;
            int count = 0;

            foreach (var layerRoot in Directory.GetDirectories(sfsRoot))
            {
                var layerName = layerRoot.Split('\\').Last();

                foreach (var projectRoot in Directory.GetDirectories(layerRoot))
                {
                    var serializationRoot = Path.Combine(projectRoot, Settings.SfsSerializationProjectFolder);

                    if (Directory.Exists(serializationRoot))
                    { 
                        var projectName = projectRoot.Split('\\').Last();
                        var project = new SfsProject();
                        project.Filesystem = fileSystem;
                        project.Name = String.Join(layerName, projectName);
                        project.FilePath = serializationRoot;

                        foreach (var predicateRoot in Directory.GetDirectories(serializationRoot))
                        {
                            var predicateName = predicateRoot.Split('\\').Last();
                            var predicate = new SfsPredicate();
                            predicate.Project = project;
                            predicate.FilePath = predicateRoot;
                            predicate.Name = predicateName;
                            predicate.Items =
                                ParseSfsItems(
                                    Directory.GetFiles(predicateRoot, String.Concat("*", Settings.SfsFileExtension),
                                        SearchOption.AllDirectories), predicate, () => callback(++count));
                            if (predicate.Items.Any())
                                project.Predicates.Add(predicate);
                        }
                        if (project.Predicates.Any())
                            fileSystem.Projects.Add(project);
                    }
                }
            }

            return fileSystem;
        }

        private static List<SfsItem> ParseSfsItems(IEnumerable<string> filePaths, SfsPredicate predicate, Action callback)
        {
            var result = new List<SfsItem>();

            foreach (var filePath in filePaths)
            {
                callback();

                string content = File.ReadAllText(filePath);

                var yamlFile = new SfsItem()
                {
                    Predicate = predicate,
                    SitecoreId = Regex.Match(content, "^ID: \"(.*)\"", RegexOptions.Multiline).Groups[1].Value,
                    SitecoreParentId = Regex.Match(content, "^Parent: \"(.*)\"", RegexOptions.Multiline).Groups[1].Value,
                    SitecoreItemPath = Regex.Match(content, "^Path: \"?([^\"\\n\\r]*)\"?", RegexOptions.Multiline).Groups[1].Value,
                    FilePath = filePath
                };

                if (!string.IsNullOrEmpty(yamlFile.SitecoreId)) result.Add(yamlFile);
            }
            return result;
        }
    }
}
