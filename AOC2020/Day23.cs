using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day23 : BaseDay<int>
    {
        public Day23() : base("Day23", "54896723", "146304752384")
        {
        }

        protected override int Parse(StreamReader reader)
        {
            return reader.Read() - '0';
        }

        protected override string Solve1(int[] items)
        {
            bool useArray = true;
            if (useArray)
            {
                int[] nextLabels = RunArray(items, part2: false);

                StringBuilder builder = new StringBuilder();
                int current = nextLabels[1];
                while (1 != current)
                {
                    builder.Append(current);
                    current = nextLabels[current];
                }
                return builder.ToString();
            }
            else
            {
                LinkedListNode<int> cup1 = RunLinkedList(items, part2: false);
                LinkedListNode<int> value = NextOrFirst(cup1);

                StringBuilder builder = new StringBuilder();
                while (value != cup1)
                {
                    builder.Append(value.Value);
                    value = NextOrFirst(value);
                }
                return builder.ToString();
            }
        }

        protected override string Solve2(int[] items)
        {
            bool useArray = true;
            if (useArray)
            {
                int[] nextLabels = RunArray(items, part2: true);
                int cupAfter1 = nextLabels[1];
                int cupAfter2 = nextLabels[cupAfter1];

                return ((long)cupAfter1 * cupAfter2).ToString();
            }
            else
            {
                LinkedListNode<int> cup1 = RunLinkedList(items, part2: true);
                LinkedListNode<int> cupAfter1 = NextOrFirst(cup1);
                LinkedListNode<int> cupAfter2 = NextOrFirst(cupAfter1);

                return ((long)cupAfter1.Value * cupAfter2.Value).ToString();
            }
        }

        private static int[] RunArray(int[] cups, bool part2)
        {
            int minValue = 1;
            int maxValue = part2 ? 1_000_000 : 9;
            int iterations = part2 ? 10_000_000 : 100;

            int[] nextLabels = new int[maxValue + 1];
            for (int i = 0; i < cups.Length - 1; i++)
            {
                nextLabels[cups[i]] = cups[i + 1];
            }
            nextLabels[cups[cups.Length - 1]] = cups[0];

            if (part2)
            {
                nextLabels[cups[cups.Length - 1]] = 10;
                for (int i = 10; i < maxValue; i++)
                {
                    nextLabels[i] = i + 1;
                }
                nextLabels[maxValue] = cups[0];
            }

            int currentLabel = cups[0];
            while (iterations > 0)
            {
                Step(currentLabel);
                iterations--;
                currentLabel = nextLabels[currentLabel];
            }

            return nextLabels;

            void Step(int currentLabel)
            {
                int pick1 = nextLabels[currentLabel];
                int pick2 = nextLabels[pick1];
                int pick3 = nextLabels[pick2];

                int destLabel = currentLabel;
                do
                {
                    destLabel--;
                    if (destLabel < minValue)
                        destLabel = maxValue;
                }
                while (destLabel == pick1 || destLabel == pick2 || destLabel == pick3);

                nextLabels[currentLabel] = nextLabels[pick3];
                nextLabels[pick3] = nextLabels[destLabel];
                nextLabels[destLabel] = pick1;
            }
        }

        private static LinkedListNode<int> RunLinkedList(int[] cups, bool part2)
        {
            int minValue = 1;
            int maxValue = part2 ? 1_000_000 : 9;
            int iterations = part2 ? 10_000_000 : 100;

            LinkedList<int> list = new LinkedList<int>();
            IDictionary<int, LinkedListNode<int>> map =
                new Dictionary<int, LinkedListNode<int>>();

            foreach (int label in cups)
            {
                map.Add(label, list.AddLast(label));
            }

            if (part2)
            {
                for (int i = 10; i <= maxValue; i++)
                {
                    map.Add(i, list.AddLast(i));
                }
            }

            LinkedListNode<int> current = list.First;
            while (iterations > 0)
            {
                Step(current);
                iterations--;
                current = NextOrFirst(current);
            }

            return map[1];

            void Step(LinkedListNode<int> current)
            {
                LinkedListNode<int> pick1 = NextOrFirst(current);
                LinkedListNode<int> pick2 = NextOrFirst(pick1);
                LinkedListNode<int> pick3 = NextOrFirst(pick2);

                int destLabel = current.Value;
                do
                {
                    destLabel--;
                    if (destLabel < minValue)
                        destLabel = maxValue;
                }
                while (pick1.Value == destLabel || pick2.Value == destLabel || pick3.Value == destLabel);

                LinkedListNode<int> destinationNode = map[destLabel];

                MoveAfter(destinationNode, pick3);
                MoveAfter(destinationNode, pick2);
                MoveAfter(destinationNode, pick1);
            }
        }

        private static LinkedListNode<int> NextOrFirst(LinkedListNode<int> node)
        {
            return node.Next ?? node.List.First;
        }

        private static void MoveAfter(LinkedListNode<int> destination, LinkedListNode<int> value)
        {
            LinkedList<int> list = value.List;
            list.Remove(value);
            if (destination.Next == null)
            {
                list.AddFirst(value);
            }
            else
            {
                list.AddAfter(destination, value);
            }
        }
    }
}
