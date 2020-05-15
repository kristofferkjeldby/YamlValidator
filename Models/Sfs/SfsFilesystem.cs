namespace YamlValidator.Models.Sfs
{
    using System.Collections.Generic;
    using System.Linq;

    public class SfsFileSystem : SfsEntity
    {
        public SfsFileSystem()
        {
            Projects = new List<SfsProject>();
        }

        public List<SfsProject> Projects { get; set; }

        public List<SfsPredicate> Predicates =>
            Projects.SelectMany(project => project.Predicates).ToList();

        public List<SfsItem> Items =>
            Predicates.SelectMany(predicate => predicate.Items).ToList();
    }
}
