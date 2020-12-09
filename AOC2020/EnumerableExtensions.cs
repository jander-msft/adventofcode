using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    public static class EnumerableExtensions
    {
        public static bool FindTwoSum(this IEnumerable<long> items, long target, out long value1, out long value2)
        {
            long[] itemArray = items.ToArray();
            Array.Sort(itemArray);

            int start = 0;
            int end = itemArray.Length - 1;

            do
            {
                value1 = itemArray[start];
                value2 = itemArray[end];
                long sum = value1 + value2;
                if (target == sum)
                {
                    return true;
                }
                else if (sum > target)
                {
                    end--;
                }
                else
                {
                    start++;
                }
            }
            while (start != end);

            return false;
        }

        public static bool FindThreeSum(this IEnumerable<long> items, long target, out long value1, out long value2, out long value3)
        {
            long[] itemArray = items.ToArray();
            Array.Sort(itemArray);

            int start = 0;
            int mid = 1;
            int end = itemArray.Length - 1;

            int iterations = 0;
            long sum;
            do
            {
                value1 = itemArray[start];
                value2 = itemArray[mid];
                value3 = itemArray[end];
                sum = value1 + value2 + value3;

                if (target == sum)
                {
                    return true;
                }
                else if (sum > target)
                {
                    end--;
                }
                else if (start == mid - 1)
                {
                    mid++;
                }
                else if (itemArray[start + 1] - itemArray[start] < itemArray[mid + 1] - itemArray[mid])
                {
                    start++;
                }
                else
                {
                    mid++;
                }
                iterations++;
            }
            while (start != end && mid != end);

            return false;
        }
    }
}
