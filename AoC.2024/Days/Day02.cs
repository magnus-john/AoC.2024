using AoC2024.Days.Base;

namespace AoC2024.Days
{
    internal class Day02 : AoCDay
    {
        protected override string Day => "02";

        public override long Part1()
        {
            return GetStrings().Select(x => new Report(x)).Count(x => x.IsSafe());
        }

        public override long Part2()
        {
            return GetStrings().Select(x => new Report(x)).Count(x => x.IsSafe2());
        }

        private class Report(string input)
        {
            private const int Threshold = 3;
            private readonly List<int> _numbers = [.. input.Split(' ').Select(int.Parse)];

            public bool IsSafe() => CheckNumbers(_numbers);

            public bool IsSafe2()
            {
                var sets = new List<List<int>>
                {
                    _numbers
                };

                sets.AddRange(_numbers.Select(_ => new List<int>(_numbers)));

                for (var i = 0; i < _numbers.Count; i++)
                {
                    sets[i + 1].RemoveAt(i);
                }

                return sets.Any(CheckNumbers);
            }

            private static bool CheckNumbers(List<int> numbers)
            {
                return LevelsAreSafe(numbers) && (NumbersAreAscending(numbers) || NumbersAreDescending(numbers));
            }

            private static bool NumbersAreAscending(IEnumerable<int> numbers)
            {
                 var last = int.MinValue;

                foreach (var number in numbers)
                {
                    if (last >= number) return false;
                    last = number;
                }

                return true;
            }

            private static bool NumbersAreDescending(IEnumerable<int> numbers)
            {
                var last = int.MaxValue;

                foreach (var number in numbers)
                {
                    if (last <= number) return false;
                    last = number;
                }

                return true;
            }

            private static bool LevelsAreSafe(List<int> numbers)
            {
                var last = numbers.First();

                for (var i = 1; i < numbers.Count; i++)
                {
                    if (Math.Abs(numbers[i] - last) > Threshold) return false;
                    last = numbers[i];
                }

                return true;
            }
        }
    }
}
