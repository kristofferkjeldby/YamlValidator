namespace YamlValidator.Analyzers
{
    using Models.Analyzer;
    using Models.Sfs;
    using System.Linq;

    public class SerializationFolderAnalyzer : Analyzer
    {
        public override string Message => "Validating serialization folder lengths";

        public override void Execute(SfsFileSystem filesystem)
        {
            foreach (var predicate in filesystem.Predicates.Where(predicate => predicate.FilePath.Length > Settings.SfsSerializationFolderPathMaxLength))
            {
                predicate.Annotations.Add(new SerializationFolderAnnotation(predicate.FilePath));
            }
        }
    }
}
