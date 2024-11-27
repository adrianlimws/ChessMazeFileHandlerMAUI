using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer;

public class Level : ILevel
{
    public IBoard Board { get; set; }
    public IPosition StartPosition { get; set; }
    public IPosition EndPosition { get; set; }
    public IPlayer Player { get; set; }
    public bool IsCompleted { get; set; }
}