﻿using aoc2022.Util;

namespace aoc2022;

internal class Day24 : Day
{
    private enum cellType
    {
        open = 0,
        wall = 1 << 0,
        blizUp = 1 << 1,
        blizRight = 1 << 2,
        blizDown = 1 << 3,
        blizLeft = 1 << 4,
    }

    private int[][]? grid;
    internal override void Parse()
    {
        var lines = new List<string>(Util.Parsing.ReadAllLines($"{GetDayNum()}"));
        grid = new int[lines.Count][];
        for (int row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            grid[row] = new int[line.Length];
            for (int col = 0; col < line.Length; col++)
            {
                grid[row][col] = line[col] switch
                {
                    '#' => (int)cellType.wall,
                    '.' => (int)cellType.open,
                    '^' => (int)cellType.blizUp,
                    '>' => (int)cellType.blizRight,
                    'v' => (int)cellType.blizDown,
                    '<' => (int)cellType.blizLeft,
                    _ => throw new Exception(),
                };
            }
        }
    }

    private static int[][] deepCopyGrid(int[][] src)
    {
        var copy = (int[][])src.Clone();
        for (int i = 0; i < src.Length; i++)
        {
            copy[i] = (int[])src[i].Clone();
        }

        return copy;
    }

    private static void render(string label, int[][] grid)
    {
        Console.WriteLine(label);

        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == (int) cellType.wall)
                {
                    Console.Write('#');
                }
                else if (grid[row][col] == (int) cellType.open)
                {
                    Console.Write('.');
                }
                else if (grid[row][col] == (int) cellType.blizRight)
                {
                    Console.Write('>');
                }
                else if (grid[row][col] == (int) cellType.blizLeft)
                {
                    Console.Write('<');
                }
                else if (grid[row][col] == (int) cellType.blizUp)
                {
                    Console.Write('^');
                }
                else if (grid[row][col] == (int) cellType.blizDown)
                {
                    Console.Write('v');
                }
                else
                {
                    int numSet = 0;
                    var enumVals = Enum.GetValues<cellType>();
                    for (int i = 2; i < enumVals.Length; i++)
                    {
                        if ((grid[row][col] & (int)enumVals[i]) != 0)
                        {
                            numSet++;
                        }
                    }
                    Console.Write($"{numSet}");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private static int[][] advanceSim(int[][] grid)
    {
        var next = deepCopyGrid(grid);
        for (int row = 1; row < grid.Length - 1; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == (int)cellType.wall)
                {
                    continue;
                }

                if (grid[row][col] == (int)cellType.open)
                {
                    continue;
                }

                var isRight = (grid[row][col] & (int) cellType.blizRight) != 0;
                var isLeft = (grid[row][col] & (int) cellType.blizLeft) != 0;
                var isUp = (grid[row][col] & (int) cellType.blizUp) != 0;
                var isDown = (grid[row][col] & (int) cellType.blizDown) != 0;
                if (isRight)
                {
                    var nextCol = col + 1;
                    if (grid[row][nextCol] == (int) cellType.wall)
                    {
                        nextCol = 1;
                    }

                    next[row][col] -= (int) cellType.blizRight;
                    next[row][nextCol] += (int) cellType.blizRight;
                }
                if (isLeft)
                {
                    var nextCol = col - 1;
                    if (grid[row][nextCol] == (int) cellType.wall)
                    {
                        nextCol = grid[row].Length - 2;
                    }

                    next[row][col] -= (int) cellType.blizLeft;
                    next[row][nextCol] += (int) cellType.blizLeft;
                }
                if (isUp)
                {
                    var nextRow = row - 1;
                    if (grid[nextRow][col] == (int) cellType.wall)
                    {
                        nextRow = grid.Length - 2;
                    }

                    next[row][col] -= (int) cellType.blizUp;
                    next[nextRow][col] += (int) cellType.blizUp;
                }
                if (isDown)
                {
                    var nextRow = row + 1;
                    if (grid[nextRow][col] == (int) cellType.wall)
                    {
                        nextRow = 1;
                    }

                    next[row][col] -= (int) cellType.blizDown;
                    next[nextRow][col] += (int) cellType.blizDown;
                }
            }
        }

        return next;
    }

    private static (int steps, int[][] gridState) getMinSteps(int[][] grid, ivec2 start, ivec2 dest)
    {
        Queue<(ivec2 pos, int[][] gridState, int steps)> states = new();
        states.Enqueue((start, deepCopyGrid(grid), 0));

        int? minSteps = null;
        int[][]? minGridState = null;

        // render("Start:", p1grid);
        while (states.TryDequeue(out var q))
        {
            if (minSteps != null && q.steps > minSteps)
            {
                continue;
            }

            var next = advanceSim(q.gridState);
            // check if we can wait
            if (next[q.pos.y][q.pos.x] == (int) cellType.open)
            {
                var nextState = (pos: q.pos, next: next, steps: q.steps + 1);
                if (!states.Any(s => s.pos == nextState.pos && s.steps == nextState.steps))
                {
                    states.Enqueue(nextState);
                }
            }

            // queue up all neighbor possibilities
            foreach (var n in q.pos.GetOrthogonalNeighbors())
            {
                if (n == dest)
                {
                    if (minSteps == null || q.steps + 1 < minSteps)
                    {
                        minSteps = q.steps + 1;
                        minGridState = next;
                    }

                    continue;
                }

                if (n.x < 0 || n.y < 0 || n.x >= next[0].Length || n.y >= next.Length)
                {
                    continue;
                }

                if (next[n.y][n.x] == (int) cellType.open)
                {
                    var nextState = (pos: n, next: next, steps: q.steps + 1);
                    if (!states.Any(s => s.pos == nextState.pos && s.steps == nextState.steps))
                    {
                        states.Enqueue(nextState);
                    }
                }
            }

            // render($"After {i+1} minute{(i + 1 == 1 ? "" : "s")}:", p1grid);
        }

        return (minSteps!.Value, minGridState!);
    }

    internal override string Part1()
    {
        var start = new ivec2(1, 0);
        var dest = new ivec2(grid![0].Length - 2, grid!.Length - 1);

        var (minSteps, _) = getMinSteps(grid!, start, dest);

        return $"Minimum steps to reach the end: <+white>{minSteps}";
    }

    internal override string Part2()
    {
        var start = new ivec2(1, 0);
        var dest = new ivec2(grid![0].Length - 2, grid!.Length - 1);

        var toEndOnce = getMinSteps(grid!, start, dest);
        var backToStart = getMinSteps(toEndOnce.gridState, dest, start);
        var toEndAgain = getMinSteps(backToStart.gridState, start, dest);

        return $"Minimum steps to go there and back again: <+white>{toEndOnce.steps + backToStart.steps + toEndAgain.steps}";
    }
}
