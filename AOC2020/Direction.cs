using System;

namespace AOC2020
{
    public enum Direction : byte
    {
        East,
        North,
        West,
        South
    }

    public static class DirectionExtensions
    {
        public static Direction RotateCW(this Direction direction, int count = 1)
        {
            while (count-- > 0)
            {
                switch (direction)
                {
                    case Direction.East:
                        direction = Direction.South;
                        break;
                    case Direction.South:
                        direction = Direction.West;
                        break;
                    case Direction.West:
                        direction = Direction.North;
                        break;
                    case Direction.North:
                        direction = Direction.East;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return direction;
        }

        public static Direction RotateCCW(this Direction direction, int count = 1)
        {
            while (count-- > 0)
            {
                switch (direction)
                {
                    case Direction.East:
                        direction = Direction.North;
                        break;
                    case Direction.North:
                        direction = Direction.West;
                        break;
                    case Direction.West:
                        direction = Direction.South;
                        break;
                    case Direction.South:
                        direction = Direction.East;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return direction;
        }
    }
}
