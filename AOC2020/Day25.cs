using System;
using System.IO;

namespace AOC2020
{
    public class Day25 : BaseDay<long>
    {
        public Day25() : base("Day25", "9420461", null)
        {
        }

        protected override long Parse(StreamReader reader)
        {
            return Int64.Parse(reader.ReadLine());
        }

        protected override string Solve1(long[] items)
        {
            long doorPublic = items[0];
            long cardPublic = items[1];

            long value = 1;
            long loopCount = 0;

            while (value != cardPublic)
            {
                value = (7 * value) % 20201227;
                loopCount++;
            }

            value = 1;
            while (loopCount-- > 0)
            {
                value = (doorPublic * value) % 20201227;
            }

            return value.ToString();
        }

        protected override string Solve2(long[] items)
        {
            return null;
        }
    }
}
