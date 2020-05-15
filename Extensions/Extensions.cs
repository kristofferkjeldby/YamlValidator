namespace YamlValidator.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using YamlValidator.Models.Analyzer;
    using YamlValidator.Models.Sfs;

    public static class Extensions
    {
        public static List<T> GetAnnotations<T>(this SfsEntity entity) where T : AnalyzerAnnotation
        {
            return entity.Annotations.Where(annotation => annotation.GetType() == typeof(T)).Cast<T>().ToList();
        }

        public static T GetAnnotation<T>(this SfsEntity entity) where T : AnalyzerAnnotation
        {
            return entity.GetAnnotations<T>().FirstOrDefault();
        }

        public static string GetRoot(this SfsPredicate predicate)
        {
            return predicate.GetAnnotation<PredicateRootAnnotation>()?.Root;
        }

        public static bool HasRoot(this SfsPredicate predicate)
        {
            return !string.IsNullOrEmpty(predicate.GetRoot());
        }

        public static bool HasAnnotation<T>(this SfsEntity entity) where T : AnalyzerAnnotation
        {
            return entity.GetAnnotation<T>() != null;
        }
    }
}
