using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day07 : AoCDay
    {
        private readonly List<Calibrator> _calibrators;
        private readonly Dictionary<int, List<string>> _partOneOperatorSequences = [];
        private readonly Dictionary<int, List<string>> _partTwoOperatorSequences = [];
        
        protected override string Day => "07";
        
        public Day07()
        {
            _calibrators = [.. GetStrings().Select(x => new Calibrator(x))];

            foreach (var i in Enumerable.Range(1, _calibrators.Max(x => x.Values.Count)))
            {
                _partOneOperatorSequences.Add(i, Explode("*+", string.Empty, i).ToList());
                _partTwoOperatorSequences.Add(i, Explode("*+|", string.Empty, i).ToList());
            }
        }

        public override long Part1()
        {
            return _calibrators
                .Where(x => x.CanBeSolved(_partOneOperatorSequences[x.Values.Count - 1]))
                .Sum(x => x.Result);
        }

        public override long Part2()
        {
            return _calibrators
                .Where(x => x.CanBeSolved(_partTwoOperatorSequences[x.Values.Count - 1]))
                .Sum(x => x.Result);
        }

        private static IEnumerable<string> Explode(string operators, string current, int toGo)
        {
            if (toGo == 0)
                yield return current;

            if (toGo < 0)
                yield break;

            foreach (var output in operators.SelectMany(o => Explode(operators, $"{current}{o}", toGo - 1)))
                yield return output;
        }

        private class Calibrator
        {
            public long Result { get; }

            public List<long> Values { get; }

            public Calibrator(string input)
            {
                var sides = input.Split(':');

                Result = long.Parse(sides[0]);
                Values = [.. sides[1].Trim().Split(' ').Select(long.Parse)];
            }

            public bool CanBeSolved(List<string> operators) => operators.Any(ComputesToResult);

            private bool ComputesToResult(string operators)
            {
                var current = Values[0];

                foreach (var (op, i) in operators.Select((x, i) => (x, i)))
                {
                    var next = Values[i + 1];

                    current = op switch
                    {
                        '*' => current * next,
                        '+' => current + next,
                        _ => long.Parse($"{current}{next}")
                    };
                }

                return current == Result;
            }
        }
    }
}
