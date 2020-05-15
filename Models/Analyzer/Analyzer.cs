namespace YamlValidator.Models.Analyzer
{
    using Sfs;

    public abstract class Analyzer
    {
        public abstract string Message { get; }
        public abstract void Execute(SfsFileSystem filesystem);
    }
}
