namespace YamlValidator.Models.Analyzer
{
    public class FilePathSparseSerializationTreeAnnotation : AnalyzerAnnotation
    {
        public FilePathSparseSerializationTreeAnnotation(string sitecoreItemPath)
        {
            SitecoreItemPath = sitecoreItemPath;
            AnnotationType = AnalyzerAnnotationType.Error;
            Message = $"Item have a sparse serialization tree: {SitecoreItemPath}";
        }

        public string SitecoreItemPath { get; set; }
    }
}
