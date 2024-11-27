using CommunityToolkit.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI
{
	public partial class MainPage : ContentPage
	{
		private async void OnSaveFileClicked(object sender, EventArgs e)
		{
			if (currentLevel == null)
			{
				await DisplayAlert("Empty Data", "There is no level data to save. Please load a level first.", "OK");
				return;
			}

			try
			{
				UpdateLevelFromUI();

				// Generate a default file name
				string defaultFileName = $"ChessMazeLevel_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

				// Use FileSaver to show save dialog
				var fileSaveResult = await FileSaver.Default.SaveAsync(savedGamesPath, defaultFileName, new MemoryStream());

				if (fileSaveResult.IsSuccessful)
				{
					string filePath = fileSaveResult.FilePath;
					fileHandler.SaveGameLevel(currentLevel, filePath);
					RecentFileLabel.Text = $"Most recent file: {filePath}";
					await _fileHistoryService.LogFileOperationAsync(
						Path.GetFileName(filePath),
						filePath,
						"Write"
					);
					RefreshFileList();
					LoadRecentHistory();
					await DisplayAlert("Success", "Level saved successfully!", "OK");
				}
				else
				{
					await DisplayAlert("Save Cancelled", "The save operation was cancelled.", "OK");
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Error saving file: {ex.Message}", "OK");
			}
		}
	}
}
