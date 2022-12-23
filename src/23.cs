using System.Collections.Immutable;
using aoc2022.Util;

namespace aoc2022;

internal class Day23 : Day
{
    private readonly HashSet<ivec2> elves = new();

    private static readonly ivec2[][] dirOrder =
    {
        new[]{ivec2.UP, ivec2.UP + ivec2.RIGHT, ivec2.UP + ivec2.LEFT},
        new[]{ivec2.DOWN, ivec2.DOWN + ivec2.RIGHT, ivec2.DOWN + ivec2.LEFT},
        new[]{ivec2.LEFT, ivec2.UP + ivec2.LEFT, ivec2.DOWN + ivec2.LEFT},
        new[]{ivec2.RIGHT, ivec2.UP + ivec2.RIGHT, ivec2.DOWN + ivec2.RIGHT},
    };

    internal override void Parse()
    {
        var lines = Parsing.ReadAllLines("23").ToImmutableList();
        for (int row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            for (int col = 0; col < line.Length; col++)
            {
                if (line[col] == '#')
                {
                    elves.Add(new ivec2(col, row));
                }
            }
        }
    }

    private static List<(ivec2 curr, ivec2 proposed)> ConsiderMove(ICollection<ivec2> positions, int orderStart)
    {
        List<(ivec2 curr, ivec2 proposed)> considerations = new(positions.Count);

        foreach (var elfPos in positions)
        {
            if (!elfPos.GetNeighbors().Any(positions.Contains))
            {
                continue;
            }

            for (int i = 0; i < dirOrder.Length; i++)
            {
                var dirs = dirOrder[(i + orderStart) % dirOrder.Length];
                if (!dirs.Any(d => positions.Contains(d + elfPos)))
                {
                    considerations.Add((elfPos, elfPos + dirs[0]));
                    break;
                }
            }
        }

        return considerations;
    }

    private static int PlayOut(ICollection<ivec2> positions, int? maxRounds = null)
    {
        int round;
        for (round = 0; (maxRounds == null || round < maxRounds); round++)
        {
            var considerations = ConsiderMove(positions, round);
            if (!considerations.Any())
            {
                break;
            }

            for (int i = 0; i < considerations.Count; i++)
            {
                bool bDuped = false;
                for (int j = i + 1; j < considerations.Count; j++)
                {
                    if (considerations[i].proposed == considerations[j].proposed)
                    {
                        bDuped = true;
                        considerations.RemoveAt(j);
                        j--;
                    }
                }

                if (bDuped)
                {
                    considerations.RemoveAt(i);
                    i--;
                }
            }

            foreach (var move in considerations)
            {
                positions.Remove(move.curr);
                positions.Add(move.proposed);
            }
        }

        return round + 1;
    }

    private static long GetEmptySpots(ICollection<ivec2> positions)
    {
        var minY = positions.MinBy(p => p.y).y;
        var minX = positions.MinBy(p => p.x).x;
        var maxY = positions.MaxBy(p => p.y).y;
        var maxX = positions.MaxBy(p => p.x).x;

        var height = (maxY - minY) + 1;
        var width = (maxX - minX) + 1;
        var area = height * width;
        var emptyGround = area - positions.Count;

        return emptyGround;
    }

    internal override string Part1()
    {
        HashSet<ivec2> positions = new(elves);
        PlayOut(positions, 10);
        var emptyGround = GetEmptySpots(positions);
        return $"Empty ground spaces after 10 rounds: <+white>{emptyGround}";
    }

    internal override string Part2()
    {
        HashSet<ivec2> positions = new(elves);
        var round = PlayOut(positions);
        return $"Rounds to settle: <+white>{round}";
    }
}
