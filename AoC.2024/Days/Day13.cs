using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day13 : AoCDay
    {
        private readonly List<Machine> _machines = [];

        protected override string Day => "13";

        public Day13()
        {
            var current = new List<string>();

            foreach (var line in GetStrings())
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    _machines.Add(new Machine(current));
                    current = [];
                }
                else
                {
                    current.Add(line);
                }
            }

            _machines.Add(new Machine(current));
        }

        public override long Part1()
        {
            return _machines.Sum(x => x.CheapestCost());
        }

        public override long Part2()
        {
            return _machines.Sum(x => x.ActualCheapestCost());
        }

        private class Machine
        {
            private const long ACost = 3;
            private const long BCost = 1;
            private const long PrizeOffset = 10000000000000;

            private Button A { get; } 
            private Button B { get; }

            private long PrizeX { get; }
            private long PrizeY { get; }

            private long ActualPrizeX => PrizeX + PrizeOffset;
            private long ActualPrizeY => PrizeY + PrizeOffset;

            public Machine(List<string> lines)
            {
                A = new Button(lines[0]);
                B = new Button(lines[1]);

                var entries = lines[2].Replace(",", "").Split();

                PrizeX = int.Parse(entries[1].Replace("X=", ""));
                PrizeY = int.Parse(entries[2].Replace("Y=", ""));
            }

            public long CheapestCost()
            {
                var output = long.MaxValue;

                foreach (var a in Enumerable.Range(0, 100))
                foreach (var b in Enumerable.Range(0, 100))
                {
                    var xOffset = a * A.X + b * B.X;
                    var yOffset = a * A.Y + b * B.Y;

                    if (xOffset != PrizeX || yOffset != PrizeY) continue;

                    var cost = Cost(a, b);

                    if (cost < output)
                    {
                        output = cost;
                    }
                }

                return output == long.MaxValue ? 0L : output;
            }

            public long ActualCheapestCost()
            {
                decimal determinant = (A.X * B.Y - A.Y * B.X);
                var a = (ActualPrizeX * B.Y - ActualPrizeY * B.X) / determinant;
                var b = (A.X * ActualPrizeY - A.Y * ActualPrizeX) / determinant;

                return decimal.IsInteger(a) && decimal.IsInteger(b)
                    ? Cost((long)a, (long)b)
                    : 0L;
            }

            private static long Cost(long a, long b) => a * ACost + b * BCost;
        }

        private class Button
        {
            public long X { get; }

            public long Y { get; }

            public Button(string input)
            {
                var entries = input.Replace(",", "").Split(" ");

                X = long.Parse(entries[2].Replace("X+", ""));
                Y = long.Parse(entries[3].Replace("Y+", ""));
            }
        }
    }
}
