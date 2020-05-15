namespace YamlValidator.Models.Sfs
{
    using System.Collections.Generic;
    using System.Linq;
    using Analyzer;

    public abstract class SfsEntity
    {
        public SfsEntity()
        {
            Annotations = new List<AnalyzerAnnotation>();
        }

        public string Name { get; set; }
        public string FilePath { get; set; }
        public List<AnalyzerAnnotation> Annotations { get; set; }
    }
}
