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
            Console.Title = "Master - Sequential Agent Receiver";

            Console.WriteLine("==========================================");
            Console.WriteLine("           Master is starting up          ");
            Console.WriteLine("==========================================\n");

            HandleAgent("agent1");

            HandleAgent("agent2");

            Console.WriteLine("\nAll agent data received. Press any key to exit.");
            Console.ReadKey();
        }

        static void HandleAgent(string pipeName)
        {
            try
            {
                using NamedPipeServerStream pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In);
                Console.WriteLine($"Waiting for connection on pipe: {pipeName}");
                pipeServer.WaitForConnection();
                Console.WriteLine($"Connected to {pipeName}.\n");

                using StreamReader reader = new StreamReader(pipeServer, Encoding.UTF8);

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"{pipeName.ToUpper()} -> {line}");
                }

                Console.WriteLine($"\n{pipeName.ToUpper()} disconnected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with {pipeName}: {ex.Message}");
            }
        }
    }
}