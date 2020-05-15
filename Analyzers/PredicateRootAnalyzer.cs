namespace YamlValidator.Analyzers
{
    using Models.Analyzer;
    using Models.Sfs;
    using System;
    using System.IO;
    using System.Linq;

    public class PredicateRootAnalyzer : Analyzer
    {
        public override string Message => "Finding predicate roots";

        public override void Execute(SfsFileSystem filesystem)
        {
            foreach (var predicate in filesystem.Predicates)
            {
                predicate.Annotations.Add(GetAnnotation(predicate));
            }
        }

        private AnalyzerAnnotation GetAnnotation(SfsPredicate predicate)
        {
            foreach (var item in predicate.Items)
            {
                var filePathParts = item.FilePath.Substring(item.Predicate.FilePath.Length + 1).Split(Path.DirectorySeparatorChar);
                if (filePathParts.Count() == 1 || !Guid.TryParse(filePathParts[0], out _))
                {
                    var sitecoreItemPathParts = item.SitecoreItemPath.Split(Settings.SitecoreSeperator);
                    return new PredicateRootAnnotation(string.Join(Settings.SitecoreSeperator.ToString(), sitecoreItemPathParts.Take(sitecoreItemPathParts.Count() - (filePathParts.Count() - 1))));

                }
            }

            return new PredicateNoRootAnnotation();
        }
    }
}
