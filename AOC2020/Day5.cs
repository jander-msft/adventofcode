using System;
using System.IO;
using System.Linq;
using static AOC2020.Day5;

namespace AOC2020
{
    public class Day5 : BaseDay<Seat>
    {
        public Day5() : base("Day5", "933", "711")
        {
        }

        protected override Seat Parse(StreamReader reader)
        {
            ReadOnlySpan<char> span = reader.ReadLine();

            return new Seat()
            {
                Row = span.Slice(0, 7).ToInt32('B'),
                Column = span.Slice(7, 3).ToInt32('R')
            };
        }

        protected override string Solve1(Seat[] items)
        {
            return items.Max(s => SeatId(s)).ToString();
        }

        protected override string Solve2(Seat[] items)
        {
            // Find row with single missing seat
            IGrouping<int, Seat> row = items
                .GroupBy(s => s.Row)
                .Single(g => g.Count() == 7 && g.Key != 0 & g.Key != 127);

            // Determine column of missing seat
            int column = Enumerable.Range(0, 8)
                .Except(row.Select(s => s.Column))
                .Single();

            return SeatId(row.Key, column).ToString();
        }

        private static int SeatId(Seat seat)
        {
            return SeatId(seat.Row, seat.Column);
        }

        private static int SeatId(int row, int column)
        {
            return 8 * row + column;
        }

        public class Seat
        {
            public int Row { get; set; }

            public int Column { get; set; }
        }
    }
}
