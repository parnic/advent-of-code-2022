namespace aoc2022;

internal class Day04 : Day
{
    private readonly List<((int min, int max) one,(int min, int max) two)> assignmentPairs = new();

    internal override void Parse()
    {
        foreach (var line in Util.Parsing.ReadAllLines("04"))
        {
            var assignments = line.Split(',');
            var firstAssignment = assignments[0].Split('-');
            var secondAssignment = assignments[1].Split('-');
            var firstRange = (int.Parse(firstAssignment[0]), int.Parse(firstAssignment[1]));
            var secondRange = (int.Parse(secondAssignment[0]), int.Parse(secondAssignment[1]));
            assignmentPairs.Add((firstRange, secondRange));
        }
    }

    internal override string Part1()
    {
        var overlaps = assignmentPairs.Count(x =>
            (x.one.min >= x.two.min && x.one.max <= x.two.max) ||
            (x.two.min >= x.one.min && x.two.max <= x.one.max));

        return $"# complete overlaps: <+white>{overlaps}";
    }

    internal override string Part2()
    {
        var overlaps = assignmentPairs.Count(x =>
            (x.one.min >= x.two.min && x.one.min <= x.two.max) ||
            (x.one.max >= x.two.min && x.one.max <= x.two.max) ||
            (x.two.min >= x.one.min && x.two.min <= x.one.max) ||
            (x.two.max >= x.one.min && x.two.max <= x.one.max));

        return $"# partial overlaps: <+white>{overlaps}";
    }
}
