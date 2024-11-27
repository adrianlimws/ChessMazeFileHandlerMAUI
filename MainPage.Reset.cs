using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI
{
	public partial class MainPage : ContentPage
	{
		private async void OnResetClicked(object sender, EventArgs e)
		{
			if (currentLevel == null)
			{
				await DisplayAlert("Empty Data", "There is no level data to save. Please load a level first.", "OK");
				return;
			}

			bool answer = await DisplayAlert("Confirm Reset", "Are you sure you want to reset the board? Any unsaved progress will be lost.", "Yes", "No");
			if (answer)
			{
				ResetBoard();
				await DisplayAlert("Board Reset", "The board has been reset.", "OK");
			}
		}
		private void ResetBoard()
		{
			currentLevel = null;
			ChessBoardGrid.Children.Clear();
			StartPositionEntry.Text = string.Empty;
			EndPositionEntry.Text = string.Empty;
			PlayerPositionEntry.Text = string.Empty;
			IsCompletedCheckBox.IsChecked = false;
		}
	}
}
