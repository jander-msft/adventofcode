using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static AOC2020.Day19;

namespace AOC2020
{
    public class Day19 : BaseDay<Item>
    {
        private static Regex LineRegex = new Regex(@"^(?<rule>\d+): (?<body>.*)$");
        private static Regex RuleRefRegex = new Regex(@"(?<reference>\d+)");

        public Day19() : base("Day19", "139", "289")
        {
        }

        protected override Item Parse(StreamReader reader)
        {
            IDictionary<int, Body> rules = new Dictionary<int, Body>();

            string line = null;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                Match lineMatch = LineRegex.Match(line);

                int rule = Int32.Parse(lineMatch.Groups["rule"].Value);
                string[] bodyElements = lineMatch.Groups["body"].Value.Split("|");

                Body body = new Body();
                foreach (string bodyElement in bodyElements)
                {
                    MatchCollection referenceMatches = RuleRefRegex.Matches(bodyElement);
                    if (referenceMatches.Count > 0)
                    {
                        IList<int> references = new List<int>();
                        foreach (Match referenceMatch in referenceMatches)
                        {
                            references.Add(Int32.Parse(referenceMatch.Groups["reference"].Value));
                        }
                        body.References.Add(references);
                    }
                    else
                    {
                        body.Literal = bodyElement.Trim(new char[] { '"', ' ' });
                    }
                }

                rules.Add(rule, body);
            }

            IList<string> values = new List<string>();
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                values.Add(line);
            }

            return new Item()
            {
                Rules = rules,
                Values = values
            };
        }

        protected override string Solve1(Item[] items)
        {
            Item item = items.First();

            string rule0 = "^" + GetRegex(0, item.Rules) + "$";

            Regex rule0Regex = new Regex(rule0);

            return item.Values.Count(v => rule0Regex.IsMatch(v)).ToString();
        }

        protected override string Solve2(Item[] items)
        {
            Item item = items.First();

            IDictionary<int, Builder> overrides = new Dictionary<int, Builder>();
            overrides.Add(8, new Rule8Part2Builder());
            overrides.Add(11, new Rule11Part2Builder());

            string rule0 = "^" + GetRegex(0, item.Rules, overrides) + "$";

            Regex rule0Regex = new Regex(rule0);

            return item.Values.Count(v => rule0Regex.IsMatch(v)).ToString();
        }

        private static string GetRegex(int rule, IDictionary<int, Body> rules)
        {
            IDictionary<int, Builder> overrides = new Dictionary<int, Builder>();
            return GetRegex(rule, rules, overrides);
        }

        private static string GetRegex(int rule, IDictionary<int, Body> rules, IDictionary<int, Builder> overrides)
        {
            IDictionary<int, string> cache = new Dictionary<int, string>();
            DefaultBuilder defaultBuilder = new DefaultBuilder();

            string GetRegexInternal(int rule, IDictionary<int, Body> rules, IDictionary<int, Builder> overrides)
            {
                if (!overrides.TryGetValue(rule, out Builder builder))
                {
                    builder = defaultBuilder;
                }
                return builder.Build(rules[rule], r => GetRegexInternal(r, rules, overrides));
            }

            return GetRegexInternal(rule, rules, overrides);
        }

        public class Item
        {
            public IDictionary<int, Body> Rules { get; set; }

            public IList<string> Values { get; set; }
        }

        public class Body
        {
            public IList<IEnumerable<int>> References { get; set; }
                = new List<IEnumerable<int>>();

            public string Literal { get; set; }
        }

        public abstract class Builder
        {
            public abstract string Build(Body body, Func<int, string> getRegex);
        }

        public class DefaultBuilder : Builder
        {
            public override string Build(Body body, Func<int, string> getRegex)
            {
                if (body.References.Any())
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (IEnumerable<int> references in body.References)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("|");
                        }
                        foreach (int reference in references)
                        {
                            builder.Append("(");
                            builder.Append(getRegex(reference));
                            builder.Append(")");
                        }
                    }

                    return builder.ToString();
                }
                else
                {
                    return body.Literal;
                }
            }
        }

        public class Rule8Part2Builder : Builder
        {
            public override string Build(Body body, Func<int, string> getRegex)
            {
                return "(" + getRegex(42) + ")+";
            }
        }

        public class Rule11Part2Builder : Builder
        {
            public override string Build(Body body, Func<int, string> getRegex)
            {
                string r42 = getRegex(42);
                string r31 = getRegex(31);

                // Reasonable approximation of ab, aabb, aaabbb, etc
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 5; i++)
                {
                    if (i > 0)
                    {
                        builder.Append("|");
                    }
                    builder.Append("(");
                    for (int j = 0; j <= i; j++)
                    {
                        builder.Append("(");
                        builder.Append(r42);
                        builder.Append(")");
                    }
                    for (int j = 0; j <= i; j++)
                    {
                        builder.Append("(");
                        builder.Append(r31);
                        builder.Append(")");
                    }
                    builder.Append(")");
                }
                return builder.ToString();
            }
        }
    }
}
