using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day01 : AoCDay
    {
        protected override string Day => "01";

        // 1603498
        public override long Part1()
        {
            var (first, second) = GetLists(sortOutput: true);

            return first.Select((x, i) => Math.Abs(x - second[i])).Sum();
        }

        // 25574739
        public override long Part2()
        {
            var (first, second) = GetLists();

            return first.Sum(x => x * second.Count(y => x == y));
        }

        public (List<long>, List<long>) GetLists(bool sortOutput = false)
        {
            var first = new List<long>();
            var second = new List<long>();

            foreach (var numbers in GetStrings().Select(x => x.Split("   ")))
            {
                first.Add(long.Parse(numbers[0]));
                second.Add(long.Parse(numbers[1]));
            }

            if (sortOutput)
            {
                first.Sort();
                second.Sort();
            }

            return (first, second);
        }
    }
}
