using System;

namespace AOC2020
{
    public class LongPoint4D : IEquatable<LongPoint4D>
    {
        public static readonly LongPoint4D Zero = new LongPoint4D(0, 0, 0, 0);

        public LongPoint4D(long x, long y, long z, long w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public LongPoint4D Add(LongPoint4D other)
        {
            return new LongPoint4D(X + other.X, Y + other.Y, Z + other.Z, W + other.W);
        }

        public LongPoint4D AddX(long value)
        {
            return new LongPoint4D(X + value, Y, Z, W);
        }

        public LongPoint4D AddY(long value)
        {
            return new LongPoint4D(X, Y + value, Z, W);
        }

        public LongPoint4D AddZ(long value)
        {
            return new LongPoint4D(X, Y, Z + value, W);
        }

        public LongPoint4D AddW(long value)
        {
            return new LongPoint4D(X, Y, Z, W + value);
        }

        public LongPoint4D Offset(long dx, long dy, long dz, long dw)
        {
            return new LongPoint4D(X + dx, Y + dy, Z + dz, W + dw);
        }

        public LongPoint4D Multiply(long factor)
        {
            return new LongPoint4D(factor * X, factor * Y, factor * Z, factor * W);
        }

        public LongPoint4D Subtract(LongPoint4D other)
        {
            return new LongPoint4D(X - other.X, Y - other.Y, Z - other.Z, W - other.W);
        }

        public static LongPoint4D operator +(LongPoint4D left, LongPoint4D right)
        {
            return left.Add(right);
        }

        public static LongPoint4D operator -(LongPoint4D left, LongPoint4D right)
        {
            return left.Subtract(right);
        }

        public static LongPoint4D operator *(long left, LongPoint4D right)
        {
            return right.Multiply(left);
        }

        public static LongPoint4D operator *(LongPoint4D left, long right)
        {
            return left.Multiply(right);
        }

        public static bool operator ==(LongPoint4D left, LongPoint4D right)
        {
            if (!Object.ReferenceEquals(null, left))
            {
                return left.Equals(right);
            }
            return Object.ReferenceEquals(null, right);
        }

        public static bool operator !=(LongPoint4D left, LongPoint4D right)
        {
            if (!Object.ReferenceEquals(null, left))
            {
                return !left.Equals(right);
            }
            return !Object.ReferenceEquals(null, right);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LongPoint4D);
        }

        public bool Equals(LongPoint4D obj)
        {
            if (null == obj)
                return false;

            return obj.X == X && obj.Y == Y && obj.Z == Z && obj.W == W;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

        public long X { get; }

        public long Y { get; }

        public long Z { get; }

        public long W { get; }
    }
}
