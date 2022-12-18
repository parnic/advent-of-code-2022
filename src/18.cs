using aoc2022.Util;

namespace aoc2022;

internal class Day18 : Day
{
    private readonly List<ivec3> cubes = new();
    internal override void Parse()
    {
        cubes.AddRange(Parsing.ReadAllLines("18").Select(ivec3.Parse));
    }

    internal override string Part1()
    {
        var totalSides = 6 * cubes.Count;
        var coveredSides = cubes.Sum(c => cubes.Count(c2 => c != c2 && c.IsTouching(c2)));

        return $"Exposed faces: <+white>{totalSides - coveredSides}";
    }

    internal override string Part2()
    {
        var set = new HashSet<ivec3>(cubes);
        var minv = cubes.Min(c => c.MinElement) - 1;
        var maxv = cubes.Max(c => c.MaxElement) + 1;
        var min = new ivec3(minv, minv, minv);
        var max = new ivec3(maxv, maxv, maxv);

        HashSet<ivec3> visited = new();
        Queue<ivec3> q = new();
        var total = 0;
        q.Enqueue(min);
        visited.Add(min);
        while (q.Count > 0)
        {
            var v = q.Dequeue();
            foreach (var neighbor in v.GetNeighbors(min, max))
            {
                if (visited.Contains(neighbor))
                {
                    continue;
                }

                if (set.Contains(neighbor))
                {
                    total++;
                    continue;
                }

                visited.Add(neighbor);
                q.Enqueue(neighbor);
            }
        }

        return $"Exterior exposed faces: <+white>{total}";
    }
}
