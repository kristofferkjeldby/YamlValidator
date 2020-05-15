// This class contains code copied from Rainbow 2.0.6.0 dll
// This code enables the calculation of item paths based on
// the same logic used by Rainbow/Unicorn
namespace YamlValidator.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class RainbowService
    {
        private static char[] InvalidFileNameCharacters = ((IEnumerable<char>)Path.GetInvalidFileNameChars()).Concat<char>((IEnumerable<char>)Settings.SfsExtraInvalidFilenameCharacters).ToArray<char>();
        private static HashSet<string> InvalidFileNames = new HashSet<string>((IEnumerable<string>)Settings.SfsInvalidFilenames, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);

        public static string PrepareItemNameForFileSystem(string name)
        {
            string str = string.Join("_", name.TrimStart(' ').Split(InvalidFileNameCharacters));
            if (str.Length > Settings.SfsMaxItemNameLengthBeforeTruncation)
                str = str.Substring(0, Settings.SfsMaxItemNameLengthBeforeTruncation);
            if (str[str.Length - 1] == ' ')
                str = str.Substring(0, str.Length - 1) + "_";
            if (InvalidFileNames.Contains(str))
                return "_" + str;
            return str;
        }
    }
}
