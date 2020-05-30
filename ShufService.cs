using System;
using System.Collections.Generic;
using System.Linq;

namespace Shuf
{
    public class ShufService
    {
        private static Random rnd = new Random();

        public ShufService(Streams streams, int totalToGet)
        {
            Streams = streams;
            OutputRecords = new string[totalToGet];
        }

        public Streams Streams { get; }
        public int TotalRecordsRead { get; private set; }
        public string CurrentInputRecord { get; private set; }
        public bool ReachedEndOfInput => CurrentInputRecord is null;
        public string[] OutputRecords { get; private set; }

        /// <summary>
        /// Gets whether <see cref="Run()" /> has already been called on this instance
        /// </summary>
        public bool HasRan { get; private set; }

        public void Run()
        {
            if (HasRan)
            {
                throw new InvalidOperationException("Cannot run twice on the same instance of " + nameof(ShufService));
            }

            try
            {
                // Fill random array
                FillOutput();

                RandomizeOutputFromInput();

                TrimOutput();

                WriteOutput();

            }
            finally
            {
                HasRan = true;
            }
        }

        private bool ReadNext()
        {
            CurrentInputRecord = Streams.ReadLine();
            if (!ReachedEndOfInput)
            {
                TotalRecordsRead++;
            }

            return !ReachedEndOfInput;
        }

        private void FillOutput()
        {
            var filledIndexes = new SortedSet<int>();
            while (TotalRecordsRead < OutputRecords.Length && ReadNext())
            {
                var insertTo = rnd.Next(0, OutputRecords.Length - (TotalRecordsRead - 1));
                foreach (var filledIndex in filledIndexes)
                {
                    if (filledIndex > insertTo)
                    {
                        break;
                    }
                    insertTo++;
                }

                filledIndexes.Add(insertTo);
                OutputRecords[insertTo] = CurrentInputRecord;
            }
        }

        private void RandomizeOutputFromInput()
        {
            while (ReadNext())
            {
                var insertTo = rnd.Next(0, TotalRecordsRead);
                if (insertTo < OutputRecords.Length)
                {
                    OutputRecords[insertTo] = CurrentInputRecord;
                }
            }
        }

        private void TrimOutput()
        {
            if (TotalRecordsRead < OutputRecords.Length)
            {
                OutputRecords = OutputRecords.Where(o => o != null).ToArray();
            }
        }

        private void WriteOutput()
        {
            Streams.Write(OutputRecords);
        }
    }
}
