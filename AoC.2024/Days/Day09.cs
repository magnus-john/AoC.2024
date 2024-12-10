using AoC2024.Days.Base;

namespace AoC2024.Days
{
    public class Day09 : AoCDay
    {
        private readonly List<long?> _data = [];

        protected override string Day => "09";

        public Day09()
        {
            var input = GetStrings().First();
            var current = 0L;
            var isWhiteSpace = false;

            foreach (var c in input.Select(x => int.Parse($"{x}")))
            {
                _data.AddRange(Enumerable.Range(1, c).Select(_ => (long?)(isWhiteSpace ? null : current)));

                if (isWhiteSpace)
                    current++;

                isWhiteSpace = !isWhiteSpace;
            }
        }

        public override long Part1()
        {
            for (var i = _data.Count - 1; i >= 0; i--)
            {
                if (_data[i] == null) continue;

                var index = _data.IndexOf(null);

                if (index == -1) continue;
                if (index >= i) break;

                _data[index] = _data[i];
                _data[i] = null;
            }

            return _data.Select((t, i) => i * (t ?? 0)).Sum();
        }

        public override long Part2()
        {
            var blocks = GetBlocks();
            var dataBlocks = blocks.Where(x => !x.IsGap).OrderByDescending(x => x.Index).ToList();

            foreach(var dataBlock in dataBlocks)
            {
                var gap = blocks.FirstOrDefault(x => x.IsGap && x.Length >= dataBlock.Length);

                if (gap == null) continue;

                MoveBlock(dataBlock, gap);
            }

            return _data.Select((t, i) => i * (t ?? 0)).Sum();
        }

        private void MoveBlock(Block data, Block gap)
        {
            if (data.Start < gap.Start) return;

            for (var i = 0; i < data.Length; i++)
            {
                _data[gap.Start + i] = _data[data.Start + i];
                _data[data.Start + i] = null;
            }

            gap.Start += data.Length;
            gap.Length -= data.Length;
        }

        private List<Block> GetBlocks()
        { 
            var output = new List<Block>();
            long? current = 0;
            var index = 0;
            var start = 0;
            var lastWasGap = false;

            for (var i = 0; i < _data.Count; i++)
            {
                if (lastWasGap)
                {
                    if (_data[i] == null) continue;

                    output.Add(new Block(null, start, i - start, isGap: true));

                    current = _data[i];
                    lastWasGap = false;
                    start = i;
                }
                else
                {
                    if (_data[i] == current) continue;
                    
                    output.Add(new Block(index, start, i - start, isGap: false));

                    current = _data[i];
                    lastWasGap = _data[i] == null;
                    start = i;
                    index++;
                }
            }

            output.Add(new Block(index, start, _data.Count - start, lastWasGap));

            return output;
        }

        private class Block(int? index, int start, int length, bool isGap)
        {
            public int? Index => index;

            public int Start { get; set; } = start;

            public int Length { get; set; } = length;

            public bool IsGap => isGap;
        }
    }
}
