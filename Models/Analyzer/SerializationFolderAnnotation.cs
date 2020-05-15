namespace YamlValidator.Models.Analyzer
{
    public class SerializationFolderAnnotation : AnalyzerAnnotation
    {
        public SerializationFolderAnnotation(string root)
        {
            AnnotationType = AnalyzerAnnotationType.Error;
            Message = $"Serialization folder exceeds maximum length: {root}";
            Root = root;
        }

        public string Root { get; set; }

    }
}
