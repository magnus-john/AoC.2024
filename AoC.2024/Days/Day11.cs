using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day11 : AoCDay
    {
        private Dictionary<long, long> _stones = [];

        protected override string Day => "11";

        public Day11()
        {
            GetNumbersOnFirstLine(" ").ForEach(x => _stones.Add(x, 1));
        }

        public override long Part1()
        {
            Blink(25);

            return _stones.Values.Sum();
        }

        public override long Part2()
        {
            Blink(75);

            return _stones.Values.Sum();
        }

        private void Blink(int times)
        {
            foreach (var _ in Enumerable.Range(0, times))
            {
                var next = new Dictionary<long, long>();

                foreach (var (key, amount) in _stones)
                {
                    var value = $"{key}";
                    var length = value.Length;

                    if (length % 2 == 0)
                    {
                        AddStone(next, long.Parse(value[(length / 2)..]), amount);
                        AddStone(next, long.Parse(value[..(length / 2)]), amount);
                    }
                    else
                        AddStone(next, key == 0 ? 1 : key * 2024, amount);
                }

                _stones = next;
            }
        }

        private static void AddStone(Dictionary<long, long> data, long key, long value)
        {
            if (!data.TryAdd(key, value)) data[key] += value;
        }
    }
}
