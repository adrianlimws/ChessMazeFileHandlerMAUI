using ChessMaze.FileHandler.Filer;
using ChessMaze.FileHandler.Filer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI
{
	public partial class MainPage : ContentPage
	{
		private void OnCellClicked(object sender, EventArgs e)
		{
			if (sender is Button button)
			{
				int row = Grid.GetRow(button);
				int col = Grid.GetColumn(button);

				PieceType currentType = currentLevel.Board.Cells[row, col].Type;
				PieceType[] pieceTypes = (PieceType[])Enum.GetValues(typeof(PieceType));
				int nextIndex = (Array.IndexOf(pieceTypes, currentType) + 1) % pieceTypes.Length;
				PieceType newType = pieceTypes[nextIndex];

				currentLevel.Board.Cells[row, col] = new Board.Piece(newType);
				button.Text = newType.ToString();
				button.BackgroundColor = SetColorForPieceType(newType);
			}
		}
		private void UpdateLevelFromUI()
		{
			if (currentLevel == null) return;

			string[] playerPos = PlayerPositionEntry.Text.Split(',');
			if (playerPos.Length == 2 && int.TryParse(playerPos[0], out int playerRow) && int.TryParse(playerPos[1], out int playerCol))
			{
				currentLevel.Player.CurrentPosition = new Position { Row = playerRow, Column = playerCol };
			}
		}

		// helper for color pieces
		private Color SetColorForPieceType(PieceType pieceType)
		{
			return pieceType switch
			{
				PieceType.Empty => Colors.White,
				PieceType.King => Colors.Gold,
				PieceType.Rook => Colors.Red,
				PieceType.Bishop => Colors.LightBlue,
				PieceType.Knight => Colors.Green,
				PieceType.Pawn => Colors.Gray,
				_ => Colors.White
			};
		}
	}
}
