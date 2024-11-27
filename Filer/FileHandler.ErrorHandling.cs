namespace ChessMaze.FileHandler.Filer
{
    public partial class FileHandlerMain
    {
        /// <summary>
        /// Displays unexpected error message with unique error code
        /// </summary>
        /// <param name="message">Error message to display</param>
        /// <param name="e">Exception that occurred</param>
        private static void DisplayUnexpectedError(string message, Exception e)
        {
            _errorCount++;
            var errorCode = $"ERR-{DateTime.Now:yyyyMMdd}-{_errorCount:D4}";
            Console.WriteLine($"{message} Error Code: {errorCode}");
            Console.WriteLine($"Please report this error code to the developers of this program");
            // Logger.LogError($"Error Code {errorCode}: {e}");
        }

        /// <summary>
        /// Displays standard error message with details from exception
        /// </summary>
        /// <param name="message">Error message to display</param>
        /// <param name="e">Exception that occurred</param>
        private static void DisplayErrorMessage(string message, Exception e)
        {
            Console.WriteLine($"{message} Details: {e.Message}");
        }

        /// <summary>
        /// Checks if a file exists at the specified path and throws an exception
        /// </summary>
        /// <param name="filePath">Path file location to check</param>
        /// <exception cref="FileNotFoundException">Thrown when file is not found</exception>
        private static void CheckIfFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
        }

        /// <summary>
        /// Validates format of a game file by checking the number of lines
        /// </summary>
        /// <param name="lines">Lines of the game file</param>
        /// <exception cref="FormatException">Thrown when the file format is invalid</exception>
        private static void CheckGameFileFormat(string[] lines)
        {
            if (lines.Length < _minLinesValidGameState)
            {
                throw new FormatException("Invalid game file format");
            }
        }

        /// <summary>
        /// Validates format of a level file by checking the number of lines
        /// </summary>
        /// <param name="lines">Lines of the level file</param>
        /// <exception cref="FormatException">Thrown when the file format is invalid</exception>
        private static void CheckFileLevelFormat(string[] lines)
        {
            if (lines.Length < _minLinesValidLevelState)
            {
                throw new FormatException("Invalid level file format");
            }
        }
    }
}