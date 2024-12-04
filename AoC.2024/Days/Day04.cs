using System.Drawing;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day04 : AoCDay
    {
        protected override string Day => "04";

        private readonly char[,] _letters;

        private readonly List<char> _characters = ['X', 'M', 'A', 'S'];

        private readonly List<Point> _cross = 
        [
            new Point(-1, -1),
            new Point(1, -1),
            new Point(-1, 1),
            new Point(1, 1),
        ];
        
        private readonly List<Point> _directions =
        [
            new Point(-1, -1),
            new Point(-1, 0),
            new Point(-1, 1),
            new Point(0, -1),
            new Point(0, 1),
            new Point(1, -1),
            new Point(1, 0),
            new Point(1, 1)
        ];

        public Day04()
        {
            var contents = GetStrings();
            var length = contents.First().Length;

            _letters = new char[contents.Count, length];

            for (var x = 0; x < contents.Count; x++)
            for (var y = 0; y < length; y++)
            {
                _letters[x, y] = contents[x][y];
            }
        }

        public override long Part1()
        {
            var output = 0L;

            for (var x = 0; x <= _letters.GetUpperBound(0); x++)
            for (var y = 0; y <= _letters.GetUpperBound(1); y++)
            {
                output += _directions.LongCount(direction => CheckDirection(new Point(x, y), direction));
            }

            return output;
        }

        public override long Part2()
        {
            var output = 0L;

            for (var x = 0; x <= _letters.GetUpperBound(0); x++)
            for (var y = 0; y <= _letters.GetUpperBound(1); y++)
            {
                if (_letters[x, y] != 'A') continue;

                if (GetCross(x, y)?.IsValid ?? false)
                {
                    output++;
                }
            }

            return output;
        }

        private bool CheckDirection(Point start, Point direction)
        {
            var position = new Point(start.X, start.Y);

            foreach (var character in _characters)
            {
                if (!IsValid(position.X, position.Y)
                    || _letters[position.X, position.Y] != character)
                {
                    return false;
                }

                position = new Point(position.X + direction.X, position.Y + direction.Y);
            }

            return true;
        }

        private Cross? GetCross(int x, int y)
        {
            var points = new List<Point>();

            foreach (var point in _cross)
            {
                var newPoint = new Point(x + point.X, y + point.Y);

                if (!IsValid(newPoint.X, newPoint.Y))
                {
                    return null;
                }

                points.Add(newPoint);
            }

            return new Cross(
                _letters[points[0].X, points[0].Y],
                _letters[points[1].X, points[1].Y],
                _letters[points[2].X, points[2].Y],
                _letters[points[3].X, points[3].Y]);
        }

        private bool IsValid(int x, int y) => x >= 0
                                              && x <= _letters.GetUpperBound(0)
                                              && y >= 0
                                              && y <= _letters.GetUpperBound(1);

        private class Cross(char topLeft, char topRight, char bottomLeft, char bottomRight)
        {
            public bool IsValid {
                get
                {
                    char[] chars = [topLeft, topRight, bottomLeft, bottomRight];

                    if (chars.Count(x => x == 'M') != 2
                        || chars.Count(x => x == 'S') != 2)
                    {
                        return false;
                    }

                    if ((topLeft == 'M' && bottomRight == 'M')
                        || (topRight == 'M' && bottomLeft == 'M'))
                    {
                        return false;
                    }

                    return true;
                }
            } 
        }
    }
}
