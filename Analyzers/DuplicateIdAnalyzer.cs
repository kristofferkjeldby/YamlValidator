namespace YamlValidator.Analyzers
{
    using Models.Analyzer;
    using Models.Sfs;
    using System;
    using System.IO;
    using System.Linq;

    public class DuplicateIdAnalyser : Analyzer
    {
        public override string Message => "Finding duplicate ids";

        public override void Execute(SfsFileSystem filesystem)
        {
            foreach (var itemGroup in filesystem.Items.GroupBy(item => item.SitecoreId).Where(itemGroup => itemGroup.Count() > 1))
            {
                var annotation = new DuplicateIdAnnotation(itemGroup.Key, itemGroup);
                foreach (var item in itemGroup)
                    item.Annotations.Add(annotation);
            }
        }
    }
}
