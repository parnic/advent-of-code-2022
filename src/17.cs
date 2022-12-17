﻿using aoc2022.Util;

namespace aoc2022;

internal class Day17 : Day
{
    private List<bool> jetPatterns = new();

    private ivec2[][] rocks =
    {
        new[]{
            new ivec2(0, 0),
            new ivec2(1, 0),
            new ivec2(2, 0),
            new ivec2(3, 0),
        },
        new[]{
            new ivec2(0, 1),
            new ivec2(1, 0),
            new ivec2(1, 1),
            new ivec2(1, 2),
            new ivec2(2, 1),
        },
        new[]{
            new ivec2(0, 2),
            new ivec2(1, 2),
            new ivec2(2, 0),
            new ivec2(2, 1),
            new ivec2(2, 2),
        },
        new[]{
            new ivec2(0, 0),
            new ivec2(0, 1),
            new ivec2(0, 2),
            new ivec2(0, 3),
        },
        new[]{
            new ivec2(0, 0),
            new ivec2(0, 1),
            new ivec2(1, 0),
            new ivec2(1, 1),
        },
    };

    internal override void Parse()
    {
        var content = Parsing.ReadAllText("17");
        foreach (var ch in content)
        {
            jetPatterns.Add(ch == '>');
        }
    }

    private const int maxWidth = 7;
    private bool canMove(int rockIdx, ivec2 rockPos, ICollection<ivec2> grid, ivec2 direction)
    {
        var rock = rocks[rockIdx];
        var rockWidth = rock.MaxBy(v => v.x).x + 1;
        var rockHeight = rock.MaxBy(v => v.y).y;
        var result = rockPos + direction;
        if (result.x < 0)
        {
            return false;
        }
        if (result.x + rockWidth > maxWidth)
        {
            return false;
        }

        if (result.y - rockHeight == -1)
        {
            return false;
        }

        foreach (var pt in rock)
        {
            if (grid.Contains(result + new ivec2(pt.x, -pt.y)))
            {
                return false;
            }
        }

        return true;
    }

    internal override string Part1()
    {
        int settledRocks = 0;
        HashSet<ivec2> grid = new();
        int jetSteps = 0;
        for (int idx = 0; settledRocks < 2022; idx++)
        {
            var rockIdx = idx % rocks.Length;
            var maxHeight = grid.Count > 0 ? grid.MaxBy(v => v.y).y + 1 : 0;
            var rockHeight = rocks[rockIdx].MaxBy(v => v.y).y;
            var rockPos = new ivec2(2, maxHeight + rockHeight + 3);
            bool settled = false;
            while (!settled)
            {
                var jetIdx = jetSteps % jetPatterns.Count;
                jetSteps++;
                if (jetPatterns[jetIdx])
                {
                    if (canMove(rockIdx, rockPos, grid, ivec2.RIGHT))
                    {
                        rockPos.x++;
                    }
                }
                else
                {
                    if (canMove(rockIdx, rockPos, grid, ivec2.LEFT))
                    {
                        rockPos.x--;
                    }
                }

                // inverted up/down for this...
                if (canMove(rockIdx, rockPos, grid, ivec2.UP))
                {
                    rockPos.y--;
                }
                else
                {
                    settled = true;
                }

                if (settled)
                {
                    foreach (var pt in rocks[rockIdx])
                    {
                        grid.Add(rockPos + new ivec2(pt.x, -pt.y));
                    }

                    settledRocks++;
                }
            }
        }

        var finalMaxHeight = grid.MaxBy(v => v.y).y + 1;
        return $"After <green>2022<+black> rocks, tower height is: <+white>{finalMaxHeight}";
    }

    internal override string Part2()
    {
        long settledRocks = 0;
        HashSet<ivec2> grid = new();
        List<long> heightDeltasPerStep = new();
        long jetSteps = 0;
        for (long idx = 0; settledRocks < 10000; idx++)
        {
            var rockIdx = (int)(idx % rocks.Length);
            var maxHeight = grid.Count > 0 ? grid.MaxBy(v => v.y).y + 1 : 0;
            var rockHeight = rocks[rockIdx].MaxBy(v => v.y).y;
            var rockPos = new ivec2(2, maxHeight + rockHeight + 3);
            bool settled = false;
            while (!settled)
            {
                var jetIdx = (int)(jetSteps % jetPatterns.Count);
                jetSteps++;
                if (jetPatterns[jetIdx])
                {
                    if (canMove(rockIdx, rockPos, grid, ivec2.RIGHT))
                    {
                        rockPos.x++;
                    }
                }
                else
                {
                    if (canMove(rockIdx, rockPos, grid, ivec2.LEFT))
                    {
                        rockPos.x--;
                    }
                }

                // inverted up/down for this...
                if (canMove(rockIdx, rockPos, grid, ivec2.UP))
                {
                    rockPos.y--;
                }
                else
                {
                    settled = true;
                }

                if (settled)
                {
                    foreach (var pt in rocks[rockIdx])
                    {
                        grid.Add(rockPos + new ivec2(pt.x, -pt.y));
                    }

                    settledRocks++;

                    var newMaxHeight = grid.MaxBy(v => v.y).y + 1;
                    heightDeltasPerStep.Add(newMaxHeight - maxHeight);
                }
            }
        }

        int cycleSize = 0;
        int skip = 0;
        for (int jumpAhead = 250; jumpAhead < 500 && skip == 0; jumpAhead++)
        {
            var skipped = heightDeltasPerStep.Skip(jumpAhead);
            for (int cycleLen = 100; cycleLen < heightDeltasPerStep.Count / 4; cycleLen++)
            {
                var chunked = skipped.Chunk(cycleLen);
                if (chunked.All(c => c.Length < cycleLen || c.SequenceEqual(chunked.First())))
                {
                    skip = jumpAhead;
                    cycleSize = cycleLen;
                    break;
                }
            }
        }

        if (skip == 0)
        {
            throw new Exception("cycle not found");
        }

        var baseHeight = heightDeltasPerStep.Take(skip).Sum();
        var chunkHeight = heightDeltasPerStep.Skip(skip).Take(cycleSize).Sum();
        var cumulativeHeight = baseHeight;
        long simmed;
        for (simmed = skip; simmed + cycleSize < 1000000000000; simmed += cycleSize)
        {
            cumulativeHeight += chunkHeight;
        }

        var finalChunkHeight = heightDeltasPerStep.Skip(skip).Take((int)(1000000000000 - simmed)).Sum();
        cumulativeHeight += finalChunkHeight;

        return $"After skipping <green>{skip}<+black> initial drops, a cycle of size <green>{cycleSize}<+black> produced a final height of <+white>{cumulativeHeight}";
    }
}
