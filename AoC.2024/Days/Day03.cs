using System.Text.RegularExpressions;
using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day03 : AoCDay
    {
        private const string Pattern = @"mul\(\d+\,\d+\)";
        private const string DoPattern = @"do\(\)";
        private const string DontPattern = @"don't\(\)";

        protected override string Day => "03";

        public override long Part1()
        {
            var regex = new Regex(Pattern);
            var contents = GetStrings();

            return contents
                .Sum(x => regex.Matches(x)
                .Select(m => new Command(m.Value))
                .Sum(c => c.Result));
        }

        public override long Part2()
        {
            var regex = new Regex(Pattern);
            var doRegex = new Regex(DoPattern);
            var dontRegex = new Regex(DontPattern);

            var initialStateEnabled = true;
            var output = 0L;

            foreach (var item in GetStrings())
            {
                var matches = regex.Matches(item);
                var matchList = new MatchList(initialStateEnabled, doRegex.Matches(item), dontRegex.Matches(item));

                foreach (var match in matches.ToList())
                {
                    if (matchList.IsEnabled(match.Index))
                    {
                        output += new Command(match.Value).Result;
                    }
                }

                initialStateEnabled = matchList.FinalStateEnabled();
            }

            return output;
        }

        private class MatchList(bool initialStateEnabled, MatchCollection doList, MatchCollection dontList)
        {
            public bool IsEnabled(int index)
            {
                var firstDo = doList.Where(x => x.Index < index).MaxBy(x => x.Index);
                var firstDont = dontList.Where(x => x.Index < index).MaxBy(x => x.Index);

                if (firstDo == null && firstDont == null)
                {
                    return initialStateEnabled;
                }

                if (firstDo == null) return false;
                if (firstDont == null) return true;

                return firstDo.Index > firstDont.Index;
            }

            public bool FinalStateEnabled() => doList.MaxBy(x => x.Index)?.Index > dontList.MaxBy(x => x.Index)?.Index;
        }

        private class Command
        {
            public long Result { get; }

            public Command(string input)
            {
                var numbers = input
                    .Replace("mul(", string.Empty)
                    .Replace(")", string.Empty)
                    .Split(',')
                    .Select(long.Parse)
                    .ToArray();

                Result = numbers[0] * numbers[1];
            }
        }
    }
}
