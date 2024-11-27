using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer
{
    public class Position : IPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}