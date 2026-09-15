using System;
using System.Collections.Generic;
using System.IO;

namespace DuplicateFileFinder.Core.Services;

public class FileScanner : IFileScanner
{
    public IEnumerable<string> ScanDirectory(string rootPath)
    {
        var files = new List<string>();
        EnumerateFilesRecursively(rootPath, files);
        return files;
    }

    private void EnumerateFilesRecursively(string path, List<string> fileList)
    {
        try
        {
            fileList.AddRange(Directory.GetFiles(path));

            foreach (var directory in Directory.GetDirectories(path))
            {
                EnumerateFilesRecursively(directory, fileList);
            }
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (DirectoryNotFoundException)
        {
        }
    }
}