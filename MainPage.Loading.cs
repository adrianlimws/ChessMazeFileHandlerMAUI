using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI
{
	public partial class MainPage : ContentPage
	{
		private async void OnLoadFileClicked(object sender, EventArgs e)
		{
			try
			{
				var result = await FilePicker.PickAsync(new PickOptions
				{
					FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
					{
						{ DevicePlatform.WinUI, new[] { ".txt" } }
					})
				});

				if (result != null)
				{
					string filePath = result.FullPath;
					LoadFile(filePath);
				}
				else
				{
					await DisplayAlert("Load Cancelled", "The load operation was cancelled.", "OK");
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
			}
		}

		private async void LoadFile(string filePath)
		{
			try
			{
				currentLevel = fileHandler.LoadGameLevel(filePath);
				RecentFileLabel.Text = $"Most recent file: {filePath}";
				await _fileHistoryService.LogFileOperationAsync(
					Path.GetFileName(filePath),
					filePath,
					"Read"
				);
				DisplayLoadedLevel();
				await DisplayAlert("Success", "Level loaded successfully!", "OK");
				LoadRecentHistory();
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Error loading file: {ex.Message}", "OK");
			}
		}

		private void DisplayLoadedLevel()
		{
			if (currentLevel == null || currentLevel.Board == null)
			{
				DisplayAlert("Error", "Invalid level data", "OK");
				return;
			}

			ChessBoardGrid.Children.Clear();
			ChessBoardGrid.RowDefinitions.Clear();
			ChessBoardGrid.ColumnDefinitions.Clear();

			for (int r = 0; r < currentLevel.Board.Rows; r++)
				ChessBoardGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			for (int c = 0; c < currentLevel.Board.Columns; c++)
				ChessBoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

			for (int row = 0; row < currentLevel.Board.Rows; row++)
			{
				for (int col = 0; col < currentLevel.Board.Columns; col++)
				{
					var cell = currentLevel.Board.Cells[row, col];
					var button = new Button
					{
						Text = cell.Type.ToString(),
						BackgroundColor = SetColorForPieceType(cell.Type),
						FontAttributes = FontAttributes.Bold,
						CornerRadius = 0,
						WidthRequest = 80,
						HeightRequest = 80
					};
					button.Clicked += OnCellClicked;

					ChessBoardGrid.Add(button, col, row);
				}
			}

			StartPositionEntry.Text = $"{currentLevel.StartPosition.Row},{currentLevel.StartPosition.Column}";
			EndPositionEntry.Text = $"{currentLevel.EndPosition.Row},{currentLevel.EndPosition.Column}";
			PlayerPositionEntry.Text = $"{currentLevel.Player.CurrentPosition.Row},{currentLevel.Player.CurrentPosition.Column}";
			IsCompletedCheckBox.IsChecked = currentLevel.IsCompleted;
		}
	}
}
