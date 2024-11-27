namespace ChessMaze.FileHandler.Filer.Interfaces;

public interface IFileHandlerExtension : IFileHandler
{
    void SaveCompressedLevel(ILevel level, string filePath);
    ILevel LoadCompressedLevel(string filePath);
}