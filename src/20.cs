namespace aoc2022;

internal class Day20 : Day
{
    private List<(int num, int origIdx)> nums = new();
    internal override void Parse()
    {
        foreach (var line in Util.Parsing.ReadAllLines("20"))
        {
            nums.Add((int.Parse(line), nums.Count));
        }
    }

    private static void mix(List<(int num, int origIdx)> mixedNums, IList<(int num, int origIdx)> origNums)
    {
        for (int i = 0; i < origNums.Count; i++)
        {
            if (origNums[i].num == 0)
            {
                continue;
            }

            var idx = mixedNums.FindIndex(n => n.origIdx == origNums[i].origIdx);
            mixedNums.RemoveAt(idx);
            var newIdx = Util.Math.Modulo(idx + origNums[i].num, mixedNums.Count);
            mixedNums.Insert((int)newIdx, origNums[i]);
        }
    }

    private static void mix(List<(long num, int origIdx)> mixedNums, IList<(long num, int origIdx)> origNums)
    {
        for (int i = 0; i < origNums.Count; i++)
        {
            if (origNums[i].num == 0)
            {
                continue;
            }

            var idx = mixedNums.FindIndex(n => n.origIdx == origNums[i].origIdx);
            mixedNums.RemoveAt(idx);
            var newIdx = Util.Math.Modulo(idx + origNums[i].num, mixedNums.Count);
            mixedNums.Insert((int)newIdx, origNums[i]);
        }
    }

    internal override string Part1()
    {
        var mixedNums = new List<(int num, int origIdx)>(nums);
        mix(mixedNums, nums);

        var zeroIdx = mixedNums.FindIndex(n => n.num == 0);
        var coord1 = mixedNums[(zeroIdx + 1000) % mixedNums.Count].num;
        var coord2 = mixedNums[(zeroIdx + 2000) % mixedNums.Count].num;
        var coord3 = mixedNums[(zeroIdx + 3000) % mixedNums.Count].num;

        return $"Grove coordinate sum: <+white>{coord1 + coord2 + coord3}";
    }

    internal override string Part2()
    {
        var keyedNums = new List<(long num, int origIdx)>(nums.Select(c => (c.num * 811589153L, c.origIdx)));
        var mixedKeyedNums = new List<(long num, int origIdx)>(keyedNums);
        for (int i = 0; i < 10; i++)
        {
            mix(mixedKeyedNums, keyedNums);
        }

        var zeroIdx = mixedKeyedNums.FindIndex(n => n.num == 0);
        var coord1 = mixedKeyedNums[(zeroIdx + 1000) % mixedKeyedNums.Count].num;
        var coord2 = mixedKeyedNums[(zeroIdx + 2000) % mixedKeyedNums.Count].num;
        var coord3 = mixedKeyedNums[(zeroIdx + 3000) % mixedKeyedNums.Count].num;

        return $"Fully decrypted grove coordinate sum: <+white>{coord1 + coord2 + coord3}";
    }
}
