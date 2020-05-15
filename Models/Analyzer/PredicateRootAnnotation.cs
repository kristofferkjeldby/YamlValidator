namespace YamlValidator.Models.Analyzer
{
    public class PredicateRootAnnotation : AnalyzerAnnotation
    {
        public PredicateRootAnnotation(string root)
        {
            AnnotationType = AnalyzerAnnotationType.Internal;
            Message = $"Predicate root resolved to: {root}";
            Root = root;
        }

        public string Root { get; set; }

    }
}
