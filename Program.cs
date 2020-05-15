namespace YamlValidator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Services;
    using YamlValidator.Analyzers;
    using YamlValidator.Models.Analyzer;

    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            WriteHead("Yaml Validator");

            Write($"Path name maxLength: {Settings.SfsSerializationFolderPathMaxLength}");
            Write($"Item name maxLength: {Settings.SfsMaxItemNameLengthBeforeTruncation}");
            Write($"SFS filesystem root: {Settings.SfsRoot}");
            Write();

            WriteHead("Parsing SFS filesystem ...");
            Write();

            var fileSystem = FileService.GetFileSystem(Settings.SfsRoot, (count) => WriteProgress("Parsing", "files", count));
            Write();

            var analyzers = new List<Analyzer>();
            analyzers.Add(new SerializationFolderAnalyzer());
            analyzers.Add(new PredicateRootAnalyzer());
            analyzers.Add(new DuplicateIdAnalyser());
            analyzers.Add(new PredicateCollisionAnalyzer());
            analyzers.Add(new DuplicateItemPathsAnalyser());
            analyzers.Add(new FilePathAnalyzer());

            WriteHead("Analyzing SFS filesystem ...");

            foreach (var analyzer in analyzers)
            {
                Write(analyzer.Message);
                analyzer.Execute(fileSystem);
            }
            Write();

            WriteHead("Report");

            var indent = 0;

            foreach (var project in fileSystem.Projects)
            {
                Write($"{project.Name} ({project.Items.Count()})", indent, ConsoleColor.White);
                project.Annotations.ForEach(annotation => Write(annotation, indent + 1)); 

                foreach (var predicate in project.Predicates)
                {
                    indent++;
                    Write($"{predicate.Name} ({predicate.Items.Count()})", indent);
                    predicate.Annotations.ForEach(annotation => Write(annotation, indent + 1));

                    foreach (var item in predicate.Items)
                    {
                        if (item.Annotations.Any(annotation => Settings.ShowInternal || (annotation.AnnotationType != AnalyzerAnnotationType.Internal)))
                        {
                            indent++;
                            Write(item.SitecoreItemPath, indent);
                            item.Annotations.ForEach(annotation => Write(annotation, indent + 1));
                            indent--;
                        }

                    }

                    indent--;
                }
            }

            Console.CursorVisible = true;
            Console.ReadKey();
        }

        private static void WriteHead(string text)
        {
            Write(text, 0, ConsoleColor.White);
            Write(new String('-', text.Length));
        }

        private static void Write(AnalyzerAnnotation annotation, int indent = 0)
        {
            switch (annotation.AnnotationType)
            {
                case AnalyzerAnnotationType.Error:
                    Write($"Error: {annotation.Message}", indent, ConsoleColor.Red);
                    annotation.SubMessages.ForEach(subMessage => Write($"- {subMessage}", indent + 1, ConsoleColor.DarkRed));
                    break;
                case AnalyzerAnnotationType.Warning:
                    Write($"Warning: {annotation.Message}", indent, ConsoleColor.Yellow);
                    annotation.SubMessages.ForEach(subMessage => Write($"- {subMessage}", indent + 1, ConsoleColor.DarkYellow));
                    break;
                case AnalyzerAnnotationType.Internal:
                    if (Settings.ShowInternal)
                    {
                        Write($"Internal: {annotation.Message}", indent, ConsoleColor.Green);
                        annotation.SubMessages.ForEach(subMessage => Write($"- {subMessage}", indent + 1, ConsoleColor.DarkGreen));
                    }
                    break;
            }
        }

        private static void WriteProgress(string prefix, string postfix, int count)
        {
            Write(true, $"{prefix} {count} {postfix}");
        }

        private static void Write(string text = "", int indent = 0, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            Write(false, text, indent, consoleColor);
        }

        private static void Write(bool overwrite, string text = "", int indent = 0, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            if (overwrite)
                Console.CursorTop--;

            var existingConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(String.Concat(new string(Settings.Indent, indent*Settings.IndentCount), text));
            Console.ForegroundColor = existingConsoleColor;
        }
    }
}
