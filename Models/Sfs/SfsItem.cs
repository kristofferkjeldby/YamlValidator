namespace YamlValidator.Models.Sfs
{
    using System.IO;
    using System.Linq;

    public class SfsItem : SfsEntity
    {
        // Sfs fields
        public SfsPredicate Predicate { get; set; }

        // Sitecore fields
        public string SitecoreId { get; set; }
        public string SitecoreParentId { get; set; }
        public string SitecoreItemPath { get; set; }
        public string SitecoreItemName => SitecoreItemPath.Split(Settings.SitecoreSeperator).Last().Trim();
        public string SitecoreParentPath => SitecoreItemPath.Substring(0, SitecoreItemPath.Length - (SitecoreItemName.Length + 1));

        // File fields
        public string FileName => Path.GetFileName(FilePath);
        public string FileExtension => Path.GetExtension(FilePath);
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FilePath);
    }
}
