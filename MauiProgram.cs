using ChessMazeFileHandlerMAUI.Data;
using ChessMazeFileHandlerMAUI.Services;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChessMazeFileHandlerMAUI
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			string dbPath = Path.Combine(FileSystem.AppDataDirectory, "filehistory.db");

			builder.Services.AddSingleton<ApplicationDbContext>(provider =>
			{
				var options = new DbContextOptionsBuilder<ApplicationDbContext>()
					.UseSqlite($"Data Source={dbPath}")
					.Options;
				return new ApplicationDbContext(options);
			});

			builder.Services.AddSingleton<FileHistoryService>();

#if DEBUG
			builder.Logging.AddDebug();
#endif
			return builder.Build();
		}
	}
}
