using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AOC2020
{
    public class Day1 : BaseDay<long>
    {
        public Day1() : base("Day1", "988771", "171933104")
        {
        }

        protected override long Parse(string line)
        {
            return long.Parse(line);
        }

        protected override string Solve1(long[] items)
        {
            Array.Sort(items);

            int start = 0;
            int end = items.Length - 1;

            int iterations = 0;
            long sum;
            do
            {
                sum = items[start] + items[end];
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

            long product = items[start] * items[end];
            return product.ToString();
        }

        protected override string Solve2(long[] items)
        {
            Array.Sort(items);

            int start = 0;
            int mid = 1;
            int end = items.Length - 1;

            int iterations = 0;
            long sum;
            do
            {
                sum = items[start] + items[mid] + items[end];
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
                else if (items[start + 1] - items[start] < items[mid + 1] - items[mid])
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

            long product = items[start] * items[mid] * items[end];
            return product.ToString();
        }

        [Fact]
        public void Puzzle1Simple()
        {
            long[] items = LoadPuzzle1Data();
            Array.Sort(items);

            IList<long[]> combinations = new List<long[]>();
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 1; j < items.Length; j++)
                {
                    if (i != j)
                    {
                        combinations.Add(new[] { items[i], items[j] });
                    }
                }
            }

            long[] match = combinations.First(c => 2020 == c.Sum());
            long product = match.Aggregate((long)1, (a, b) => a * b);
            Assert.Equal(Expected1, product.ToString());
        }

        [Fact]
        public void Puzzle2Simple()
        {
            long[] items = LoadPuzzle2Data();
            Array.Sort(items);

            IList<long[]> combinations = new List<long[]>();
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 1; j < items.Length; j++)
                {
                    for (int k = 2; k < items.Length; k++)
                    {
                        if (i != j && i != k && j != k)
                        {
                            combinations.Add(new[] { items[i], items[j], items[k] });
                        }
                    }
                }
            }

            long[] match = combinations.First(c => 2020 == c.Sum());
            long product = match.Aggregate((long)1, (a, b) => a * b);
            Assert.Equal(Expected2, product.ToString());
        }
    }
}
