using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AOC2020
{
    public class Day1
    {
        private readonly ITestOutputHelper _helper;

        public Day1(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void Puzzle1()
        {
            long[] expenses = LoadData("Puzzle1.txt").ToArray();
            Array.Sort(expenses);

            int start = 0;
            int end = expenses.Length - 1;

            int iterations = 0;
            long sum;
            do
            {
                sum = expenses[start] + expenses[end];
                _helper.WriteLine("expenses[{0}] + expenses[{1}] = {2}", start, end, sum);
                if (2020 == sum)
                {
                    break;
                }
                else if (sum > 2020)
                {
                    end--;
                }
                else
                {
                    start++;
                }
                iterations++;
            }
            while (start != end);

            Assert.Equal(2020, sum);
            _helper.WriteLine("Iterations: {0}", iterations);

            long product = expenses[start] * expenses[end];
            Assert.Equal(988771, product);
            _helper.WriteLine("Answer: {0}", product);
        }

        [Fact]
        public void Puzzle1Simple()
        {
            long[] expenses = LoadData("Puzzle1.txt").ToArray();
            Array.Sort(expenses);

            IList<long[]> combinations = new List<long[]>();
            for (int i = 0; i < expenses.Length; i++)
            {
                for (int j = 1; j < expenses.Length; j++)
                {
                    if (i != j)
                    {
                        combinations.Add(new[] { expenses[i], expenses[j] });
                    }
                }
            }

            long[] match = combinations.First(c => 2020 == c.Sum());
            long product = match.Aggregate((long)1, (a, b) => a * b);
            Assert.Equal(988771, product);
        }

        [Fact]
        public void Puzzle2()
        {
            long[] expenses = LoadData("Puzzle2.txt").ToArray();
            Array.Sort(expenses);

            int start = 0;
            int mid = 1;
            int end = expenses.Length - 1;

            int iterations = 0;
            long sum;
            do
            {
                sum = expenses[start] + expenses[mid] + expenses[end];
                _helper.WriteLine("expenses[{0}] + expenses[{1}] + expenses[{2}] = {3}", start, mid, end, sum);
                if (2020 == sum)
                {
                    break;
                }
                else if (sum > 2020)
                {
                    end--;
                }
                else if (start == mid - 1)
                {
                    mid++;
                }
                else if (expenses[start + 1] - expenses[start] < expenses[mid + 1] - expenses[mid])
                {
                    start++;
                }
                else
                {
                    mid++;
                }
                iterations++;
            }
            while (start != end && mid != end);

            Assert.Equal(2020, sum);
            _helper.WriteLine("Iterations: {0}", iterations);

            long product = expenses[start] * expenses[mid] * expenses[end];
            Assert.Equal(171933104, product);
            _helper.WriteLine("Answer: {0}", product);
        }

        [Fact]
        public void Puzzle2Simple()
        {
            long[] expenses = LoadData("Puzzle2.txt").ToArray();
            Array.Sort(expenses);

            IList<long[]> combinations = new List<long[]>();
            for (int i = 0; i < expenses.Length; i++)
            {
                for (int j = 1; j < expenses.Length; j++)
                {
                    for (int k = 2; k < expenses.Length; k++)
                    {
                        if (i != j && i != k && j != k)
                        {
                            combinations.Add(new[] { expenses[i], expenses[j], expenses[k] });
                        }
                    }
                }
            }

            long[] match = combinations.First(c => 2020 == c.Sum());
            long product = match.Aggregate((long)1, (a, b) => a * b);
            Assert.Equal(171933104, product);
        }

        private static IEnumerable<long> LoadData(string fileName)
        {
            string filePath = Path.Combine("Input", "Day1", fileName);
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);

            IList<long> inputs = new List<long>();

            string line;
            while (null != (line = reader.ReadLine()))
            {
                inputs.Add(long.Parse(line));
            }

            return inputs;
        }
    }
}
