namespace aoc2022;

internal class Day03 : Day
{
    private IEnumerable<string>? sacks;

    internal override void Parse()
    {
        sacks = Util.Parsing.ReadAllLines("03");
    }

    static int GetPriority(char x) => x <= 'Z' ? x - 'A' + 27 : x - 'a' + 1;

    internal override string Part1()
    {
        var compartments = sacks!.Select(x => (x[..(x.Length/2)], x[(x.Length/2)..]));
        var intersected = compartments.Select(x => x.Item1.Intersect(x.Item2).First());
        var total = intersected.Select(GetPriority).Sum();

        return $"Sum of duplicates' priorities: <+white>{total}";
    }

    internal override string Part2()
    {
        var groups = sacks!.Chunk(3);
        var sum = groups.Sum(x =>
            GetPriority(
                x.Skip(1).Aggregate(
                    x.First().AsEnumerable(),
                    (l, e) => l.Intersect(e)
                ).First()
            )
        );

        return $"Sum of badges' priorities: <+white>{sum}";
    }
}
