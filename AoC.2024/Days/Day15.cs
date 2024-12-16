using System.Drawing;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day15 : AoCDay
    {
        protected override string Day => "15";

        private readonly List<Point> _directions = [];
        private readonly List<Point> _boxes = [];
        private readonly List<Point> _walls = [];
        private readonly List<LargeBox> _largeBoxes = [];
        private readonly List<Point> _largeWalls = [];

        private Point _robot;
        private Point _largeRobot;

        public Day15()
        {
            var strings = GetStrings();
            var processingDirections = false;

            foreach (var (line, y) in strings.Select((x, i) => (x, i)))
            {
                if (processingDirections)
                {
                    _directions.AddRange(line.Select(GetDirection));
                }
                else
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        processingDirections = true;
                    }
                    else
                    {
                        foreach (var (c, x) in line.Select((x, i) => (x, i)))
                        {
                            var p = new Point(x, y);

                            switch (c)
                            {
                                case '#': 
                                    _walls.Add(p);
                                    _largeWalls.AddRange(new LargeBox(p).Points);
                                    break;

                                case 'O':
                                    _boxes.Add(p);
                                    _largeBoxes.Add(new LargeBox(p));
                                    break;

                                case '@':
                                    _robot = p;
                                    _largeRobot = new Point(x * 2, y);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public override long Part1()
        {
            _directions.ForEach(Move);

            return _boxes.Sum(x => x.X + (x.Y * 100));
        }

        public override long Part2()
        {
            _directions.ForEach(Move2);

            return _largeBoxes.Sum(x => x.Points[0].X + (x.Points[0].Y * 100));
        }

        private static Point GetDirection(char c) => c switch
        {
            '<' => new Point(-1, 0),
            '>' => new Point(1, 0),
            '^' => new Point(0, -1),
            'v' => new Point(0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(c))
        };

        private void Move(Point direction)
        {
            var boxesTilSpace = new List<Point>();
            var next = new Point(_robot.X + direction.X, _robot.Y + direction.Y);

            while (true)
            {
                if (_walls.Contains(next))
                {
                    return;
                }

                if (_boxes.Contains(next))
                {
                    boxesTilSpace.Add(_boxes[_boxes.IndexOf(next)]);
                }
                else
                {
                    break;
                }

                next = new Point(next.X + direction.X, next.Y + direction.Y);
            }

            foreach (var box in boxesTilSpace)
            {
                _boxes[_boxes.IndexOf(box)] = new Point(box.X + direction.X, box.Y + direction.Y);
            }

            _robot = new Point(_robot.X + direction.X, _robot.Y + direction.Y);
        }

        private void Move2(Point direction)
        {
            var found = new HashSet<string>();
            var current = new List<Point> { _largeRobot };
            var next = Next(current, direction);

            while (true)
            {
                if (next == null) return;
                if (next.Count == 0) break;

                next.ForEach(box => found.Add(box.HashCode));

                if (direction.X == 0)
                {
                    current = next.SelectMany(box => box.Points).ToList();
                }
                else
                {
                    var endBox = direction.X == 1 ? next.Last() : next.First();
                    current = direction.X == 1 ? [endBox.Points[1]] : [endBox.Points[0]];
                }

                next = Next(current, direction);
            }

            foreach (var box in found.Select(hashCode => _largeBoxes.First(x => x.HashCode == hashCode)))
            {
                box.Points = [..box.Points.Select(p => new Point(p.X + direction.X, p.Y + direction.Y))]; 
            }

            _largeRobot = new Point(_largeRobot.X + direction.X, _largeRobot.Y + direction.Y);
        }

        private List<LargeBox>? Next(IEnumerable<Point> points, Point direction)
        {
            var found = new HashSet<string>();

            foreach (var initial in points)
            {
                var next = new Point(initial.X + direction.X, initial.Y + direction.Y);

                if (_largeWalls.Contains(next)) return null;

                var box = _largeBoxes.FirstOrDefault(x => x.Points.Contains(next));

                if (box != null) found.Add(box.HashCode);
            }

            return found.Select(hashCode => _largeBoxes.First(box => box.HashCode == hashCode)).ToList();
        }

        private class LargeBox(Point p)
        {
            public List<Point> Points =
            [
                p with { X = p.X * 2 },
                p with { X = p.X * 2 + 1 }
            ];

            public string HashCode => $"{Points[0].X}.{Points[0].Y}:{Points[1].X}.{Points[1].Y}";
        }
    }
}
