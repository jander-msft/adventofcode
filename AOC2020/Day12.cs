using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using static AOC2020.Day12;

namespace AOC2020
{
    public class Day12 : BaseDay<Instruction>
    {
        private delegate void InstructionAction(VirtualMachine vm, int count);

        private static readonly InstructionAction LeftPuzzle1 = (vm, count) =>
            vm.ShipDirection = vm.ShipDirection.RotateCCW(count / 90);
        private static readonly InstructionAction RightPuzzle1 = (vm, count) =>
            vm.ShipDirection = vm.ShipDirection.RotateCW(count / 90);
        private static readonly InstructionAction ForwardPuzzle1 = (vm, count) =>
            vm.ShipLocation = vm.ShipLocation.Add(count, vm.ShipDirection);
        private static readonly InstructionAction EastPuzzle1 = (vm, count) =>
            vm.ShipLocation = vm.ShipLocation.Add(count, Direction.East);
        private static readonly InstructionAction NorthPuzzle1 = (vm, count) =>
            vm.ShipLocation = vm.ShipLocation.Add(count, Direction.North);
        private static readonly InstructionAction WestPuzzle1 = (vm, count) =>
            vm.ShipLocation = vm.ShipLocation.Add(count, Direction.West);
        private static readonly InstructionAction SouthPuzzle1 = (vm, count) =>
            vm.ShipLocation = vm.ShipLocation.Add(count, Direction.South);

        private static readonly IDictionary<string, InstructionAction> Puzzle1Actions =
            new Dictionary<string, InstructionAction>(StringComparer.Ordinal)
            {
                { "L", LeftPuzzle1 },
                { "R", RightPuzzle1 },
                { "F", ForwardPuzzle1 },
                { "E", EastPuzzle1 },
                { "N", NorthPuzzle1 },
                { "W", WestPuzzle1 },
                { "S", SouthPuzzle1 }
            };

        private static readonly InstructionAction LeftPuzzle2 = (vm, count) =>
            vm.WaypointLocation = vm.WaypointLocation.RotateCCW(count / 90);
        private static readonly InstructionAction RightPuzzle2 = (vm, count) =>
            vm.WaypointLocation = vm.WaypointLocation.RotateCW(count / 90);
        private static readonly InstructionAction ForwardPuzzle2 = (vm, count) =>
            vm.ShipLocation += vm.WaypointLocation.Multiply(count);
        private static readonly InstructionAction EastPuzzle2 = (vm, count) =>
            vm.WaypointLocation = vm.WaypointLocation.Add(count, Direction.East);
        private static readonly InstructionAction NorthPuzzle2 = (vm, count) =>
            vm.WaypointLocation = vm.WaypointLocation.Add(count, Direction.North);
        private static readonly InstructionAction WestPuzzle2 = (vm, count) =>
            vm.WaypointLocation = vm.WaypointLocation.Add(count, Direction.West);
        private static readonly InstructionAction SouthPuzzle2 = (vm, count) =>
            vm.WaypointLocation = vm.WaypointLocation.Add(count, Direction.South);

        private static readonly IDictionary<string, InstructionAction> Puzzle2Actions =
            new Dictionary<string, InstructionAction>(StringComparer.Ordinal)
            {
                { "L", LeftPuzzle2 },
                { "R", RightPuzzle2 },
                { "F", ForwardPuzzle2 },
                { "E", EastPuzzle2 },
                { "N", NorthPuzzle2 },
                { "W", WestPuzzle2 },
                { "S", SouthPuzzle2 }
            };

        private static readonly Regex LineRegex = new Regex(@"^(?<code>\w)(?<count>\d+)$");

        public Day12() : base("Day12", "2297", "89984")
        {
        }

        protected override Instruction Parse(StreamReader reader)
        {
            string line = reader.ReadLine();

            Match lineMatch = LineRegex.Match(line);

            return new Instruction()
            {
                Code = lineMatch.Groups["code"].Value,
                Count = Int32.Parse(lineMatch.Groups["count"].Value)
            };
        }

        protected override string Solve1(Instruction[] items)
        {
            VirtualMachine vm = new VirtualMachine();
            vm.ShipLocation = LongPoint.Zero;
            vm.ShipDirection = Direction.East;

            Execute(vm, items, Puzzle1Actions);

            return vm.ShipLocation.Manhattan.ToString();
        }

        protected override string Solve2(Instruction[] items)
        {
            VirtualMachine vm = new VirtualMachine();
            vm.ShipLocation = LongPoint.Zero;
            vm.ShipDirection = Direction.East;
            vm.WaypointLocation = new LongPoint(10, 1);

            Execute(vm, items, Puzzle2Actions);

            return vm.ShipLocation.Manhattan.ToString();
        }

        private static void Execute(VirtualMachine vm, Instruction[] instructions, IDictionary<string, InstructionAction> actions)
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                Instruction instruction = instructions[i];

                if (!actions.TryGetValue(instruction.Code, out InstructionAction action))
                    throw new InvalidOperationException();

                action(vm, instruction.Count);
            }
        }

        public class Instruction
        {
            public string Code { get; set; }

            public int Count { get; set; }
        }

        public class VirtualMachine
        {
            public Direction ShipDirection { get; set; }

            public LongPoint ShipLocation { get; set; }

            public LongPoint WaypointLocation { get; set; }
        }
    }
}
