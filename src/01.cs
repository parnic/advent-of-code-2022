namespace aoc2022;

internal class Day01 : Day
{
    IEnumerable<string>? lines;

    internal override void Parse()
    {
        lines = Util.ReadAllLines("inputs/01.txt");
    }

    internal override string Part1()
    {
        int lastDepth = 0;
        int numIncreased = -1;

        foreach (var line in lines!)
        {
            var depth = Convert.ToInt32(line);
            if (depth > lastDepth)
            {
                numIncreased++;
            }

            lastDepth = depth;
        }

        return $"<+white>{numIncreased}";
    }

    internal override string Part2()
    {
        int lastTotal = 0;
        int numIncreased = -1;
        int num1 = -1;
        int num2 = -1;
        int num3 = -1;

        foreach (var line in lines!)
        {
            var depth = Convert.ToInt32(line);
            num1 = num2;
            num2 = num3;
            num3 = depth;

            if (num1 < 0 || num2 < 0 || num3 < 0)
            {
                continue;
            }

            var total = num1 + num2 + num3;
            if (total > lastTotal)
            {
                numIncreased++;
            }

            lastTotal = total;
        }

        return $"<+white>{numIncreased}";
    }
}
