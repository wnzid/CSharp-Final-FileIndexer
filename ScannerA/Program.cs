using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

namespace ScannerA
{
    internal class Program
    {
        static Dictionary<string, Dictionary<string, int>> wordCounts = new();

        static void Main(string[] args)
        {
            Console.Title = "ScannerA - Word Frequency Counter";

            Console.WriteLine("=========================================");
            Console.WriteLine("         ScannerA is starting up         ");
            Console.WriteLine("=========================================\n");

            while (true)
            {
                Console.Write("Enter the full path to the folder: ");
                string folderPath = Console.ReadLine();

                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine("\nThe folder doesn’t exist. Please check the path and try again.");
                }
                else
                {
                    wordCounts.Clear();

                    Thread readThread = new Thread(() => ReadAndCountWords(folderPath));
                    readThread.Start();
                    readThread.Join();

                    Console.WriteLine("\nFinal formatted output:\n");
                    foreach (var fileEntry in wordCounts)
                    {
                        foreach (var wordEntry in fileEntry.Value)
                        {
                            Console.WriteLine($"{fileEntry.Key}:{wordEntry.Key}:{wordEntry.Value}");
                        }
                    }

                    string sendAnswer;
                    while (true)
                    {
                        Console.Write("\nDo you want to send this result to the Master? (y/n): ");
                        sendAnswer = Console.ReadLine()?.Trim().ToLower();

                        if (sendAnswer == "y" || sendAnswer == "n")
                            break;
                        else
                            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    }

                    if (sendAnswer == "y")
                    {
                        Thread sendThread = new Thread(SendToMaster);
                        sendThread.Start();
                        sendThread.Join();
                    }
                    else
                    {
                        Console.WriteLine("Skipping sending data to Master.");
                    }
                }

                string continueAnswer;
                while (true)
                {
                    Console.Write("\nDo you want to scan another folder? (y/n): ");
                    continueAnswer = Console.ReadLine()?.Trim().ToLower();

                    if (continueAnswer == "y" || continueAnswer == "n")
                        break;
                    else
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                }

                if (continueAnswer != "y")
                {
                    Console.WriteLine("Exiting program. Goodbye!");
                    break;
                }

                Console.WriteLine();
            }
        }

        static void ReadAndCountWords(string folderPath)
        {
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            if (txtFiles.Length == 0)
            {
                Console.WriteLine("No .txt files found in the folder.");
                return;
            }

            Console.WriteLine("Found the following .txt files:");
            foreach (string file in txtFiles)
            {
                Console.WriteLine("- " + Path.GetFileName(file));
            }

            Console.WriteLine("\nCounting words in each file...\n");

            foreach (string filePath in txtFiles)
            {
                string fileName = Path.GetFileName(filePath);
                string content;

                try
                {
                    content = File.ReadAllText(filePath).Trim();

                    if (string.IsNullOrEmpty(content))
                    {
                        Console.WriteLine($"Skipping {fileName} (empty file)");
                        continue;
                    }

                    if (!wordCounts.ContainsKey(fileName))
                        wordCounts[fileName] = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

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
        }

        static void SendToMaster()
        {
            try
            {
                using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "agent1", PipeDirection.Out);
                pipeClient.Connect(2000);

                using StreamWriter writer = new StreamWriter(pipeClient);
                writer.AutoFlush = true;

                foreach (var fileEntry in wordCounts)
                {
                    foreach (var wordEntry in fileEntry.Value)
                    {
                        string line = $"{fileEntry.Key}:{wordEntry.Key}:{wordEntry.Value}";
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("\nAll word data sent to Master via pipe.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not connect to Master pipe: " + ex.Message);
            }
        }
    }
}