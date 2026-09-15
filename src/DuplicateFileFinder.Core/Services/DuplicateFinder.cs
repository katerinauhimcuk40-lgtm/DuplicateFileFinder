using System.Collections.Generic;
using System.IO;
using System.Linq;
using DuplicateFileFinder.Core.Models;

namespace DuplicateFileFinder.Core.Services;

public class DuplicateFinder
{
    private readonly IFileScanner _scanner;
    private readonly IHashCalculator _hashCalculator;

    public DuplicateFinder(IFileScanner scanner, IHashCalculator hashCalculator)
    {
        _scanner = scanner;
        _hashCalculator = hashCalculator;
    }

    public Dictionary<string, List<string>> FindDuplicates(string rootPath)
    {
        var allFilePaths = _scanner.ScanDirectory(rootPath);

        var filesGroupedBySize = allFilePaths
            .Select(path => {
                try { return new FileCandidate(path, new FileInfo(path).Length); }
                catch { return null; }
            })
            .Where(f => f != null)
            .GroupBy(f => f!.Size)
            .Where(g => g.Count() > 1);

        var duplicatesGroupedByHash = new Dictionary<string, List<string>>();

        foreach (var sizeGroup in filesGroupedBySize)
        {
            var partialHashGroups = sizeGroup
                .GroupBy(f => _hashCalculator.ComputePartialHash(f.FilePath))
                .Where(g => !string.IsNullOrEmpty(g.Key) && g.Count() > 1);

            foreach (var partialGroup in partialHashGroups)
            {
                var fullHashGroups = partialGroup
                    .GroupBy(f => _hashCalculator.ComputeFullHash(f.FilePath))
                    .Where(g => !string.IsNullOrEmpty(g.Key) && g.Count() > 1);

                foreach (var fullGroup in fullHashGroups)
                {
                    duplicatesGroupedByHash[fullGroup.Key] = fullGroup.Select(f => f.FilePath).ToList();
                }
            }
        }

        return duplicatesGroupedByHash;
    }
}