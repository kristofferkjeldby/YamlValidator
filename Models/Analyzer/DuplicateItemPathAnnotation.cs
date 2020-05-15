namespace YamlValidator.Models.Analyzer
{
    using System.Collections.Generic;
    using System.Linq;
    using YamlValidator.Models.Sfs;

    public class DuplicateItemPathAnnotation : AnalyzerAnnotation
    {
        public DuplicateItemPathAnnotation(string itemPath, IEnumerable<SfsItem> items)
        {
            AnnotationType = AnalyzerAnnotationType.Warning;
            Message = $"Duplicate item paths found for {itemPath}";
            Items = items.ToList();
            SubMessages = items.Select(item => item.FilePath).ToList();
        }

        public List<SfsItem> Items { get; set; }

    }
}
