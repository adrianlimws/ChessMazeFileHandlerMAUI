using ChessMaze.FileHandler.Filer.Extensions;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer
{
    public partial class FileHandlerMain : IFileHandlerExtension
    {
        private const string Separator = ",";
        private static int _errorCount = 0;
        private static int _minLinesValidGameState = 9;
        private static int _minLinesValidLevelState = 5;
        private const string DefaultStartOfFileName = "ChessMaze";
        private readonly LevelConverter _converter = new LevelConverter();

        // For logging test
        public string GetLogFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "FileHandler.log");
        }
    }

    // References
    // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/private
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const
}   // https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-8.0