using System;
using System.IO;
using System.Text;

namespace ScannerA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ScannerA - File Reader";

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

                    Console.WriteLine("\nReading and combining content from all files...\n");

                    StringBuilder allContent = new StringBuilder();

                    foreach (string filePath in txtFiles)
                    {
                        try
                        {
                            string fileContent = File.ReadAllText(filePath);
                            allContent.AppendLine(fileContent);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error reading " + Path.GetFileName(filePath) + ": " + ex.Message);
                        }
                    }

                    Console.WriteLine("Combined content:\n");
                    Console.WriteLine(allContent.ToString());
                }
            }

            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
        }
    }
}