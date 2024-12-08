using System.Drawing;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day08 : AoCDay
    {
        private readonly List<Antenna> _antennae = [];
        private readonly List<AntiNode> _antiNodes = [];

        protected override string Day => "08";

        public Day08()
        {
            var lines = GetStrings();

            foreach (var (line, y) in lines.Select((x, i) => (x, i)))
            foreach (var (c, x) in line.Select((x, i) => (x, i)))
            {
                if (c != '.') 
                    _antennae.Add(new Antenna(c, x, y));
            }

            foreach (var frequency in _antennae.Select(x => x.Frequency).Distinct())
            foreach (var antenna in _antennae.Where(x => x.Frequency == frequency))
            {
                var otherAntennae = _antennae
                    .Where(x => x.Frequency == antenna.Frequency && x.Position != antenna.Position)
                    .Select(a => new AntiNode(antenna, a, lines.Count));

                _antiNodes.AddRange(otherAntennae);
            }
        }

        public override long Part1()
        {
            return new HashSet<Point>(_antiNodes.SelectMany(x => x.InitialLocations)).Count;
        }

        public override long Part2()
        {
            return new HashSet<Point>(_antiNodes.SelectMany(x => x.AllLocations).Union(_antennae.Select(x => x.Position))).Count;
        }

        private class Antenna(char frequency, int x, int y)
        {
            public char Frequency => frequency;

            public Point Position => new(x, y);
        }

        private class AntiNode
        {
            private readonly int _size;

            public List<Point> AllLocations { get; } = [];
            public List<Point> InitialLocations { get; } = [];

            public AntiNode(Antenna first, Antenna second, int size)
            {
                _size = size;

                var xVector = first.Position.X - second.Position.X;
                var yVector = first.Position.Y - second.Position.Y;

                AddLocations(first, xVector, yVector);
                AddLocations(second, xVector * -1, yVector * -1);
            }

            private void AddLocations(Antenna antenna, int xVector, int yVector)
            {
                var newLocation = new Point(antenna.Position.X + xVector, antenna.Position.Y + yVector);

                AddLocation(newLocation);

                while (AddLocation(newLocation, isInitialLocation: false))
                {
                    newLocation = new Point(newLocation.X + xVector, newLocation.Y + yVector);
                }
            }

            private bool AddLocation(Point p, bool isInitialLocation = true)
            {
                if (p.X < 0 || p.X >= _size || p.Y < 0 || p.Y >= _size) 
                    return false;

                if (isInitialLocation) 
                    InitialLocations.Add(p);
                else 
                    AllLocations.Add(p);

                return true;
            }
        }
    }
}
