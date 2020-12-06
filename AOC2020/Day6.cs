using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AOC2020.Day6;

namespace AOC2020
{
    public class Day6 : BaseDay<Group>
    {
        public Day6() : base("Day6", "6430", "3125")
        {
        }

        protected override Group Parse(StreamReader reader)
        {
            Group group = new Group();
            string line = reader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                group.Answers.Add(line.Aggregate(0, (t, a) => t | 1 << (a - 'a')));

                line = reader.ReadLine();
            }
            return group;
        }

        protected override string Solve1(Group[] items)
        {
            return items.Sum(group => CountBinaryOnes(group.Answers.Aggregate((g, i) => g | i))).ToString();
        }

        protected override string Solve2(Group[] items)
        {
            return items.Sum(group => CountBinaryOnes(group.Answers.Aggregate((g, i) => g & i))).ToString();
        }

        private static int CountBinaryOnes(int value)
        {
            int count = 0;
            while (value > 0)
            {
                if (0 != (value & 1))
                {
                    count++;
                }
                value >>= 1;
            }
            return count;
        }

        public class Group
        {
            public IList<int> Answers { get; } = new List<int>();
        }
    }
}
