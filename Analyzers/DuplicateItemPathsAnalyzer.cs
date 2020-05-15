namespace YamlValidator.Analyzers
{
    using Models.Analyzer;
    using Models.Sfs;
    using System;
    using System.Linq;
    using YamlValidator.Extensions;

    public class DuplicateItemPathsAnalyser : Analyzer
    {
        public override string Message => "Finding duplicate item paths";

        public override void Execute(SfsFileSystem filesystem)
        {
            foreach (var itemGroup in filesystem.Items.Where(item => item.GetAnnotation<DuplicateIdAnnotation>() == null).
                GroupBy(item => item.SitecoreItemPath, StringComparer.InvariantCultureIgnoreCase).
                Where(itemPathGroup => itemPathGroup.Count() > 1))
            {
                var annotation = new DuplicateItemPathAnnotation(itemGroup.Key, itemGroup);
                foreach (var item in itemGroup)
                    item.Annotations.Add(annotation);
            }
          
        }
    }
}
