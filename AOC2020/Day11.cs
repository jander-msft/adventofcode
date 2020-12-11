using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using static AOC2020.Day11;

namespace AOC2020
{
    public class Day11 : BaseDay<Seat[]>
    {
        public Day11() : base("Day11", "2359", "2131")
        {
        }

        protected override Seat[] Parse(StreamReader reader)
        {
            string line = reader.ReadLine();

            Seat[] row = new Seat[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '.':
                        row[i] = new Seat(State.Floor);
                        break;
                    case 'L':
                        row[i] = new Seat(State.Empty);
                        break;
                    case '#':
                        row[i] = new Seat(State.Occupied);
                        break;
                }
            }

            return row;
        }

        protected override string Solve1(Seat[][] items)
        {
            IList<Seat[]> seats = new List<Seat[]>();
            for (int i = 0; i < items.Length; i++)
                seats.Add(items[i]);

            bool changed;
            do
            {
                Step1(seats, out changed);

                for (int row = 0; row < seats.Count; row++)
                    for (int col = 0; col < seats[0].Length; col++)
                        seats[row][col].Reset();
            }
            while (changed);

            return seats.Sum(row => row.Count(s => s.Previous == State.Occupied)).ToString();
        }

        private static void Step1(IList<Seat[]> seats, out bool changed)
        {
            State Get(int row, int col)
            {
                if (row == -1 || row == seats.Count)
                    return State.Floor;
                else if (col == -1 || col == seats[0].Length)
                    return State.Floor;
                else
                    return seats[row][col].Previous;
            }

            IEnumerable<State> Around(int row, int col)
            {
                return new State[]
                {
                    Get(row - 1, col - 1),
                    Get(row - 1, col),
                    Get(row - 1, col + 1),
                    Get(row, col - 1),
                    Get(row, col + 1),
                    Get(row + 1, col - 1),
                    Get(row + 1, col),
                    Get(row + 1, col + 1)
                };
            }

            changed = false;

            IList<State[]> newSeats = new List<State[]>();
            for (int row = 0; row < seats.Count; row++)
            {
                for (int col = 0; col < seats[0].Length; col++)
                {
                    Seat seat = seats[row][col];
                    switch (seat.Previous)
                    {
                        case State.Empty:
                            if (Around(row, col).All(s => s != State.Occupied))
                            {
                                seat.Next = State.Occupied;
                                changed = true;
                            }
                            break;
                        case State.Occupied:
                            if (Around(row, col).Count(s => s == State.Occupied) >= 4)
                            {
                                seat.Next = State.Empty;
                                changed = true;
                            }
                            break;
                    }
                }
            }
        }

        protected override string Solve2(Seat[][] items)
        {
            IList<Seat[]> seats = new List<Seat[]>();
            for (int i = 0; i < items.Length; i++)
                seats.Add(items[i]);

            bool changed;
            do
            {
                Step2(seats, out changed);

                for (int row = 0; row < seats.Count; row++)
                    for (int col = 0; col < seats[0].Length; col++)
                        seats[row][col].Reset();
            }
            while (changed);

            return seats.Sum(row => row.Count(s => s.Previous == State.Occupied)).ToString();
        }

        private static void Step2(IList<Seat[]> seats, out bool changed)
        {
            int maxRow = seats.Count - 1;
            int maxCol = seats[0].Length - 1;

            State Get(int row, int col)
            {
                if (row == -1 || row == seats.Count)
                    return State.Floor;
                else if (col == -1 || col == seats[0].Length)
                    return State.Floor;
                else
                    return seats[row][col].Previous;
            }

            State CheckDir(int row, int col, int rowInc, int colInc)
            {
                while (row >= 0 && row <= maxRow && col >= 0 && col <= maxCol)
                {
                    State state = Get(row, col);
                    if (state != State.Floor)
                        return state;

                    row += rowInc;
                    col += colInc;
                }

                return State.Floor;
            }

            IEnumerable<State> Visible(int row, int col)
            {
                return new State[]
                {
                    CheckDir(row - 1, col - 1, -1, -1),
                    CheckDir(row - 1, col, -1, 0),
                    CheckDir(row - 1, col + 1, -1, 1),
                    CheckDir(row, col - 1, 0, -1),
                    CheckDir(row, col + 1, 0, 1),
                    CheckDir(row + 1, col - 1, 1, -1),
                    CheckDir(row + 1, col, 1, 0),
                    CheckDir(row + 1, col + 1, 1, 1)
                };
            }

            changed = false;

            IList<State[]> newSeats = new List<State[]>();
            for (int row = 0; row < seats.Count; row++)
            {
                for (int col = 0; col < seats[0].Length; col++)
                {
                    Seat seat = seats[row][col];
                    switch (seat.Previous)
                    {
                        case State.Empty:
                            if (Visible(row, col).All(s => s != State.Occupied))
                            {
                                seat.Next = State.Occupied;
                                changed = true;
                            }
                            break;
                        case State.Occupied:
                            if (Visible(row, col).Count(s => s == State.Occupied) >= 5)
                            {
                                seat.Next = State.Empty;
                                changed = true;
                            }
                            break;
                    }
                }
            }
        }

        public class Seat
        {
            public Seat(State state)
            {
                Previous = state;
                Next = state;
            }

            public void Reset()
            {
                Previous = Next;
            }

            public State Previous { get; set; }

            public State Next { get; set; }
        }

        public enum State
        {
            Floor,
            Empty,
            Occupied
        }
    }
}
