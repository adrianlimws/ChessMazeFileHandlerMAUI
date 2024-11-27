using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI
{
	public partial class MainPage : ContentPage
	{
		private void OnRefreshListClicked(object sender, EventArgs e)
		{
			RefreshFileList();
		}
		private void RefreshFileList()
		{
			try
			{
				string[] files = Directory.GetFiles(savedGamesPath, "*.txt");
				List<string> fileNames = files.Select(Path.GetFileName).ToList();
				FileListView.ItemsSource = fileNames;

				DirectoryLabel.Text = $"Loading files from: {savedGamesPath}";
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", $"Failed to refresh file list: {ex.Message}", "OK");
			}
		}
	}
}
