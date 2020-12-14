using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static AOC2020.Day14;

namespace AOC2020
{
    public class Day14 : BaseDay<Item>
    {
        private static Regex MaskRegex = new Regex(@"^mask = (?<mask>\w{36})$");
        private static Regex MemRegex = new Regex(@"^mem\[(?<address>\d+)\] = (?<value>\d+)$");

        public Day14() : base("Day14", "8566770985168", "4832039794082")
        {
        }

        protected override Item Parse(StreamReader reader)
        {
            string line = reader.ReadLine();

            Match maskMatch = MaskRegex.Match(line);
            if (maskMatch.Success)
            {
                return new Item()
                {
                    Code = Opcode.Mask,
                    Mask = maskMatch.Groups["mask"].Value
                };
            }

            Match memMatch = MemRegex.Match(line);

            return new Item()
            {
                Code = Opcode.Mem,
                Address = Int64.Parse(memMatch.Groups["address"].Value),
                Value = Int64.Parse(memMatch.Groups["value"].Value)
            };
        }

        protected override string Solve1(Item[] items)
        {
            IDictionary<long, long> mem = new Dictionary<long, long>();

            string mask = new string('X', 36);

            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                switch (item.Code)
                {
                    case Opcode.Mask:
                        mask = item.Mask;
                        break;
                    case Opcode.Mem:
                        long value = item.Value;
                        for (int j = 0; j < mask.Length; j++)
                        {
                            switch (mask[j])
                            {
                                case '0':
                                    value &= ~((long)1 << (35 - j));
                                    break;
                                case '1':
                                    value |= ((long)1 << (35 - j));
                                    break;
                            }
                        }
                        mem[item.Address] = value;
                        break;
                }
            }

            return mem.Values.Sum().ToString();
        }

        protected override string Solve2(Item[] items)
        {
            IDictionary<long, long> mem = new Dictionary<long, long>();

            string mask = new string('X', 36);

            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                switch (item.Code)
                {
                    case Opcode.Mask:
                        mask = item.Mask;
                        break;
                    case Opcode.Mem:
                        long template = 0;
                        IList<int> variables = new List<int>();
                        for (int j = 0; j < mask.Length; j++)
                        {
                            switch (mask[j])
                            {
                                case '0':
                                    template |= (item.Address & (1 << (35 - j)));
                                    break;
                                case '1':
                                    template |= ((long)1 << (35 - j));
                                    break;
                                case 'X':
                                    variables.Add(35 - j);
                                    break;
                            }
                        }

                        Queue<long> addresses = new Queue<long>();
                        addresses.Enqueue(template);

                        foreach (int index in variables)
                        {
                            Queue<long> newAddresses = new Queue<long>();
                            while (addresses.Count > 0)
                            {
                                template = addresses.Dequeue();
                                newAddresses.Enqueue(template | ((long)1 << index));
                                newAddresses.Enqueue(template & ~((long)1 << index));
                            }
                            addresses = newAddresses;
                        }

                        while (addresses.Count > 0)
                        {
                            mem[addresses.Dequeue()] = item.Value;
                        }
                        break;
                }
            }

            return mem.Values.Sum().ToString();
        }

        public class Item
        {
            public Opcode Code { get; set; }

            public string Mask { get; set; }

            public long Value { get; set; }

            public long Address { get; set; }
        }

        public enum Opcode
        {
            Mask,
            Mem
        }

    }
}
