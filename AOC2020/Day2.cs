using System.Linq;
using System.Text.RegularExpressions;
using static AOC2020.Day2;

namespace AOC2020
{
    public class Day2 : BaseDay<Day2Item>
    {
        private static readonly Regex Format =
            new Regex("(\\d+)-(\\d+) (.): (.+)");

        public Day2() : base("Day2", "528", "497")
        {
        }

        protected override Day2Item Parse(string line)
        {
            var result = Format.Match(line);
            return new Day2Item()
            {
                Lower = int.Parse(result.Groups[1].Value),
                Upper = int.Parse(result.Groups[2].Value),
                Character = result.Groups[3].Value[0],
                Password = result.Groups[4].Value.Trim()
            };
        }

        protected override string Solve1(Day2Item[] items)
        {
            return items.Count(item =>
            {
                int count = item.Password.Count(c => c == item.Character);
                return item.Lower <= count && item.Upper >= count;
            }).ToString();
        }

        protected override string Solve2(Day2Item[] items)
        {
            return items.Count(item =>
            {
                return item.Password[item.Lower - 1] == item.Character ^
                    item.Password[item.Upper - 1] == item.Character;
            }).ToString();
        }

        public class Day2Item
        {
            public int Lower { get; set; }

            public int Upper { get; set; }

            public char Character { get; set; }

            public string Password { get; set; }
        }
    }
}
