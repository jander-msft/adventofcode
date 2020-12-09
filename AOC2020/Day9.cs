using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day9 : BaseDay<long>
    {
        public Day9() : base("Day9", "23278925", "4011064")
        {
        }

        protected override long Parse(StreamReader reader)
        {
            return Int64.Parse(reader.ReadLine());
        }

        protected override string Solve1(long[] items)
        {
            const int windowSize = 25;

            int windowIndex = 0;
            IList<long> window = new List<long>(items.Take(windowSize));

            for (int itemIndex = windowSize; itemIndex < items.Length; itemIndex++)
            {
                if (!window.FindTwoSum(items[itemIndex], out _, out _))
                {
                    return items[itemIndex].ToString();
                }

                window[windowIndex] = items[itemIndex];
                windowIndex = (windowIndex + 1) % windowSize;
            }

            throw new InvalidOperationException();
        }

        protected override string Solve2(long[] items)
        {
            const long targetSum = 23278925;

            ReadOnlySpan<long> range;

            long sum;
            int start = 0;
            int end = 1;
            do
            {
                range = items.AsSpan(start, end - start + 1);
                sum = range.Sum();

                if (sum > targetSum)
                {
                    start++;
                }
                else if (sum < targetSum)
                {
                    end++;
                }
            }
            while (sum != targetSum);

            return (range.Min() + range.Max()).ToString();
        }
    }
}
