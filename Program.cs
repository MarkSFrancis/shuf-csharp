using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Shuf
{
    class Program
    {
        static void Main(FileInfo input = null, FileInfo output = null, bool quiet = false, int n = 5)
        {
            using Streams streams = Streams.FromFilesOrStdInOut(input, output, quiet);

            var shuf = new ShufService(streams, n);
            shuf.Run();

            // MeasureProbability(input, n);
        }

        private static void MeasureProbability(FileInfo inputStream, int n)
        {
            var inputRecords = File.ReadAllLines(inputStream.FullName);
            var recordsDistribution = inputRecords.ToDictionary(r => r, r => new int[n]);

            for (int index = 0; index < 1000000; index++)
            {
                MemoryStream outputStream = new MemoryStream();
                using var streams = new Streams(
                    new StringReader(string.Join(Environment.NewLine, inputRecords)),
                    new StreamWriter(outputStream) { AutoFlush = true },
                    false);

                new ShufService(streams, n).Run();

                var outputRecords = Encoding.UTF8.GetString(outputStream.ToArray()).Split(Environment.NewLine);

                for (int outputIndex = 0; outputIndex < outputRecords.Length; outputIndex++)
                {
                    var outputDistribution = recordsDistribution[outputRecords[outputIndex]];
                    outputDistribution[outputIndex]++;
                }
            }

            // Dump to output
            Console.WriteLine("Val\tRec1\tRec2\tRec3\tRec4\tRec5");
            foreach (var dist in recordsDistribution)
            {
                Console.WriteLine($"{dist.Key}\t{string.Join('\t', dist.Value)}");
            }
        }
    }
}
