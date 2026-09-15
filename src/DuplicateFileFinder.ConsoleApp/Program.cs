using System;
using System.Diagnostics;
using DuplicateFileFinder.Core.Services;

namespace DuplicateFileFinder.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Duplicate File Finder ===");
        Console.Write("Введіть шлях до папки: ");
        string? inputPath = Console.ReadLine()?.Trim('"', ' ');

        if (string.IsNullOrWhiteSpace(inputPath) || !Directory.Exists(inputPath))
        {
            Console.WriteLine("Помилка: Вказаний шлях не існує або порожній!");
            return;
        }

        IFileScanner scanner = new FileScanner();
        IHashCalculator hashCalculator = new HashCalculator();
        var duplicateFinder = new DuplicateFinder(scanner, hashCalculator);

        Console.WriteLine($"\nСканування папки: {inputPath}...");

        var stopwatch = Stopwatch.StartNew();
        var duplicates = duplicateFinder.FindDuplicates(inputPath);
        stopwatch.Stop();

        if (duplicates.Count == 0)
        {
            Console.WriteLine("\nДублікатів не знайдено!");
        }
        else
        {
            Console.WriteLine($"\nЗнайдено груп дублікатів: {duplicates.Count}");
            int groupIndex = 1;

            foreach (var kvp in duplicates)
            {
                Console.WriteLine($"\n[Група №{groupIndex++}] Хеш: {kvp.Key}");
                foreach (var filePath in kvp.Value)
                {
                    Console.WriteLine($"  -> {filePath}");
                }
            }
        }

        Console.WriteLine($"\nСканування завершено за {stopwatch.Elapsed.TotalSeconds:F2} сек.");
    }
}