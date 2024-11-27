using System.Text;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer;

public partial class FileHandlerMain
{
    /// <summary>
    /// Loads a compressed level from a file
    /// </summary>
    /// <param name="filePath">Path of file to be loaded</param>
    /// <returns>A loaded level</returns>
    /// <exception cref="IOException">Thrown when there's an error loading the file</exception>
    public ILevel LoadCompressedLevel(string filePath)
    {
        try
        {
            CheckIfFileExists(filePath);
            var lines = File.ReadAllLines(filePath);

            if (!ValidateCompressedInput(lines))
            {
                throw new FormatException("Invalid compressed level format");
            }

            var level = LoadLevelFromCompressedLines(lines);

            if (!ValidateExpandedLevel(level))
            {
                throw new FormatException("Expanded level data is invalid");
            }

            return level;
        }
        catch (FormatException e)
        {
            DisplayErrorMessage("Error: Invalid compressed level format", e);
            throw new IOException($"Failed to load compressed level: {e.Message}", e);
        }
        catch (Exception e)
        {
            DisplayUnexpectedError("An unexpected error occurred while loading compressed level", e);
            throw new IOException($"Failed to load compressed level file: {e.Message}", e);
        }
    }

    /// <summary>
    /// Loads level from compressed data
    /// </summary>
    /// <param name="lines">Compressed data containing lines of level</param>
    /// <returns>The loaded level</returns>
    private ILevel LoadLevelFromCompressedLines(string[] lines)
    {
        var dimensions = RenderDimensions(lines[0]);
        string boardData = lines[1];

        if (boardData.Contains('E') && boardData.Any(char.IsDigit))
        {
            _converter.Expand(boardData);
            boardData = _converter.Expanded;
        }

        var expandedBoardLines = boardData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        if (expandedBoardLines.Length != dimensions.Item1)
        {
            throw new FormatException("Expanded board data does not match expected dimensions");
        }

        IBoard board = CreateBoard(dimensions, expandedBoardLines);

        return new Level
        {
            Board = board,
            StartPosition = RenderPosition(lines[2]),
            EndPosition = RenderPosition(lines[3]),
            Player = new Player { CurrentPosition = RenderPosition(lines[4]) },
            IsCompleted = bool.Parse(lines[5])
        };
    }

    /// <summary>
    /// Validates compressed input data
    /// </summary>
    /// <param name="lines">Lines of compressed data to be validated</param>
    /// <returns>True if the input is valid</returns>
    private bool ValidateCompressedInput(string[] lines)
    {
        if (lines.Length < _minLinesValidLevelState) return false;

        var dimensions = lines[0].Split(Separator);

        // Ensure there are precisely two dimensions (rows and columns) and both are valid integers
        if (dimensions.Length != 2 || !int.TryParse(dimensions[0], out _) || !int.TryParse(dimensions[1], out _))
            return false;
        // Check the second line (compressed board data) is not empty or whitespace
        if (string.IsNullOrWhiteSpace(lines[1]))
            return false;

        // Validate next 3 lines (start, end & player position)
        for (int i = 2; i <= 4; i++)
        {
            var position = lines[i].Split(Separator);

            // Make sure each position has exactly two coordinates and are valid integers
            if (position.Length != 2 || !int.TryParse(position[0], out _) || !int.TryParse(position[1], out _))
                return false;
        }

        return bool.TryParse(lines[5], out _);
    }


    /// <summary>
    /// Validates an expanded level
    /// </summary>
    /// <param name="level">Level to be validated</param>
    /// <returns>True if the level is valid</returns>
    private bool ValidateExpandedLevel(ILevel level)
    {
        return level.Board.Rows > 0 &&
               level.Board.Columns > 0 &&
               level.StartPosition != null &&
               level.EndPosition != null &&
               level.Player?.CurrentPosition != null &&
               IsValidPosition(level.StartPosition, level.Board) &&
               IsValidPosition(level.EndPosition, level.Board) &&
               IsValidPosition(level.Player.CurrentPosition, level.Board);
    }

    /// <summary>
    /// Checks if a position is valid within the loaded board
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <param name="board">Board to check</param>
    /// <returns>True if the position is valid</returns>
    private bool IsValidPosition(IPosition position, IBoard board)
    {
        return position.Row >= 0 && position.Row < board.Rows &&
               position.Column >= 0 && position.Column < board.Columns;
    }
}

// References
// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/discards 
// https://stackoverflow.com/questions/42920622/c7-underscore-star-in-out-variable 

/*
 * Note to Future Self;
 * Interesting bit about line 94 using "out _" discards
 * in the context when int.TryParse converts a string to an integer it returns a boolean to indicate
 * true/false(success or fail).
 * If true => it outputs the parsed integer to an out parameter
 * According to docs, by using "out _" it tells the compiler that this method outputs a value BUT
 * ignore the value and only want to know if the parse is successful
 *
 * more readable example
 * int value;
 * if (int.TryParse(dimensions[0], out value)) { }
 *
 * <TL:DR>
 * Using "out _" let us check if the string can be parsed as an integer without creating another variable,
 * makes code more precise and indicates that (me, the dev) to other devs it's deliberately ignoring the output value. :D...
 * PS: int myBrain; if(int.TryParse(cells[0], out myBrain))
 */