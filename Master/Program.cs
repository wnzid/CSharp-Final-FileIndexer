using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Master
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Master - Named Pipe Server";

            Console.WriteLine("=========================================");
            Console.WriteLine("         Master is waiting...            ");
            Console.WriteLine("=========================================\n");

            string pipeName = "agent1";

            using NamedPipeServerStream pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In);

            Console.WriteLine($"Waiting for connection on pipe: {pipeName}");
            pipeServer.WaitForConnection();
            Console.WriteLine("Connected to ScannerA.\n");

            using StreamReader reader = new StreamReader(pipeServer, Encoding.UTF8);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine("Received: " + line);
            }

            Console.WriteLine("\nConnection closed. Press any key to exit.");
            Console.ReadKey();
        }
    }
}