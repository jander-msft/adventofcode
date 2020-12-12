using System;

namespace AOC2020
{
    public class LongPoint : IEquatable<LongPoint>
    {
        public static readonly LongPoint Zero = new LongPoint(0, 0);

        public LongPoint(long x, long y)
        {
            X = x;
            Y = y;
        }

        public LongPoint Add(LongPoint other)
        {
            return new LongPoint(X + other.X, Y + other.Y);
        }

        public LongPoint Add(long value, Direction direction)
        {
            switch (direction)
            {
                case Direction.East:
                    return AddX(value);
                case Direction.North:
                    return AddY(value);
                case Direction.South:
                    return AddY(-value);
                case Direction.West:
                    return AddX(-value);
                default:
                    throw new NotSupportedException();
            }
        }

        public LongPoint AddX(long value)
        {
            return new LongPoint(X + value, Y);
        }

        public LongPoint AddY(long value)
        {
            return new LongPoint(X, Y + value);
        }

        public LongPoint Offset(long dx, long dy)
        {
            return new LongPoint(X + dx, Y + dy);
        }

        public LongPoint Multiply(long factor)
        {
            return new LongPoint(factor * X, factor * Y);
        }

        public LongPoint RotateCCW(int count = 1)
        {
            long tmpX;
            long x = X;
            long y = Y;
            while (count-- > 0)
            {
                tmpX = -y;
                y = x;
                x = tmpX;
            }
            return new LongPoint(x, y);
        }

        public LongPoint RotateCW(int count = 1)
        {
            long tmpX;
            long x = X;
            long y = Y;
            while (count-- > 0)
            {
                tmpX = y;
                y = -x;
                x = tmpX;
            }
            return new LongPoint(x, y);
        }

        public LongPoint Subtract(LongPoint other)
        {
            return new LongPoint(X - other.X, Y - other.Y);
        }

        public static LongPoint operator +(LongPoint left, LongPoint right)
        {
            return left.Add(right);
        }

        public static LongPoint operator -(LongPoint left, LongPoint right)
        {
            return left.Subtract(right);
        }

        public static LongPoint operator *(long left, LongPoint right)
        {
            return right.Multiply(left);
        }

        public static LongPoint operator *(LongPoint left, long right)
        {
            return left.Multiply(right);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LongPoint);
        }

        public bool Equals(LongPoint obj)
        {
            if (null == obj)
                return false;

            return obj.X == X && obj.Y == Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public long Manhattan => Math.Abs(X) + Math.Abs(Y);

        public long X { get; }

        public long Y { get; }
    }
}
