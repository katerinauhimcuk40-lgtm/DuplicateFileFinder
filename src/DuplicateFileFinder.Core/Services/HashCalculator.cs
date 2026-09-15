using System;
using System.IO;
using System.Security.Cryptography;

namespace DuplicateFileFinder.Core.Services;

public class HashCalculator : IHashCalculator
{
    public string ComputeFullHash(string filePath)
    {
        try
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hashBytes = md5.ComputeHash(stream);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
        catch
        {
            return string.Empty;
        }
    }

    public string ComputePartialHash(string filePath, int bytesToRead = 4096)
    {
        try
        {
            using var stream = File.OpenRead(filePath);
            byte[] buffer = new byte[bytesToRead];
            int readBytes = stream.Read(buffer, 0, bytesToRead);

            using var md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(buffer, 0, readBytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
        catch
        {
            return string.Empty;
        }
    }
}