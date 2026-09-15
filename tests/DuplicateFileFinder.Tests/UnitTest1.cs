using System.Collections.Generic;
using DuplicateFileFinder.Core.Services;
using Xunit;

namespace DuplicateFileFinder.Tests;

public class DuplicateFinderTests
{
    private class FakeFileScanner : IFileScanner
    {
        private readonly IEnumerable<string> _files;
        public FakeFileScanner(IEnumerable<string> files) => _files = files;
        public IEnumerable<string> ScanDirectory(string rootPath) => _files;
    }
    private class FakeHashCalculator : IHashCalculator
    {
        private readonly Dictionary<string, string> _hashes;
        public FakeHashCalculator(Dictionary<string, string> hashes) => _hashes = hashes;

        public string ComputeFullHash(string filePath) =>
            _hashes.TryGetValue(filePath, out var hash) ? hash : "hash_default";

        public string ComputePartialHash(string filePath, int bytesToRead = 4096) =>
            ComputeFullHash(filePath);
    }

    [Fact]
    public void FindDuplicates_ShouldReturnEmpty_WhenNoFilesFound()
    {
        var scanner = new FakeFileScanner(new List<string>());
        var hashCalc = new FakeHashCalculator(new Dictionary<string, string>());
        var finder = new DuplicateFinder(scanner, hashCalc);
        var result = finder.FindDuplicates("C:\\FakeFolder");
        Assert.Empty(result);
    }
}