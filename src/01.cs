namespace aoc2022;

internal class Day01 : Day
{
    List<List<int>> lines = new();

    internal override void Parse()
    {
        List<int> calories = new();
        foreach (var line in Util.Parsing.ReadAllLines("01"))
        {
            if (string.IsNullOrEmpty(line))
            {
                lines.Add(new(calories));
                calories.Clear();
                continue;
            }

            calories.Add(int.Parse(line));
        }
        lines.Add(new(calories));
    }

    internal override string Part1()
    {
        return $"Most calories: <+white>{lines.Max(x => x.Sum())}";
    }

    internal override string Part2()
    {
        return $"Sum of top 3: <+white>{lines.OrderByDescending(x => x.Sum()).Take(3).Sum(x => x.Sum())}";
    }
}
