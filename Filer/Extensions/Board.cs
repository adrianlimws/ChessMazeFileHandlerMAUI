using ChessMaze.FileHandler.Filer.Interfaces;
using ChessMaze.FileHandler.Filer.Enums;
using System;

namespace ChessMaze.FileHandler.Filer
{
    public class Board : IBoard
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public IPiece[,] Cells { get; set; }
        
        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new IPiece[rows, columns];
        }

        public IPiece GetPieceAt(IPosition position)
        {
            throw new NotImplementedException();
        }

        public void PlacePiece(IPiece piece, IPosition position)
        {
            throw new NotImplementedException();
        }

        public void RemovePiece(IPosition position)
        {
            throw new NotImplementedException();
        }

        public void MovePiece(IPosition from, IPosition to)
        {
            throw new NotImplementedException();
        }

        public bool IsValidPosition(IPosition position)
        {
            throw new NotImplementedException();
        }

        public bool IsMoveLegal(IPosition from, IPosition to)
        {
            throw new NotImplementedException();
        }
        public class Piece : IPiece
        {
            public PieceType Type { get; }

            public Piece(PieceType type)
            {
                Type = type;
            }
			public override string ToString()
			{
				return ((char)Type).ToString();
			}
		}
    }
}