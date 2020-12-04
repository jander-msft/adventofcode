using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AOC2020
{
    public abstract class BaseDay<T1, T2>
    {
        private readonly string _dayName;

        public BaseDay(string dayName)
            : this(dayName, String.Empty, String.Empty)
        {
        }

        public BaseDay(string dayName, string expected1, string expected2)
        {
            _dayName = dayName;
            Expected1 = expected1;
            Expected2 = expected2;
        }

        [Fact]
        public void Puzzle1()
        {
            Assert.Equal(Expected1, Solve1(LoadPuzzle1Data()));
        }

        [Fact]
        public void Puzzle2()
        {
            Assert.Equal(Expected2, Solve2(LoadPuzzle2Data()));
        }

        protected T1[] LoadPuzzle1Data()
        {
            return LoadData("Puzzle1.txt", Parse1);
        }

        protected T2[] LoadPuzzle2Data()
        {
            return LoadData("Puzzle2.txt", Parse2);
        }

        protected abstract T1 Parse1(StreamReader reader);

        protected abstract T2 Parse2(StreamReader reader);

        protected abstract string Solve1(T1[] items);

        protected abstract string Solve2(T2[] items);

        protected T[] LoadData<T>(string fileName, Func<StreamReader, T> parse)
        {
            string filePath = Path.Combine("Input", _dayName, fileName);
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);

            IList<T> inputs = new List<T>();
            while (!reader.EndOfStream)
            {
                inputs.Add(parse(reader));
            }
            return inputs.ToArray();
        }

        protected string Expected1 { get; }

        protected string Expected2 { get; }
    }

    public abstract class BaseDay<T> : BaseDay<T, T>
    {
        public BaseDay(string dayName)
            : base(dayName, String.Empty, String.Empty)
        {
        }

        public BaseDay(string dayName, string expected1, string expected2)
            : base(dayName, expected1, expected2)
        {
        }

        protected override sealed T Parse1(StreamReader reader)
        {
            return Parse(reader);
        }

        protected override sealed T Parse2(StreamReader reader)
        {
            return Parse(reader);
        }

        protected abstract T Parse(StreamReader reader);
    }
}
