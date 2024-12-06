using System.Drawing;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day06 : AoCDay
    {
        private readonly HashSet<Point> _visited = [];
        private readonly List<Point> _obstructions = [];
        private readonly List<State> _states = [];
        private readonly int _size;
        private readonly Point _initialPosition;

        private readonly List<Point> _directions = 
        [
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0)
        ];

        protected override string Day => "06";

        public Day06()
        {
            var lines = GetStrings();

            _size = lines.Count;

            foreach (var (line, y) in lines.Select((x, i) => (x, i)))
            foreach (var (c, x) in line.Select((x, i) => (x, i)))
            {
                switch (c)
                {
                    case '^': _initialPosition = new Point(x, y); break;
                    case '#': _obstructions.Add(new Point(x, y)); break;
                }
            }
        }

        public override long Part1()
        {
            WalkRoute();

            return _visited.Count;
        }

        public override long Part2()
        {
            WalkRoute();

            var newObstructions = new HashSet<Point>();

            var positions = _states
                .Select(x => NextPosition(x.Position, x.Direction))
                .Where(x => !IsOutOfBounds(x));

            foreach (var position in positions)
            {
                _obstructions.Add(position);

                if (CausesLoop())
                {
                    newObstructions.Add(position);
                }

                _obstructions.RemoveAt(_obstructions.Count - 1);
            }

            return newObstructions.Count;
        }

        private bool CausesLoop()
        {
            var visited = new HashSet<string>();
            var direction = 0;
            var position = new Point(_initialPosition.X, _initialPosition.Y);

            while (true)
            {
                var currentState = new State(position, direction);

                if (!visited.Add(currentState.HashCode))
                    return true;

                var nextPosition = NextPosition(position, direction);

                if (_obstructions.Contains(nextPosition))
                    direction = IncrementDirection(direction);
                else
                    position = nextPosition;

                if (IsOutOfBounds(position))
                    return false;
            }
        }

        private bool IsOutOfBounds(Point position) => position.X < 0
                                                      || position.X >= _size
                                                      || position.Y < 0
                                                      || position.Y >= _size;

        private int IncrementDirection(int direction) => direction < _directions.Count - 1 
            ? direction + 1 
            : 0;

        private Point NextPosition(Point position, int direction) => new(
            position.X + _directions[direction].X,
            position.Y + _directions[direction].Y);

        private void WalkRoute()
        {
            var direction = 0;
            var position = new Point(_initialPosition.X, _initialPosition.Y);

            while (true)
            {
                _visited.Add(position);

                var nextPosition = NextPosition(position, direction);

                if (_obstructions.Contains(nextPosition))
                    direction = IncrementDirection(direction);
                else
                {
                    _states.Add(new State(position, direction));
                    position = nextPosition;
                }

                if (IsOutOfBounds(position))
                    break;
            }
        }

        private class State(Point position, int direction)
        {
            public Point Position { get; } = position;
            public int Direction { get; } = direction;

            public string HashCode => $"{Position.X}:{Position.Y}:{Direction}";
        }
    }
}
