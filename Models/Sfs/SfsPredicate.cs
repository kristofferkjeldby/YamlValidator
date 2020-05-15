namespace YamlValidator.Models.Sfs
{
    using System.Collections.Generic;

    public class SfsPredicate : SfsEntity
    {
        public SfsPredicate()
        {
            Items = new List<SfsItem>();
        }

        public List<SfsItem> Items { get; set; }
        public SfsProject Project { get; set; }
    }
}
