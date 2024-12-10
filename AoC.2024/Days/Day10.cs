using System.Drawing;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day10 : AoCDay
    {
        private readonly int[,] _map;
        private readonly int _size;

        protected override string Day => "10";

        public Day10()
        {
            var lines = GetStrings();

            _size = lines.Count;
            _map = new int[_size, _size];

            foreach (var (line, y) in lines.Select((x, i) => (x, i)))
            foreach (var (c, x) in line.Select((x, i) => (x, i)))
            {
                _map[x, y] = int.Parse($"{c}");
            }
        }

        public override long Part1()
        {
            return GetStartingPositions().Aggregate(0L, (current, start) => current + TrailThreads(start).Distinct().Count());
        }

        public override long Part2()
        {
            return GetStartingPositions().Aggregate(0L, (current, start) => current + TrailThreads(start).Count());
        }

        private IEnumerable<Point> GetStartingPositions() => 
            from y in Enumerable.Range(0, _size)
            from x in Enumerable.Range(0, _size)
            where _map[x, y] == 0
            select new Point(x, y);

        private IEnumerable<Point> TrailThreads(Point p) => EnumerateTrail(p, 0, 9);

        private IEnumerable<Point> EnumerateTrail(Point p, int height, int target)
        {
            if (height == target) 
                yield return p;

            var next = Neighbours(p, height + 1).ToList();

            if (next.Count == 0) 
                yield break;

            foreach (var route in next.SelectMany(x => EnumerateTrail(x, height + 1, target)))
            {
                yield return route;
            }
        }

        private IEnumerable<Point> Neighbours(Point p, int height)
        {
            if (p.X > 0 && _map[p.X - 1, p.Y] == height) yield return p with { X = p.X - 1 };
            if (p.X < _size - 1 && _map[p.X + 1, p.Y] == height) yield return p with { X = p.X + 1 };
            if (p.Y > 0 && _map[p.X, p.Y - 1] == height) yield return p with { Y = p.Y - 1 };
            if (p.Y < _size - 1 && _map[p.X, p.Y + 1] == height) yield return p with { Y = p.Y + 1 };
        }
    }
}
