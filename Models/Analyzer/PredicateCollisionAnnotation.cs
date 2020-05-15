namespace YamlValidator.Models.Analyzer
{
    using System.Collections.Generic;
    using System.Linq;
    using YamlValidator.Extensions;
    using YamlValidator.Models.Sfs;

    public class PredicateCollisionAnnotation : AnalyzerAnnotation
    {
        public PredicateCollisionAnnotation(string root, IEnumerable<SfsPredicate> predicates)
        {
            Root = root;
            AnnotationType = AnalyzerAnnotationType.Error;
            Predicates = predicates;
        }

        public string Root { get; set; }
        public IEnumerable<SfsPredicate> Predicates { get; set; }
        public override string Message => $"Predicate collisions found for {Root}";
        public override List<string> SubMessages => Predicates.Select(predicates => $"{predicates.Project.Name} {predicates.Name} {predicates.GetRoot()}").ToList();
    }
}
