using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static AOC2020.Day13;

namespace AOC2020
{
    public class Day13 : BaseDay<Schedule>
    {
        public Day13() : base("Day13", "4938", "230903629977901")
        {
        }

        protected override Schedule Parse(StreamReader reader)
        {
            Schedule schedule = new Schedule();
            schedule.Arrive = Int64.Parse(reader.ReadLine());

            string[] busStrings = reader.ReadLine().Split(",");
            for (int i = 0; i < busStrings.Length; i++)
            {
                if (busStrings[i] != "x")
                {
                    schedule.Buses.Add(i, Int64.Parse(busStrings[i]));
                }
            }

            return schedule;
        }

        protected override string Solve1(Schedule[] items)
        {
            Schedule schedule = items.First();

            long nearestBus = 0;
            long arrivalTime = long.MaxValue;
            foreach (long bus in schedule.Buses.Values)
            {
                long candidate = 0;
                while (candidate < schedule.Arrive)
                {
                    candidate += bus;
                }
                if (candidate < arrivalTime)
                {
                    arrivalTime = candidate;
                    nearestBus = bus;
                }
            }

            return (nearestBus * (arrivalTime % schedule.Arrive)).ToString();
        }

        protected override string Solve2(Schedule[] items)
        {
            Schedule schedule = items.First();
            long arrivalTime = schedule.Arrive;

            while (!schedule.Buses.All(t => (arrivalTime + t.Key) % t.Value == 0))
            {
                long prod = 1;
                foreach (long bus in schedule.Buses.Where(t => (arrivalTime + t.Key) % t.Value == 0).Select(t => t.Value))
                {
                    prod *= bus;
                }
                arrivalTime += prod;
            }

            return arrivalTime.ToString();
        }

        public class Schedule
        {
            public long Arrive { get; set; }

            public IDictionary<int, long> Buses { get; set; } =
                new Dictionary<int, long>();
        }
    }
}
