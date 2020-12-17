using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day17 : BaseDay<object>
    {
        public Day17() : base("Day17", "348", "2236")
        {
        }

        private static readonly LongPoint SingleInc = new LongPoint(-1, 1);

        private int _lineNumber = 0;
        private IDictionary<LongPoint4D, Item> _state =
            new Dictionary<LongPoint4D, Item>();

        protected override object Parse(StreamReader reader)
        {
            string line = reader.ReadLine();

            for (int i = 0; i < line.Length; i++)
            {
                State state = line[i] == '#' ? State.Active : State.Inactive;
                _state.Add(
                    new LongPoint4D(i, _lineNumber, 0, 0),
                    new Item() { Previous = state, Next = state });
            }
            _lineNumber++;

            return null;
        }

        protected override string Solve1(object[] items)
        {
            return Run(6, SingleInc, SingleInc, SingleInc, LongPoint.Zero).ToString();
        }

        protected override string Solve2(object[] items)
        {
            return Run(6, SingleInc, SingleInc, SingleInc, SingleInc).ToString();
        }

        private int Run(int steps, LongPoint incX, LongPoint incY, LongPoint incZ, LongPoint incW)
        {
            IList<LongPoint4D> offsets = new List<LongPoint4D>();
            for (long x = incX.X; x <= incX.Y; x++)
            {
                for (long y = incY.X; y <= incY.Y; y++)
                {
                    for (long z = incZ.X; z <= incZ.Y; z++)
                    {
                        for (long w = incW.X; w <= incW.Y; w++)
                        {
                            LongPoint4D offset = new LongPoint4D(x, y, z, w);
                            if (LongPoint4D.Zero != offset)
                            {
                                offsets.Add(offset);
                            }
                        }
                    }
                }
            }

            // Initial data size
            LongPoint newX = new LongPoint(0, 7);
            LongPoint newY = new LongPoint(0, 7);
            LongPoint newZ = new LongPoint(0, 0);
            LongPoint newW = new LongPoint(0, 0);

            do
            {
                // Increment in each direction
                newX += incX;
                newY += incY;
                newZ += incZ;
                newW += incW;

                // Compute the new state
                Step(offsets, newX, newY, newZ, newW);
            }
            while (--steps > 0);

            return _state.Values.Count(i => i.Previous == State.Active);
        }

        private State GetState(LongPoint4D point)
        {
            if (_state.TryGetValue(point, out Item item))
            {
                return item.Previous;
            }
            return State.Inactive;
        }

        private int CountActiveNeighbors(LongPoint4D point, IEnumerable<LongPoint4D> offsets)
        {
            return offsets.Count(o => State.Active == GetState(point + o));
        }

        private void Step(IEnumerable<LongPoint4D> offsets, LongPoint newX, LongPoint newY, LongPoint newZ, LongPoint newW)
        {
            for (long x = newX.X; x <= newX.Y; x++)
            {
                for (long y = newY.X; y <= newY.Y; y++)
                {
                    for (long z = newZ.X; z <= newZ.Y; z++)
                    {
                        for (long w = newW.X; w <= newW.Y; w++)
                        {
                            Update(new LongPoint4D(x, y, z, w), offsets);
                        }
                    }
                }
            }

            foreach (Item item in _state.Values)
            {
                item.Reset();
            }
        }

        private void Update(LongPoint4D point, IEnumerable<LongPoint4D> offsets)
        {
            int activeNeighbors = CountActiveNeighbors(point, offsets);

            if (!_state.TryGetValue(point, out Item item))
            {
                item = new Item() { Previous = State.Inactive, Next = State.Inactive };
                _state.Add(point, item);
            }

            switch (item.Previous)
            {
                case State.Active:
                    if (activeNeighbors == 2 || activeNeighbors == 3)
                    {
                        item.Next = State.Active;
                    }
                    else
                    {
                        item.Next = State.Inactive;
                    }
                    break;
                case State.Inactive:
                    if (activeNeighbors == 3)
                    {
                        item.Next = State.Active;
                    }
                    else
                    {
                        item.Next = State.Inactive;
                    }
                    break;
            }
        }

        public class Item
        {
            public State Previous { get; set; }

            public State Next { get; set; }

            public void Reset()
            {
                Previous = Next;
            }
        }

        public enum State
        {
            Inactive,
            Active
        }
    }
}
