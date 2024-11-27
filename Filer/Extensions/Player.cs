using ChessMaze.FileHandler.Filer.Interfaces;
using System;

namespace ChessMaze.FileHandler.Filer
{
    public class Player : IPlayer
    {
        public IPosition CurrentPosition { get; set; }

        public bool CanMove(IPosition newPosition, IBoard board)
        {
            throw new NotImplementedException();
        }

        public void Move(IPosition newPosition, IBoard board)
        {
            throw new NotImplementedException();
        }
    }
}