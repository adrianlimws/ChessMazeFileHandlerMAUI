using ChessMaze.FileHandler.Filer.Extensions;
using ChessMaze.FileHandler.Filer.Interfaces;
using System.IO;


namespace ChessMaze.FileHandler.Filer;

public partial class FileHandlerMain
{
    /// <summary>
    /// Saves current level to a file
    /// </summary>
    /// <param name="level">The level to be saved</param>
    /// <param name="filePath">Path location where level will be saved</param>
    /// <exception cref="IOException">Thrown when there's an error saving the level</exception>
    public void SaveGameLevel(ILevel level, string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GenerateAutoFileName("ChessMazeLevel");
            }

            using var writer = new StreamWriter(filePath);
            SaveLevel(level, writer);
            Logger.Log($"SaveGameLevel - Success: {filePath}");
        }
        catch (UnauthorizedAccessException e)
        {
            Logger.Log($"SaveGameLevel - Failure: {filePath}. Error: Unauthorized access");
            DisplayErrorMessage("Error: No permission to write file", e);
            throw new IOException($"Failed to save level to file: {e.Message}", e);
        }
        catch (DirectoryNotFoundException e)
        {
            Logger.Log($"SaveGameLevel - Failure: {filePath}. Error: Directory not found");
            DisplayErrorMessage("Error: The chosen directory does not exist", e);
            throw new IOException($"Failed to save level to file: {e.Message}", e);
        }
        catch (IOException e)
        {
            Logger.Log($"SaveGameLevel - Failure: {filePath}. Error: I/O error");
            DisplayErrorMessage("Error: An I/O error occurred while writing file", e);
            throw;
        }
        catch (Exception e)
        {
            Logger.Log($"SaveGameLevel - Failure: {filePath}. Error: {e.Message}");
            DisplayUnexpectedError("An unexpected error occurred while saving level", e);
            throw new IOException($"Failed to save level to file: {e.Message}", e);
        }
    }

    /// <summary>
    /// Saves the level data to a StreamWriter
    /// </summary>
    /// <param name="level">Level to be saved</param>
    /// <param name="writer">The StreamWriter to write the level data to</param>
    private static void SaveLevel(ILevel level, StreamWriter writer)
    {
        writer.WriteLine($"{level.Board.Rows}{Separator}{level.Board.Columns}");
        CreateBoard(level.Board, writer);
        CreatePosition(level.StartPosition, writer);
        CreatePosition(level.EndPosition, writer);
        CreatePosition(level.Player.CurrentPosition, writer);
        writer.WriteLine(level.IsCompleted.ToString());
    }

    /// <summary>
    /// Writes the board data to a StreamWriter
    /// </summary>
    /// <param name="board">Board to be written</param>
    /// <param name="writer">The StreamWriter to write the board data to</param>
    private static void CreateBoard(IBoard board, StreamWriter writer)
    {
        for (int row = 0; row < board.Rows; row++)
        {
            for (int col = 0; col < board.Columns; col++)
            {
                writer.Write(board.Cells[row, col].ToString());
            }

            writer.WriteLine();
        }
    }

    /// <summary>
    /// Writes a position to a StreamWriter
    /// </summary>
    /// <param name="position">Position to be written</param>
    /// <param name="writer">The StreamWriter to write the position to</param>
    private static void CreatePosition(IPosition position, StreamWriter writer)
    {
        writer.WriteLine($"{position.Row}{Separator}{position.Column}");
    }
}
// References
// https://learn.microsoft.com/en-us/dotnet/api/system.io.streamwriter?view=net-8.0