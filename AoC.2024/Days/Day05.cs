using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day05 : AoCDay
    {
        protected override string Day => "05";

        private readonly List<Rule> _rules = [];
        private readonly List<Update> _updates = [];

        public Day05()
        {
            var processingRules = true;

            foreach (var line in GetStrings())
            {
                if (line == string.Empty)
                    processingRules = false;
                else
                {
                    if (processingRules)
                        _rules.Add(new Rule(line));
                    else
                        _updates.Add(new Update(line));
                }
            }
        }

        public override long Part1()
        {
            return _updates
                .Where(AllRulesSatisfied)
                .Aggregate(0L, (current, x) => current + GetMiddlePage(x));
        }

        public override long Part2()
        {
            return _updates
                .Where(x => !AllRulesSatisfied(x))
                .Aggregate(0L, (current, x) => current + GetMiddlePage(ReOrderUpdate(x)));
        }

        private bool AllRulesSatisfied(Update update)
        {
            foreach (var (entry, index) in update.Select((x, i) => (x, i)))
            {
                if (_rules
                    .Where(x => x.FirstPage == entry)
                    .Select(x => update.IndexOf(x.SecondPage))
                    .Any(x => x != -1 && x < index))
                {
                    return false;
                }
            }

            return true;
        }

        private static int GetMiddlePage(Update update) => update[(update.Count - 1) / 2];

        private Update ReOrderUpdate(Update input)
        {
            var rules = new Dictionary<int, int>();

            foreach (var entry in input)
            {
                var applicableRules = _rules.Where(x => x.FirstPage == entry && input.Contains(x.SecondPage));

                rules.Add(entry, applicableRules.Count());
            }

            var newUpdate = string.Join(',', rules.OrderBy(x => x.Value).Select(x => x.Key));

            return new Update(newUpdate);
        }

        private class Rule(string input)
        {
            public int FirstPage => int.Parse(input.Split('|')[0]);
            public int SecondPage => int.Parse(input.Split('|')[1]);
        }

        private class Update : List<int>
        {
            public Update(string input)
            {
                AddRange(input.Split(',').Select(int.Parse));
            }
        }
    }
}
