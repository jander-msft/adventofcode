using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static AOC2020.Day10;

namespace AOC2020
{
    public class Day10 : BaseDay<long>
    {
        public Day10() : base("Day10", "2346", "6044831973376")
        {
        }

        protected override long Parse(StreamReader reader)
        {
            return Int64.Parse(reader.ReadLine());
        }

        protected override string Solve1(long[] items)
        {
            Array.Sort(items);

            int oneDiffCount = 0;
            int threeDiffCount = 0;
            for (int i = 0; i < items.Length - 1; i++)
            {
                switch (items[i + 1] - items[i])
                {
                    case 1:
                        oneDiffCount++;
                        break;
                    case 3:
                        threeDiffCount++;
                        break;
                }
            }

            // Add another of the outlet, which has a value of 0; thus a joltage
            // of 1 adds to the one difference count; a joltage of 3 adss to the
            // three difference count.
            switch (items[0])
            {
                case 1:
                    oneDiffCount++;
                    break;
                case 3:
                    threeDiffCount++;
                    break;
            }

            // The device is always three above the last adapter
            threeDiffCount++;

            return (oneDiffCount * threeDiffCount).ToString();
        }

        protected override string Solve2(long[] items)
        {
            Array.Sort(items);

            List<long> joltages = new List<long>();
            joltages.Add(0); // outlet
            joltages.AddRange(items);
            joltages.Add(joltages.Last() + 3); // device

            // Map adapter to number of combinations from the adapter to device
            IDictionary<long, long> combinations = new Dictionary<long, long>();
            combinations[joltages.Count - 1] = 1;

            for (int index = joltages.Count - 2; index >= 0; index--)
            {
                long currentCount = 0;
                // Try each adapter ahead of the current adapter where its value
                // is less than or equals to 3 more than the current adapter value.
                for (int aheadIndex = index + 1; aheadIndex < joltages.Count && joltages[aheadIndex] - joltages[index] <= 3; aheadIndex++)
                {
                    currentCount += combinations[aheadIndex];
                }
                combinations.Add(index, currentCount);
            }

            return combinations[0].ToString();
        }
    }
}
