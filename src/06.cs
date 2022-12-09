namespace aoc2022;

internal class Day06 : Day
{
    private string? buffer;

    internal override void Parse()
    {
        buffer = Util.Parsing.ReadAllText("06");
    }

    private static int FindDistinct(string buf, int distinctLen)
    {
        Queue<char> s = new(distinctLen);
        for (int i = 0; i < buf.Length; i++)
        {
            var c = buf[i];
            s.Enqueue(c);
            if (s.Count < distinctLen)
            {
                continue;
            }

            if (s.Distinct().Count() == distinctLen)
            {
                return i + 1;
            }

            s.Dequeue();
        }

        throw new Exception("didn't find anything");
    }

    internal override string Part1()
    {
        return $"First span of 4 distinct characters at character <+white>{FindDistinct(buffer!, 4)}";
    }

    internal override string Part2()
    {
        return $"First span of 14 distinct characters at character <+white>{FindDistinct(buffer!, 14)}";
    }
}
