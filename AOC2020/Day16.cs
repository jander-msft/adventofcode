using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static AOC2020.Day16;

namespace AOC2020
{
    public class Day16 : BaseDay<Item>
    {
        private static Regex RuleRegex = new Regex(@"^(?<name>[\w\s]+): (?<r1l>\d+)-(?<r1u>\d+) or (?<r2l>\d+)-(?<r2u>\d+)$");
        private static Regex ValuesRegex = new Regex(@"(?<value>\d+)");

        public Day16() : base("Day16", "26053", "1515506256421")
        {
        }

        protected override Item Parse(StreamReader reader)
        {
            IDictionary<string, IEnumerable<Range>> rules =
                new Dictionary<string, IEnumerable<Range>>();

            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                Match ruleMatch = RuleRegex.Match(line);
                rules.Add(
                    ruleMatch.Groups["name"].Value,
                    new Range[]
                    {
                        new Range()
                        {
                            Lower = Int64.Parse(ruleMatch.Groups["r1l"].Value),
                            Upper = Int64.Parse(ruleMatch.Groups["r1u"].Value)
                        },
                        new Range()
                        {
                            Lower = Int64.Parse(ruleMatch.Groups["r2l"].Value),
                            Upper = Int64.Parse(ruleMatch.Groups["r2u"].Value)
                        }
                    });
            }

            reader.ReadLine(); // "your ticket:"
            var yourTicket = ParseTicket(reader.ReadLine()); // ticket data

            reader.ReadLine(); // empty
            reader.ReadLine(); // "nearby tickets:"
            IList<IDictionary<int, long>> nearbyTickets = new List<IDictionary<int, long>>();
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                nearbyTickets.Add(ParseTicket(line));
            }

            return new Item()
            {
                Rules = rules,
                YourTicket = yourTicket,
                NearbyTickets = nearbyTickets
            };
        }

        private static IDictionary<int, long> ParseTicket(string line)
        {
            IDictionary<int, long> map = new Dictionary<int, long>();
            MatchCollection matches = ValuesRegex.Matches(line);
            for (int i = 0; i < matches.Count; i++)
            {
                map.Add(i, Int64.Parse(matches[i].Groups["value"].Value));
            }
            return map;
        }

        protected override string Solve1(Item[] items)
        {
            Item item = items.First();

            long acc = 0;
            foreach (IDictionary<int, long> ticket in item.NearbyTickets)
            {
                foreach (long value in ticket.Values)
                {
                    if (item.Rules.Values.All(rules => !rules.Any(r => r.Within(value))))
                    {
                        acc += value;
                        break;
                    }
                }
            }

            return acc.ToString();
        }

        protected override string Solve2(Item[] items)
        {
            Item item = items.First();

            IList<IDictionary<int, long>> validTickets = new List<IDictionary<int, long>>();

            foreach (IDictionary<int, long> ticket in item.NearbyTickets)
            {
                bool isValid = true;
                foreach (long value in ticket.Values)
                {
                    if (item.Rules.Values.All(rules => !rules.Any(r => r.Within(value))))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    validTickets.Add(ticket);
                }
            }

            IDictionary<string, IList<int>> map = new Dictionary<string, IList<int>>();

            for (int i = 0; i < item.YourTicket.Count; i++)
            {
                foreach (string ruleName in item.Rules.Keys)
                {
                    IEnumerable<Range> ranges = item.Rules[ruleName];

                    if (validTickets.All(t => ranges.Any(r => r.Within(t[i]))))
                    {
                        if (!map.TryGetValue(ruleName, out IList<int> indexes))
                        {
                            indexes = new List<int>();
                            map.Add(ruleName, indexes);
                        }
                        indexes.Add(i);
                    }
                }
            }

            IList<string> remainingRules = item.Rules.Keys.ToList();

            while (map.Values.Any(i => i.Count > 1))
            {
                IList<string> singles = new List<string>();
                foreach (KeyValuePair<string, IList<int>> kvp in map)
                {
                    if (kvp.Value.Count == 1)
                    {
                        singles.Add(kvp.Key);
                    }
                }

                foreach (string ruleName in singles)
                {
                    int indexToRemove = map[ruleName][0];

                    foreach (string key in map.Keys)
                    {
                        if (ruleName != key)
                        {
                            map[key].Remove(indexToRemove);
                        }
                    }
                }
            }

            long acc = 1;
            foreach (string ruleName in map.Keys)
            {
                if (ruleName.StartsWith("departure"))
                {
                    acc *= item.YourTicket[map[ruleName][0]];
                }
            }

            return acc.ToString();
        }

        public class Range
        {
            public long Lower { get; set; }

            public long Upper { get; set; }

            public bool Within(long value)
            {
                return value >= Lower && value <= Upper;
            }
        }

        public class Item
        {
            public IDictionary<string, IEnumerable<Range>> Rules { get; set; }

            public IDictionary<int, long> YourTicket { get; set; }

            public IEnumerable<IDictionary<int, long>> NearbyTickets { get; set; }
        }
    }
}
