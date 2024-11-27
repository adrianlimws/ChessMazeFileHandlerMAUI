namespace ChessMaze.FileHandler.Filer.Extensions
{
    public class Logger
    {
        public static string LogFilePath { get; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileHandler.log");

        public static void Log(string message)
        {
            try
            {
                string directory = Path.GetDirectoryName(LogFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter sw = File.AppendText(LogFilePath))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Warning: Unable to write to log file - Error: {e.Message}");
            }
        }
    }
}
// References
// https://stackoverflow.com/questions/20185015/how-to-write-log-file-in-c
// https://stackoverflow.com/questions/674857/should-i-use-appdomain-currentdomain-basedirectory-or-system-environment-current/674865
// https://learn.microsoft.com/en-us/dotnet/api/System.AppDomain.BaseDirectory?view=net-8.0