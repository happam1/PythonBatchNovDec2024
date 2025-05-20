
Program
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exercise2_MultiThreading
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: <program> <inputFilePath> <outputFilePath>");
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];

            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Error: Input file not found.");
                return;
            }

            try
            {
                var lines = await File.ReadAllLinesAsync(inputPath);
                var numbers = lines.Select(line => int.TryParse(line, out int n) ? n : (int?)null)
                                   .Where(n => n.HasValue)
                                   .Select(n => n.Value)
                                   .ToList();

                var squared = new int[numbers.Count];

                Parallel.For(0, numbers.Count, i =>
                {
                    squared[i] = numbers[i] * numbers[i];
                });

                await File.WriteAllLinesAsync(outputPath, squared.Select(n => n.ToString()));

                Console.WriteLine("Processing complete. Output written to " + outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}


