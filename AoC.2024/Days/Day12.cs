using System.Drawing;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day12 : AoCDay
    {
        private readonly int _size;
        private readonly char[,] _map;
        private readonly List<Region> _regions = [];
        private readonly HashSet<Point> _seen = [];

        protected override string Day => "12";

        public Day12()
        {
            var lines = GetStrings();
            
            _size = lines.Count;
            _map = new char[_size, _size];

            foreach (var (line, y) in lines.Select((x, i) => (x, i)))
            foreach (var (c, x) in line.Select((x, i) => (x, i)))
            {
                _map[x, y] = c;
            }

            foreach (var x in Enumerable.Range(0, _size))
            foreach (var y in Enumerable.Range(0, _size))
            {
                if (!_seen.Contains(new Point(x, y)))
                {
                    _regions.Add(EnumerateRegion(x, y));
                }
            }
        }

        public override long Part1() => _regions.Sum(x => x.Area * x.Edges);

        public override long Part2() => _regions.Sum(x => x.Area * x.Sides());

        private Region EnumerateRegion(int x, int y)
        {
            var output = new Region(_map[x, y]);

            output.AddRange(GetAllNeighbours(output.Name, new Point(x, y)));

            return output;
        }

        private List<Point> GetAllNeighbours(char c, Point p)
        {
            var output = new List<Point>();

            if (!_seen.Contains(p))
            {
                output.Add(p);
                _seen.Add(p);
            }

            Neighbours(c, p.X, p.Y).ForEach(x => output.AddRange(GetAllNeighbours(c, x)));

            return output;
        }

        private List<Point> Neighbours(char c, int x, int y)
        {
            var output = new List<Point>();

            if (x > 0 && _map[x - 1, y] == c) output.Add(new Point(x - 1, y));
            if (x < _size - 1 && _map[x + 1, y] == c) output.Add(new Point(x + 1, y));
            if (y > 0 && _map[x, y - 1] == c) output.Add(new Point(x, y - 1));
            if (y < _size - 1 && _map[x, y + 1] == c) output.Add(new Point(x, y + 1));

            output.RemoveAll(_seen.Contains);

            return output;
        }

        private class Region(char name) : List<Point>
        {
            private enum Edge
            {
                Neither,
                Both,
                First,
                Second
            }

            public char Name { get; } = name;

            public int Area => Count;

            public int Edges => this.Sum(x => 4 - Neighbours(x));

            public int Sides()
            {
                var output = 0;
                var xRange = Enumerable.Range(this.Min(x => x.X), (this.Max(x => x.X) - this.Min(x => x.X)) + 2).ToList();
                var yRange = Enumerable.Range(this.Min(x => x.Y), (this.Max(x => x.Y) - this.Min(x => x.Y)) + 2).ToList();

                foreach (var y in yRange)
                {
                    var last = Edge.Neither;

                    foreach (var current in xRange.Select(x => XEdge(x, y)))
                    {
                        if (HasStarted(last, current)) output++;

                        last = current;
                    }
                }

                foreach (var x in xRange)
                {
                    var last = Edge.Neither;

                    foreach (var current in yRange.Select(y => YEdge(x, y)))
                    {
                        if (HasStarted(last, current)) output++;

                        last = current;
                    }
                }

                return output;
            }

            private int Neighbours(Point p)
            {
                var neighbours = new[]
                {
                    p with { X = p.X - 1 },
                    p with { X = p.X + 1 },
                    p with { Y = p.Y - 1 },
                    p with { Y = p.Y + 1 }
                };

                return neighbours.Count(Contains);
            }

            private Edge XEdge(int x, int y)
            {
                var last = new Point(x, y - 1);
                var next = new Point(x, y);

                if (Contains(last)) return Contains(next) ? Edge.Both : Edge.First;

                return Contains(next) ? Edge.Second : Edge.Neither;
            }

            private Edge YEdge(int x, int y)
            {
                var last = new Point(x - 1, y);
                var next = new Point(x, y);

                if (Contains(last)) return Contains(next) ? Edge.Both : Edge.First;

                return Contains(next) ? Edge.Second : Edge.Neither;
            }

            private static bool HasStarted(Edge last, Edge current)
            {
                if (last == current) return false;

                return last switch
                {
                    Edge.Both or Edge.Neither => current is Edge.First or Edge.Second,
                    Edge.First => current == Edge.Second,
                    Edge.Second => current == Edge.First,
                    _ => false
                };
            }
        }
    }
}
