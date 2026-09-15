namespace DuplicateFileFinder.Core.Models;

public class FileCandidate
{
    public string FilePath { get; }
    public long Size { get; }

    public FileCandidate(string filePath, long size)
    {
        FilePath = filePath;
        Size = size;
    }
}