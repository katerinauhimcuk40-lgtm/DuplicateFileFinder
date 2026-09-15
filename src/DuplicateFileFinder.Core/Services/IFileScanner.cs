using System.Collections.Generic;

namespace DuplicateFileFinder.Core.Services;

public interface IFileScanner
{
    IEnumerable<string> ScanDirectory(string rootPath);
}