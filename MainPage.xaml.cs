using ChessMaze.FileHandler.Filer.Interfaces;
using ChessMaze.FileHandler.Filer;
using ChessMazeFileHandlerMAUI.Services;

namespace ChessMazeFileHandlerMAUI
{
	public partial class MainPage : ContentPage
	{
		private FileHandlerMain fileHandler;
		private ILevel currentLevel;
		private string savedGamesPath;
		private readonly FileHistoryService _fileHistoryService;
		public MainPage()
		{
			InitializeComponent();

			_fileHistoryService = Application.Current.Handler.MauiContext.Services.GetService<FileHistoryService>();

			fileHandler = new FileHandlerMain();
			savedGamesPath = Path.Combine(GetDesktopPath(), "ChessMazeSavedGames");

			if (!Directory.Exists(savedGamesPath))
			{
				try
				{
					Directory.CreateDirectory(savedGamesPath);
				}
				catch (Exception ex)
				{
					// Fallback to app data directory if desktop is not accessible
					savedGamesPath = Path.Combine(FileSystem.AppDataDirectory, "SavedGames");
					Directory.CreateDirectory(savedGamesPath);
				}
			}
			RefreshFileList();
			LoadRecentHistory();
		}

		private async void LoadRecentHistory()
		{
			if (_fileHistoryService != null)
			{
				var recentOperations = await _fileHistoryService.GetRecentOperationsAsync();
				FileHistoryListView.ItemsSource = recentOperations;
			}
		}

		private string GetDesktopPath()
		{
			if (DeviceInfo.Platform == DevicePlatform.WinUI)
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
			else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Desktop");
			}
			else
			{
				// Fallback to appdata directory for other platforms that is not Windows
				return FileSystem.AppDataDirectory;
			}
		}
	}
}

/* References
 * https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-system-helpers?view=net-maui-8.0&tabs=windows
 * https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.storage.filesystem.appdatadirectory?view=net-maui-8.0#microsoft-maui-storage-filesystem-appdatadirectory
 * https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.itemsview.itemssource?view=net-maui-8.0
 * https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.devices.deviceplatform?view=net-maui-8.0
 * https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.storage.filepickerfiletype?view=net-maui-8.0
 * https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0&tabs=windows
 * https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.page.displayalert?view=net-maui-8.0#microsoft-maui-controls-page-displayalert(system-string-system-string-system-string)
 * https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/?view=net-maui-8.0
 * https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.devices.deviceinfo?view=net-maui-8.0
 * https://devblogs.microsoft.com/dotnet/file-and-folder-dialogs-communitytoolkit/
 * XAML MAUI
 * https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/scrollview?view=net-maui-8.0
 * https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/listview?view=net-maui-8.0
 * https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/grid?view=net-maui-8.0
 * https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/data-binding-basics?view=net-maui-8.0
*/