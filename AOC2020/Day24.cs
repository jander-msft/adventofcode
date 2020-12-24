using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit.Sdk;
using static AOC2020.Day24;

namespace AOC2020
{
    public class Day24 : BaseDay<string>
    {
        public Day24() : base("Day24", "438", "4038")
        {
        }

        protected override string Parse(StreamReader reader)
        {
            return reader.ReadLine();
        }

        protected override string Solve1(string[] items)
        {
            return GenerateMap(items).Values.Count(isBlack => isBlack).ToString();
        }

        protected override string Solve2(string[] items)
        {
            IDictionary<Point, bool> map = GenerateMap(items);

            bool IsBlack(Point point)
            {
                if (!map.TryGetValue(point, out bool isBlack))
                {
                    isBlack = false;
                    map.Add(point, isBlack);
                }
                return isBlack;
            }

            static void AddAdjacentPoints(HashSet<Point> points, Point point)
            {
                points.Add(NextPoint(point, "e"));
                points.Add(NextPoint(point, "ne"));
                points.Add(NextPoint(point, "nw"));
                points.Add(NextPoint(point, "w"));
                points.Add(NextPoint(point, "sw"));
                points.Add(NextPoint(point, "se"));
            }

            bool ShouldFlip(Point point)
            {
                HashSet<Point> adjacent = new HashSet<Point>();
                AddAdjacentPoints(adjacent, point);

                int blackCount = adjacent.Count(p => IsBlack(p));
                if (IsBlack(point))
                {
                    return blackCount == 0 || blackCount > 2;
                }
                else
                {
                    return blackCount == 2;
                }
            }

            for (int i = 0; i < 100; i++)
            {
                HashSet<Point> needsFlip = new HashSet<Point>();

                IList<Point> points = new List<Point>(map.Keys);
                HashSet<Point> adjacent = new HashSet<Point>();
                foreach (Point point in points)
                {
                    AddAdjacentPoints(adjacent, point);
                }

                foreach (Point point in points.Concat(adjacent).ToHashSet())
                {
                    if (ShouldFlip(point))
                    {
                        needsFlip.Add(point);
                    }
                }

                foreach (Point point in needsFlip)
                {
                    map[point] = !map[point];
                }
            }

            return map.Values.Count(isBlack => isBlack).ToString();
        }

        private IDictionary<Point, bool> GenerateMap(string[] tiles)
        {
            IDictionary<Point, bool> map =
                new Dictionary<Point, bool>();

            foreach (string tile in tiles)
            {
                Point tilePoint = Point.Empty;
                foreach (string direction in GetDirections(tile))
                {
                    tilePoint = NextPoint(tilePoint, direction);
                }

                if (map.TryGetValue(tilePoint, out bool isBlack))
                {
                    map[tilePoint] = !isBlack;
                }
                else
                {
                    map.Add(tilePoint, true);
                }
            }

            return map;
        }

        private IEnumerable<string> GetDirections(string tile)
        {
            for (int i = 0; i < tile.Length; i++)
            {
                switch (tile[i])
                {
                    case 'e':
                    case 'w':
                        yield return tile[i].ToString();
                        break;
                    case 's':
                        i++;
                        if (tile[i] == 'e')
                            yield return "se";
                        else
                            yield return "sw";
                        break;
                    case 'n':
                        i++;
                        if (tile[i] == 'e')
                            yield return "ne";
                        else
                            yield return "nw";
                        break;
                }
            }
        }

        private static Point NextPoint(Point current, string direction)
        {
            switch (direction)
            {
                case "e":
                    return new Point(current.X + 1, current.Y);
                case "se":
                    return new Point(current.X + 1, current.Y - 1);
                case "sw":
                    return new Point(current.X, current.Y - 1);
                case "w":
                    return new Point(current.X - 1, current.Y);
                case "nw":
                    return new Point(current.X - 1, current.Y + 1);
                case "ne":
                    return new Point(current.X, current.Y + 1);
            }
            throw new NotSupportedException();
        }
    }
}
