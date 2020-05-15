using System.Collections.Generic;
using YamlValidator.Models.Sfs;

namespace YamlValidator.Models.Analyzer
{
    public class FilePathAnnotation : AnalyzerAnnotation
    {
        public FilePathAnnotation(string actualFilePath, string expectedFilePath, SfsItem item)
        {
            AnnotationType = AnalyzerAnnotationType.Error;
            ActualFilePath = actualFilePath;
            ExpectedFilePath = expectedFilePath;
            Item = item;
        }

        public string ActualFilePath { get; set; }
        public string ExpectedFilePath { get; set; }
        public SfsItem Item { get; set; }
        public override string Message => $"File path does not match expected file path for item {Item.SitecoreItemPath}";
        public override List<string> SubMessages { get
            {
                return new List<string>() {
                    $"Actual:   {ActualFilePath}",
                    $"Expected: {ExpectedFilePath}"
                };
            }
        }
    }
}
