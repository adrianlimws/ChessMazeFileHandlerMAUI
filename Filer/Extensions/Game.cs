using ChessMaze.FileHandler.Filer.Interfaces;
using System;

namespace ChessMaze.FileHandler.Filer
{
    public class Game : IGame
    {
        public ILevel CurrentLevel { get; set; }
        public int MoveCount { get; set; }

        public void LoadLevel(ILevel aLevel)
        {
            throw new NotImplementedException();
        }

        public bool MakeMove(IPosition newPosition)
        {
            throw new NotImplementedException();
        }

        public bool IsGameOver => throw new NotImplementedException();

        public int GetMoveCount()
        {
            return MoveCount;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }
    }
}