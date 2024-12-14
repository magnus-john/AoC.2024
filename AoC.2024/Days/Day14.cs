using System.Drawing;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day14 : AoCDay
    {
#if DEBUG
        private const int Width = 11;
        private const int Height = 7;
#else
        private const int Width = 101;
        private const int Height = 103;
#endif

        private readonly List<Robot> _robots = [];
        private readonly List<Quadrant> _quadrants;

        protected override string Day => "14";

        public Day14()
        {
            _robots.AddRange(GetStrings().Select(x => new Robot(x)));

            const int midX = (Width - 1) / 2;
            const int midY = (Height - 1) / 2;

            _quadrants =
            [
                new Quadrant(new Point(0, 0), new Point(midX - 1, midY - 1)),
                new Quadrant(new Point(midX + 1, 0), new Point(midX * 2, midY - 1)),
                new Quadrant(new Point(0, midY + 1), new Point(midX - 1, midY * 2)),
                new Quadrant(new Point(midX + 1, midY + 1), new Point(midX * 2, midY * 2))
            ];
        }

        public override long Part1()
        {
            Enumerable.Range(0, 100).ToList().ForEach(_ => _robots.ForEach(Move));

            return _quadrants
                .Select(q => _robots.Count(r => q.Contains(r.Position)))
                .Aggregate(1L, (current, total) => current * total);
        }

        public override long Part2()
        {
            var i = 0L;

            while(true)
            {
                _robots.ForEach(Move);
                i++;

                if (ContainDupes()) continue;

                DrawBoard();
                return i;
            }
        }

        private static void Move(Robot r)
        {
            var newPosition = new Point(r.Position.X + r.Velocity.X, r.Position.Y + r.Velocity.Y);

            if (newPosition.X < 0) newPosition.X += Width;
            if (newPosition.X >= Width) newPosition.X -= Width;
            if (newPosition.Y < 0) newPosition.Y += Height;
            if (newPosition.Y >= Height) newPosition.Y -= Height;

            r.Position = newPosition;
        }

        private void DrawBoard()
        {
            Console.Clear();

            foreach (var y in Enumerable.Range(0, Height))
            {
                var output = Enumerable.Range(0, Width)
                    .Select(x => _robots.Count(r => r.Position == new Point(x, y)))
                    .Aggregate(string.Empty, (current, count) => current + (count > 0 ? count : "."));

                Console.WriteLine(output);
            }
        }

        private bool ContainDupes()
        {
            return (
                from y in Enumerable.Range(0, Height) 
                from x in Enumerable.Range(0, Width) 
                where _robots.Count(r => r.Position == new Point(x, y)) > 1 
                select y).Any();
        }

        public class Quadrant(Point topLeft, Point bottomRight)
        {
            public bool Contains(Point p) => p.X >= topLeft.X
                                             && p.X <= bottomRight.X
                                             && p.Y >= topLeft.Y
                                             && p.Y <= bottomRight.Y;
        }

        private class Robot
        {
            public Point Position { get; set; }

            public Point Velocity { get; }

            public Robot(string input)
            {
                var entries = input
                    .Replace("p=", "")
                    .Replace("v=", "")
                    .Split(" ")
                    .Select(x => x.Split(",").Select(int.Parse).ToArray())
                    .ToArray();

                Position = new Point(entries[0][0], entries[0][1]);
                Velocity = new Point(entries[1][0], entries[1][1]);
            }
        }
    }
}
