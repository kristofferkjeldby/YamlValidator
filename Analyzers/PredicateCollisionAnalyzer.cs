namespace YamlValidator.Analyzers
{
    using Models.Analyzer;
    using Models.Sfs;
    using System.Linq;
    using YamlValidator.Extensions;

    public class PredicateCollisionAnalyzer : Analyzer
    {
        public override string Message => "Finding predicate collisions";

        public override void Execute(SfsFileSystem filesystem)
        {
            var predicates = filesystem.Predicates.Where(predicate => predicate.HasRoot()).ToList();

            foreach (var predicateGroup in predicates.GroupBy(predicate => predicate.GetRoot()).Where(predicateGroup => predicateGroup.Count() > 1))
            {
                var annotation = new PredicateCollisionAnnotation(predicateGroup.Key, predicateGroup);
                foreach (var predicate in predicateGroup)
                    predicate.Annotations.Add(annotation);
            }
        }
    }
}
