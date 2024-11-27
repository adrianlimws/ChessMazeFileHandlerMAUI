using ChessMaze.FileHandler.Filer.Enums;
using ChessMaze.FileHandler.Filer.Extensions;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer;

public partial class FileHandlerMain
{
    /// <summary>
    /// Loads level from a file
    /// </summary>
    /// <param name="filePath">Path location of file to load level from</param>
    /// <returns>A loaded level</returns>
    /// <exception cref="IOException">Thrown when there's an error loading the level</exception>
    public ILevel LoadGameLevel(string filePath)
    {
        try
        {
            CheckIfFileExists(filePath);
            var lines = File.ReadAllLines(filePath);
            Logger.Log($"LoadGameLevel - Success: {filePath}");
            return LoadLevelFromLines(lines);
        }
        catch (FileNotFoundException e)
        {
            Logger.Log($"LoadGameLevel - Failure: {filePath}. Error: File not found");
            DisplayErrorMessage("Error: The chosen file was not found", e);
            throw;
        }
        catch (FormatException e)
        {
            Logger.Log($"LoadGameLevel - Failure: {filePath}. Error: Invalid file format");
            DisplayErrorMessage("Error: The level file might be corrupted or has an invalid format", e);
            throw new IOException("Failed to load level: Invalid file format", e);
        }
        catch (IOException e)
        {
            Logger.Log($"LoadGameLevel - Failure: {filePath}. Error: I/O error");
            DisplayErrorMessage("Error: An I/O error occurred while reading file", e);
            throw;
        }
        catch (Exception e)
        {
            Logger.Log($"LoadGameLevel - Failure: {filePath}. Error: {e.Message}");
            DisplayUnexpectedError("An unexpected error occurred while loading level", e);
            throw new IOException($"Failed to load level file: {e.Message}", e);
        }
    }

    /// <summary>
    /// Loads a level of an array of strings.
    /// </summary>
    /// <param name="lines">Line array of strings containing the level data</param>
    /// <returns>A loaded level</returns>
    private static Level LoadLevelFromLines(string[] lines)
    {
        CheckFileLevelFormat(lines);

        var dimensions = RenderDimensions(lines[0]);
        // Create board with dimensions and data
        // Skip the first line (dimensions) and take the next number of lines (rows)
        IBoard board = CreateBoard(dimensions, lines.Skip(1).Take(dimensions.Item1).ToArray());

        // Calculate starting index of additional level info
        // After board data, so number of rows plus 1 (for the dimension line)
        var startIndex = dimensions.Item1 + 1;
        return new Level
        {
            Board = board,
            StartPosition = RenderPosition(lines[startIndex]),
            EndPosition = RenderPosition(lines[startIndex + 1]),
            Player = new Player { CurrentPosition = RenderPosition(lines[startIndex + 2]) },
            IsCompleted = bool.Parse(lines[startIndex + 3])
        };
    }

    /// <summary>
    /// Parses dimensions of the game board from a string
    /// </summary>
    /// <param name="dimensionsLine">String containing the board dimensions</param>
    /// <returns>A tuple of integers containing number of rows and columns</returns>
    private static (int, int) RenderDimensions(string dimensionsLine)
    {
        var dimensions = dimensionsLine.Split(Separator);
        return (int.Parse(dimensions[0]), int.Parse(dimensions[1]));
    }

    /// <summary>
    /// Creates a board from an array of strings.
    /// </summary>
    /// <param name="dimensions">Dimensions of the board</param>
    /// <param name="boardLines">Lines of an array of strings containing the board data</param>
    /// <returns>A created board</returns>
    private static Board CreateBoard((int rows, int columns) dimensions, string[] boardLines)
    {
        var board = new Board(dimensions.rows, dimensions.columns);

        for (int row = 0; row < dimensions.rows; row++)
        {
            for (int col = 0; col < dimensions.columns; col++)
            {
                char pieceChar = boardLines[row][col];
                PieceType pieceType = (PieceType)pieceChar;
                board.Cells[row, col] = new Board.Piece(pieceType);
            }
        }

        return board;
    }

    /// <summary>
    /// Renders a position from a string
    /// </summary>
    /// <param name="positionLine">String containing the position data</param>
    private static Position RenderPosition(string positionLine)
    {
        var coordinates = positionLine.Split(Separator);
        return new Position { Row = int.Parse(coordinates[0]), Column = int.Parse(coordinates[1]) };
    }
}

// References
// https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalllines?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.skip?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.take?view=net-8.0