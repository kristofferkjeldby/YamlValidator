namespace YamlValidator.Models.Analyzer
{
    public class PredicateNoRootAnnotation : AnalyzerAnnotation
    {
        public PredicateNoRootAnnotation()
        {
            Message = "Predicate root could not be resolved";
            AnnotationType = AnalyzerAnnotationType.Warning;
        }
    }
}
