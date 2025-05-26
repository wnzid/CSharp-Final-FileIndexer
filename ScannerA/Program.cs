using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScannerA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ScannerA - Word Frequency Counter";

            Console.WriteLine("=========================================");
            Console.WriteLine("         ScannerA is starting up         ");
            Console.WriteLine("=========================================\n");

            Console.WriteLine("This program will scan .txt files in a folder.");
            Console.WriteLine("Let’s start by choosing the folder to scan.\n");

            Console.Write("Enter the full path to the folder: ");
            string folderPath = Console.ReadLine();

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("\nThe folder doesn’t exist. Please check the path and try again.");
            }
            else
            {
                Console.WriteLine("\nFolder found. Scanning for .txt files...\n");

                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

                if (txtFiles.Length == 0)
                {
                    Console.WriteLine("No .txt files found in the folder.");
                }
                else
                {
                    Console.WriteLine("Found the following .txt files:");
                    foreach (string file in txtFiles)
                    {
                        Console.WriteLine("- " + Path.GetFileName(file));
                    }

                    Console.WriteLine("\nCounting words in each file...\n");

                    Dictionary<string, Dictionary<string, int>> wordCounts = new();

                    foreach (string filePath in txtFiles)
                    {
                        string fileName = Path.GetFileName(filePath);
                        wordCounts[fileName] = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                        try
                        {
                            string content = File.ReadAllText(filePath);
                            string[] words = Regex.Split(content, @"\W+");

                            foreach (string word in words)
                            {
                                if (string.IsNullOrWhiteSpace(word))
                                    continue;

                                if (!wordCounts[fileName].ContainsKey(word))
                                    wordCounts[fileName][word] = 1;
                                else
                                    wordCounts[fileName][word]++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error reading " + fileName + ": " + ex.Message);
                        }
                    }

                    Console.WriteLine("Final formatted output:\n");

                    foreach (var fileEntry in wordCounts)
                    {
                        foreach (var wordEntry in fileEntry.Value)
                        {
                            Console.WriteLine($"{fileEntry.Key}:{wordEntry.Key}:{wordEntry.Value}");
                        }
                    }
                }
            }

            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
        }
    }
}