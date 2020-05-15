namespace YamlValidator.Models.Sfs
{
    using System.Collections.Generic;
    using System.Linq;

    public class SfsProject : SfsEntity
    {
        public SfsProject()
        {
            Predicates = new List<SfsPredicate>();
        }

        public List<SfsPredicate> Predicates { get; set; }
        public SfsFileSystem Filesystem { get; set; }
        public List<SfsItem> Items => Predicates.SelectMany(predicate => predicate.Items).ToList();
    }
}
