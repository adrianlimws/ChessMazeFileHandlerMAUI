using System;
using System.IO;
using System.Linq;
using ChessMaze.FileHandler.Filer.Enums;
using ChessMaze.FileHandler.Filer.Extensions;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer
{
    public partial class FileHandlerMain
    {
        /// <summary>
        /// Loads a previous game state from file
        /// </summary>
        /// <param name="filePath">Path location of file to load the game from</param>
        /// <returns>A loaded game</returns>
        /// <exception cref="IOException">Thrown when error loading game</exception>
        public IGame LoadGameState(string filePath)
        {
            try
            {
                CheckIfFileExists(filePath);
                string[] lines = File.ReadAllLines(filePath);
                CheckGameFileFormat(lines);

                ILevel level = LoadLevelFromLines(lines.Take(lines.Length - 1).ToArray());

                int moveCount = int.Parse(lines[^1]);

                Logger.Log($"LoadGameState - Success: {filePath}");

                return new Game
                {
                    CurrentLevel = level,
                    MoveCount = moveCount
                };
            }
            catch (FileNotFoundException e)
            {
                Logger.Log($"LoadGameState - Failure: {filePath}. Error: {e.Message}");
                DisplayErrorMessage("Error: The chosen file was not found", e);
                throw;
            }
            catch (FormatException e)
            {
                DisplayErrorMessage("Error: The game file might be corrupted or has an invalid format", e);
                throw new IOException("Failed to load game: Invalid file format", e);
            }
            catch (IOException e)
            {
                DisplayErrorMessage("Error: An I/O error occurred while reading file", e);
                throw;
            }
            catch (Exception e)
            {
                Logger.Log($"LoadGameState - Failure: {filePath}. Error: {e.Message}");
                DisplayUnexpectedError("An unexpected error occurred while loading game", e);
                throw new IOException($"Fail to load game file: {e.Message}", e);
            }
        }
    }

    // References
    // https://code-maze.com/csharp-array-slicing/
    // https://stackoverflow.com/questions/18203809/drawing-a-chess-board
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalllines?view=net-8.0
    // https://stackoverflow.com/questions/1262965/how-do-i-read-a-specified-line-in-a-text-file
}