using System;
using System.IO;

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
                }
            }

            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
        }
    }
}