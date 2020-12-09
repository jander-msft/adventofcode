using System.IO;
using Xunit;

namespace AOC2020
{
    public class Day1 : BaseDay<long>
    {
        public Day1() : base("Day1", "988771", "171933104")
        {
        }

        protected override long Parse(StreamReader reader)
        {
            return long.Parse(reader.ReadLine());
        }

        protected override string Solve1(long[] items)
        {
            Assert.True(items.FindTwoSum(2020, out long value1, out long value2));

            return (value1 * value2).ToString();
        }

        protected override string Solve2(long[] items)
        {
            Assert.True(items.FindThreeSum(2020, out long value1, out long value2, out long value3));

            return (value1 * value2 * value3).ToString();
        }
    }
}
