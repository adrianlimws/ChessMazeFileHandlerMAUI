namespace ChessMaze.FileHandler.Filer;

public partial class FileHandlerMain
{
    /// <summary>
    /// Generates a file path based on provided path or creates automatic file name
    /// </summary>
    /// <param name="fileType">Type of file (Level or Game). Defaults to Level</param>
    /// <param name="directory">Directory to save the file. If null, uses the current directory</param>
    /// <returns>A file path</returns>
    public string GenerateAutoFileName(string fileType = "Level", string directory = null)
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"ChessMaze_{fileType}_{timestamp}.txt";

        directory = directory ?? Directory.GetCurrentDirectory();
        string filePath = Path.Combine(directory, fileName);

        int counter = 1;
        while (File.Exists(filePath))
        {
            fileName = $"ChessMaze_{fileType}_{timestamp}_{counter}.txt";
            filePath = Path.Combine(directory, fileName);
            counter++;
        }

        return filePath;
    }
}

// References
// https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-8.0