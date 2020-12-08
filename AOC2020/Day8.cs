using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using static AOC2020.Day8;

namespace AOC2020
{
    public class Day8 : BaseDay<Instruction>
    {
        private static Regex LineRegex = new Regex(@"^(?<code>\w{3}) (?<operand>[+-]\d+)$");

        public Day8() : base("Day8", "1262", "1643")
        {
        }

        protected override Instruction Parse(StreamReader reader)
        {
            Match lineMatch = LineRegex.Match(reader.ReadLine());

            Instruction item = new Instruction();
            item.Code = Enum.Parse<Opcode>(lineMatch.Groups["code"].Value, ignoreCase: true);
            item.Operand = Int32.Parse(lineMatch.Groups["operand"].Value, NumberStyles.Integer);
            return item;
        }

        protected override string Solve1(Instruction[] items)
        {
            Simulate(items, out int accumulator);

            return accumulator.ToString();
        }

        protected override string Solve2(Instruction[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Code == Opcode.Jmp)
                {
                    items[i].Code = Opcode.Nop;

                    if (Simulate(items, out int accumulator))
                    {
                        return accumulator.ToString();
                    }

                    items[i].Code = Opcode.Jmp;
                }
                else if (items[i].Code == Opcode.Nop)
                {
                    items[i].Code = Opcode.Jmp;

                    if (Simulate(items, out int accumulator))
                    {
                        return accumulator.ToString();
                    }

                    items[i].Code = Opcode.Nop;
                }
            }

            throw new InvalidOperationException();
        }

        private static bool Simulate(Instruction[] instructions, out int accumulator)
        {
            accumulator = 0;

            HashSet<int> visited = new HashSet<int>();

            int index = 0;
            while (index < instructions.Length)
            {
                if (!visited.Add(index))
                {
                    return false;
                }

                Instruction instruction = instructions[index];
                switch (instruction.Code)
                {
                    case Opcode.Acc:
                        accumulator += instruction.Operand;
                        index++;
                        break;
                    case Opcode.Jmp:
                        index += instruction.Operand;
                        break;
                    case Opcode.Nop:
                        index++;
                        break;
                }
            }

            return true;
        }

        public class Instruction
        {
            public Opcode Code { get; set; }

            public int Operand { get; set; }
        }

        public enum Opcode
        {
            Nop,
            Acc,
            Jmp
        }
    }
}
