using ChessMazeFileHandlerMAUI.Data;

namespace ChessMazeFileHandlerMAUI
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
		}
	}
}
