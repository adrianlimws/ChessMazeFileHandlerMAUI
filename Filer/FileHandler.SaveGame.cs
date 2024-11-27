using ChessMaze.FileHandler.Filer.Extensions;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer;

public partial class FileHandlerMain
{
    /// <summary>
    /// Saves the current game state to a file
    /// </summary>
    /// <param name="game">Game file to be saved</param>
    /// <param name="filePath">Path location the game will be saved</param>
    /// <exception cref="IOException">Thrown when there's an error saving the game</exception>
    public void SaveGameState(IGame game, string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GenerateAutoFileName("ChessMazeGame");
            }

            using var writer = new StreamWriter(filePath);
            SaveLevel(game.CurrentLevel, writer);
            writer.WriteLine(game.GetMoveCount());
            Logger.Log($"SaveGameState - Success: {filePath}");
        }
        catch (UnauthorizedAccessException e)
        {
            Logger.Log($"SaveGameState - Failure: {filePath}. Error: Unauthorised access");
            DisplayErrorMessage("Error: No permission to write file", e);
            throw new IOException($"Failed to save game to file: {e.Message}", e);
        }
        catch (DirectoryNotFoundException e)
        {
            Logger.Log($"SaveGameState - Failure: {filePath}. Error: Directory not found");
            DisplayErrorMessage("Error: The chosen directory does not exist", e);
            throw new IOException($"Failed to save game to file: {e.Message}", e);
        }
        catch (IOException e)
        {
            Logger.Log($"SaveGameState - Failure: {filePath}. Error: I/O error");
            DisplayErrorMessage("Error: An I/O error occurred while writing file", e);
            throw;
        }
        catch (Exception e)
        {
            Logger.Log($"SaveGameState - Failure: {filePath}. Error: {e.Message}");
            DisplayUnexpectedError("An unexpected error occurred while saving game", e);
            throw new IOException($"Failed to save game to file: {e.Message}", e);
        }
    }
}

// References
// https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-explicitly-throw-exceptions
// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/using-exceptions