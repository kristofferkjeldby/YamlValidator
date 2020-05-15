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
            foreach (var project in filesystem.Projects.Where(project => project.FilePath.Length > Settings.SfsSerializationFolderPathMaxLength))
            {
                project.Annotations.Add(new SerializationFolderAnnotation(project.FilePath));
            }
        }
    }
}
