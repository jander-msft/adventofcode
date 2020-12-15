using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static AOC2020.Day15;

namespace AOC2020
{
    public class Day15 : BaseDay<long>
    {
        public Day15() : base("Day15", "253", "13710")
        {
        }

        protected override long Parse(StreamReader reader)
        {
            return Int64.Parse(reader.ReadLine());
        }

        protected override string Solve1(long[] items)
        {
            return Play(items, 2020).ToString();
        }

        protected override string Solve2(long[] items)
        {
            return Play(items, 30000000).ToString();
        }

        private static long Play(long[] numbers, int rounds)
        {
            IDictionary<long, int> indexes = new Dictionary<long, int>();
            for (int i = 0; i < numbers.Length; i++)
            {
                indexes.Add(numbers[i], i);
            }

            int lastIndex = numbers.Length - 1;
            long last = numbers[lastIndex];
            bool first = true;
            do
            {
                if (first)
                {
                    // First spoken
                    indexes[last] = lastIndex;

                    last = 0;
                    first = false;
                }
                else
                {
                    long age = lastIndex - indexes[last];
                    indexes[last] = lastIndex;

                    first = !indexes.ContainsKey(age);
                    last = age;
                }
                lastIndex++;
            }
            while (lastIndex != (rounds - 1));

            return last;
        }
    }
}
