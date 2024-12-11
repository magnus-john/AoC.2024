namespace AoC2024.Days.Base
{
    public abstract class AoCDay
    {
        protected abstract string Day { get; }

        public abstract long Part1();
        public abstract long Part2();

        protected List<long> GetNumbers()
        {
            return [.. File.ReadAllLines($"{Environment.CurrentDirectory}/Inputs/{Day}.txt").Select(long.Parse)];
        }

        protected List<long> GetNumbersOnFirstLine(string separator)
        {
            return [.. GetStrings().First().Split(separator).Select(long.Parse)];
        }

        protected List<string> GetStrings()
        {
            return [.. File.ReadAllLines($"{Environment.CurrentDirectory}/Inputs/{Day}.txt")];
        }
    }
}
