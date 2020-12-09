using System;

namespace AOC2020
{
    public static class ReadOnlySpanExtensions
    {
        public static long Max(this ReadOnlySpan<long> span)
        {
            long max = long.MinValue;
            for (int i = 0; i < span.Length; i++)
            {
                max = Math.Max(max, span[i]);
            }
            return max;
        }

        public static long Min(this ReadOnlySpan<long> span)
        {
            long min = long.MaxValue;
            for (int i = 0; i < span.Length; i++)
            {
                min = Math.Min(min, span[i]);
            }
            return min;
        }

        public static long Sum(this ReadOnlySpan<long> span)
        {
            long accumulator = 0;
            for (int i = 0; i < span.Length; i++)
            {
                accumulator += span[i];
            }
            return accumulator;
        }

        public static int ToInt32(this ReadOnlySpan<char> span, char on = '1')
        {
            int value = 0;
            for (int i = 0; i < span.Length; i++)
            {
                value <<= 1;
                if (span[i] == on)
                {
                    value |= 1;
                }
            }
            return value;
        }
    }
}
