using System.IO;

namespace AOC2020
{
    public class Day3 : BaseDay<string>
    {
        public Day3() : base("Day3", "211", "3584591857")
        {
        }

        protected override string Parse(StreamReader reader)
        {
            return reader.ReadLine();
        }

        protected override string Solve1(string[] items)
        {
            return TreeCount(items, 3, 1).ToString();
        }

        protected override string Solve2(string[] items)
        {
            return (TreeCount(items, 1, 1) *
                   TreeCount(items, 3, 1) *
                   TreeCount(items, 5, 1) *
                   TreeCount(items, 7, 1) *
                   TreeCount(items, 1, 2)).ToString();
        }

        private long TreeCount(string[] lines, int left, int down)
        {
            long count = 0;
            int column = 0;
            for (int row = 0; row < lines.Length; row += down)
            {
                string line = lines[row];
                if (line[column] == '#') count++;
                column = (column + left) % line.Length;
            }
            return count;
        }
    }
}
