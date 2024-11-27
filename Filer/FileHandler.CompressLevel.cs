using System.Text;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer
{
    public partial class FileHandlerMain
    {
        /// <summary>
        /// Saves a compressed level to a file
        /// </summary>
        /// <param name="level">Level to be saved</param>
        /// <param name="filePath">Path of file save location</param>
        /// <exception cref="IOException">Thrown when there's an error saving the file</exception>
        public void SaveCompressedLevel(ILevel level, string filePath)
        {
            try
            {
                if (!ValidateInputLevel(level))
                {
                    throw new ArgumentException("Invalid input level data for compression");
                }

                string uncompressedLevel = GetUncompressedLevelString(level);
                _converter.Compress(uncompressedLevel);

                string saveCompressedLevelDataIfSmallerSize = _converter.Compressed.Length < uncompressedLevel.Length
                    ? _converter.Compressed
                    : uncompressedLevel;

                using var writer = new StreamWriter(filePath);
                writer.WriteLine($"{level.Board.Rows}{Separator}{level.Board.Columns}");
                writer.WriteLine(saveCompressedLevelDataIfSmallerSize);
                CreatePosition(level.StartPosition, writer);
                CreatePosition(level.EndPosition, writer);
                CreatePosition(level.Player.CurrentPosition, writer);
                writer.WriteLine(level.IsCompleted.ToString());
            }
            catch (ArgumentException e)
            {
                DisplayErrorMessage("Error: Invalid input data for compression", e);
                throw new IOException($"Failed to compress level: {e.Message}", e);
            }
            catch (Exception e)
            {
                DisplayUnexpectedError("An unexpected error occurred saving compressed level", e);
                throw new IOException($"Failed to save compressed level to file: {e.Message}", e);
            }
        }

        /// <summary>
        /// Validates input level before compression
        /// </summary>
        /// <param name="level">Level to be validated</param>
        /// <returns>True if level is valid</returns>
        private bool ValidateInputLevel(ILevel level)
        {
            return level != null &&
                   level.Board != null &&
                   level.Board.Rows > 0 &&
                   level.Board.Columns > 0 &&
                   level.StartPosition != null &&
                   level.EndPosition != null &&
                   level.Player?.CurrentPosition != null;
        }

        /// <summary>
        /// Converts a level to an uncompressed string representation
        /// </summary>
        /// <param name="level">Level to be converted</param>
        /// <returns>A string representation of uncompressed level</returns>
        private string GetUncompressedLevelString(ILevel level)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < level.Board.Rows; row++)
            {
                for (var col = 0; col < level.Board.Columns; col++)
                {
                    sb.Append(level.Board.Cells[row, col].Type.ToString()[0]);
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}