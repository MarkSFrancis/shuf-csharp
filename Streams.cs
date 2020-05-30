using System;
using System.IO;

namespace Shuf
{
    public class Streams : IDisposable
    {
        public Streams(TextReader input, TextWriter output, bool alsoToStdOut)
        {
            Input = input;
            Output = output;
            AlsoToStdOut = alsoToStdOut;
        }

        public TextReader Input { get; }
        public TextWriter Output { get; }
        public bool AlsoToStdOut { get; }

        public string ReadLine()
        {
            return Input.ReadLine();
        }

        public void Write(string[] outputRecords)
        {
            var textToWrite = string.Join(Environment.NewLine, outputRecords);
            Output.Write(textToWrite);

            if (AlsoToStdOut)
            {
                Console.WriteLine(textToWrite);
            }
        }

        public static Streams FromFilesOrStdInOut(FileInfo inputFile, FileInfo outputFile, bool quietMode)
        {
            TextReader input = inputFile is null ? Console.In : inputFile.OpenText();
            TextWriter output = outputFile is null ? Console.Out : outputFile.CreateText();

            return new Streams(input, output, outputFile != null && !quietMode);
        }

        public void Dispose()
        {
            Input?.Dispose();
            Output?.Dispose();
        }
    }
}