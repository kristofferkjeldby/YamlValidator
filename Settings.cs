namespace YamlValidator
{
    static class Settings
    {
        public static char[] SfsExtraInvalidFilenameCharacters => "$".ToCharArray();
        public static string[] SfsInvalidFilenames => "CON,PRN,AUX,NUL,COM1,COM2,COM3,COM4,COM5,COM6,COM7,COM8,COM9,LPT1,LPT2,LPT3,LPT4,LPT5,LPT6,LPT7,LPT8,LPT9".Split(',');
        public static int SfsSerializationFolderPathMaxLength = 110;
        public static int SfsMaxItemNameLengthBeforeTruncation = 30;
        public static int SfsMaxRelativePathLength => 240 - SfsSerializationFolderPathMaxLength;
        public static string SfsRoot = @"C:\Projects\Habitat\src";
        public static string SfsSerializationProjectFolder = "serialization";
        public static string SfsFileExtension = ".yml";
        public static char SitecoreSeperator = '/';
        public static char Indent = ' ';
        public static int IndentCount = 4;
        public static bool ShowInternal = false;
        public static string SolutionPrefix = "Sitecore";
    }
}
