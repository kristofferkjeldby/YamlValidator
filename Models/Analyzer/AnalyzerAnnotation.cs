namespace YamlValidator.Models.Analyzer
{
    using System.Collections.Generic;

    public abstract class AnalyzerAnnotation
    {
        public AnalyzerAnnotation()
        {
            SubMessages = new List<string>();
        }

        public virtual string Message { get; set; }
        public AnalyzerAnnotationType AnnotationType { get; set; }
        public virtual List<string> SubMessages { get; set; }
    }
}