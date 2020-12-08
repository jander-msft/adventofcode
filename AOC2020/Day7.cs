using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static AOC2020.Day7;

namespace AOC2020
{
    public class Day7 : BaseDay<Bag>
    {
        private static Regex LineRegex = new Regex(@"^(?<name>.*) bags contain (?<content>.*).$");
        private static Regex ContentRegex = new Regex(@"(?<count>\d+) (?<name>[\w ]+) bags?");

        private readonly IDictionary<string, Bag> _bags =
            new Dictionary<string, Bag>(StringComparer.Ordinal);

        public Day7() : base("Day7", "254", "6006")
        {
        }

        protected override Bag Parse(StreamReader reader)
        {
            Bag GetOrCreate(string color)
            {
                if (!_bags.TryGetValue(color, out Bag bag))
                {
                    bag = new Bag(color);
                    _bags.Add(color, bag);
                }
                return bag;
            }

            Match lineMatch = LineRegex.Match(reader.ReadLine());

            Bag bag = GetOrCreate(lineMatch.Groups["name"].Value);

            foreach (Match contentMatch in ContentRegex.Matches(lineMatch.Groups["content"].Value))
            {
                Bag containedBag = GetOrCreate(contentMatch.Groups["name"].Value);

                containedBag.Containers.Add(bag);

                bag.Contains.Add(
                    containedBag,
                    Int32.Parse(contentMatch.Groups["count"].Value));
            }

            return bag;
        }

        protected override string Solve1(Bag[] items)
        {
            static void Ancestors(Bag bag, HashSet<Bag> ancestors)
            {
                foreach (Bag container in bag.Containers)
                {
                    if (ancestors.Add(container) && container.Containers.Count > 0)
                    {
                        Ancestors(container, ancestors);
                    }
                }
            }

            HashSet<Bag> ancestors = new HashSet<Bag>();

            Ancestors(items.First(i => i.Color == "shiny gold"), ancestors);

            return ancestors.Count.ToString();
        }

        protected override string Solve2(Bag[] items)
        {
            static int BagCount(Bag bag)
            {
                return bag.Contains.Sum(p => (1 + BagCount(p.Key)) * p.Value);
            }

            return BagCount(items.First(i => i.Color == "shiny gold")).ToString();
        }

        [DebuggerDisplay("{Color,nq}")]
        public class Bag : IEquatable<Bag>
        {
            public Bag(string color)
            {
                Color = color;
            }

            public string Color { get; }

            public IDictionary<Bag, int> Contains { get; } =
                new Dictionary<Bag, int>();

            public IList<Bag> Containers { get; } =
                new List<Bag>();

            public bool Equals([AllowNull] Bag other)
            {
                if (null == other)
                {
                    return false;
                }
                return string.Equals(Color, other.Color, StringComparison.Ordinal);
            }
        }
    }
}
