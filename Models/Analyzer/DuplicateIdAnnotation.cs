namespace YamlValidator.Models.Analyzer
{
    using System.Collections.Generic;
    using System.Linq;
    using YamlValidator.Models.Sfs;

    public class DuplicateIdAnnotation : AnalyzerAnnotation
    {
        public DuplicateIdAnnotation(string id, IEnumerable<SfsItem> items)
        {
            AnnotationType = AnalyzerAnnotationType.Error;
            Message = $"Duplicate id found for {id}";
            Items = items.ToList();
            SubMessages = items.Select(item => item.FilePath).ToList();
        }

        public List<SfsItem> Items { get; set; }
    }
}
