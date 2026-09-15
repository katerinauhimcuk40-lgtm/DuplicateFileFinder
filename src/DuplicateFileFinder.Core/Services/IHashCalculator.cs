namespace DuplicateFileFinder.Core.Services;

public interface IHashCalculator
{
    string ComputeFullHash(string filePath);
    string ComputePartialHash(string filePath, int bytesToRead = 4096);
}