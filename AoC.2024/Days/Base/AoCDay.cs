namespace AoC2024.Days.Base
{
    public abstract class AoCDay
    {
        protected abstract string Day { get; }

        public abstract long Part1();
        public abstract long Part2();

        protected List<int> GetIntegers()
        {
            return [.. File.ReadAllLines($"{Environment.CurrentDirectory}/Inputs/{Day}.txt").Select(int.Parse)];
        }

        protected List<int> GetIntegersOnFirstLine()
        {
            return [.. GetStrings().First().Split(",").Select(int.Parse)];
        }

        protected List<string> GetStrings()
        {
            return [.. File.ReadAllLines($"{Environment.CurrentDirectory}/Inputs/{Day}.txt")];
        }
    }
}
