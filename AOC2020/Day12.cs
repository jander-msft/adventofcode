using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static AOC2020.Day12;

namespace AOC2020
{
    public class Day12 : BaseDay<Instruction>
    {
        private static Regex LineRegex = new Regex(@"^(?<code>\w)(?<count>\d+)$");

        public Day12() : base("Day12", "2297", "89984")
        {
        }

        protected override Instruction Parse(StreamReader reader)
        {
            string line = reader.ReadLine();

            Match lineMatch = LineRegex.Match(line);

            return new Instruction()
            {
                Code = lineMatch.Groups["code"].Value[0],
                Count = Int32.Parse(lineMatch.Groups["count"].Value)
            };
        }

        protected override string Solve1(Instruction[] items)
        {
            Direction direction = Direction.East;

            int x = 0;
            int y = 0;

            for (int i = 0; i < items.Length; i++)
            {
                Instruction item = items[i];
                switch (item.Code)
                {
                    case 'L':
                    case 'R':
                        direction = Rotate(direction, item.Code, item.Count / 90);
                        break;
                    case 'F':
                        switch (direction)
                        {
                            case Direction.East:
                                x -= item.Count;
                                break;
                            case Direction.North:
                                y += item.Count;
                                break;
                            case Direction.South:
                                y -= item.Count;
                                break;
                            case Direction.West:
                                x += item.Count;
                                break;
                        }
                        break;
                    case 'N':
                        y += item.Count;
                        break;
                    case 'S':
                        y -= item.Count;
                        break;
                    case 'E':
                        x -= item.Count;
                        break;
                    case 'W':
                        x += item.Count;
                        break;
                }
            }

            return (Math.Abs(x) +Math.Abs(y)).ToString();
        }

        private static Direction Rotate(Direction direction, char spin, int count)
        {
            Direction newDirection = Direction.North;
            switch (direction)
            {
                case Direction.East:
                    newDirection = spin == 'L' ? Direction.North : Direction.South;
                    break;
                case Direction.North:
                    newDirection = spin == 'L' ? Direction.West : Direction.East;
                    break;
                case Direction.South:
                    newDirection = spin == 'L' ? Direction.East : Direction.West;
                    break;
                case Direction.West:
                    newDirection = spin == 'L' ? Direction.South : Direction.North;
                    break;
            }

            if (count == 1)
            {
                return newDirection;
            }
            else
            {
                return Rotate(newDirection, spin, count - 1);
            }
        }

        private static (int, int) Rotate(int x, int y, char spin, int count)
        {
            if (spin == 'L')
            {
                int xNew = -y;
                y = x;
                x = xNew;
            }
            else
            {
                int xNew = y;
                y = -x;
                x = xNew;
            }

            if (count == 1)
            {
                return (x, y);
            }
            else
            {
                return Rotate(x, y, spin, count - 1);
            }
        }

        protected override string Solve2(Instruction[] items)
        {
            int xShip = 0;
            int yShip = 0;
            int xRelative = 10;
            int yRelative = 1;

            for (int i = 0; i < items.Length; i++)
            {
                Instruction item = items[i];
                switch (item.Code)
                {
                    case 'L':
                    case 'R':
                        (xRelative, yRelative) = Rotate(xRelative, yRelative, item.Code, item.Count / 90);
                        break;
                    case 'F':
                        xShip += item.Count * xRelative;
                        yShip += item.Count * yRelative;
                        break;
                    case 'N':
                        yRelative += item.Count;
                        break;
                    case 'S':
                        yRelative -= item.Count;
                        break;
                    case 'E':
                        xRelative += item.Count;
                        break;
                    case 'W':
                        xRelative -= item.Count;
                        break;
                }
            }

            return (Math.Abs(xShip) + Math.Abs(yShip)).ToString();
        }

        public class Instruction
        {
            public char Code { get; set; }

            public int Count { get; set; }
        }

        public enum Direction
        {
            North,
            South,
            East,
            West
        }
    }
}
