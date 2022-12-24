using aoc2022.Util;

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

    private static int[][][] grids = Array.Empty<int[][]>();
    internal override void Parse()
    {
        var lines = new List<string>(Parsing.ReadAllLines($"{GetDayNum()}"));
        var maxNumGrids = (int)Util.Math.LCM((ulong) lines.Count - 2, (ulong) lines[0].Length - 2);

        grids = new int[maxNumGrids][][];

        var grid = new int[lines.Count][];
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

        for (int i = 0; i < maxNumGrids; i++)
        {
            if (i == 0)
            {
                grids[i] = grid;
                continue;
            }

            grids[i] = advanceSim(grids[i - 1]);
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

    private static int getMinSteps(ivec2 start, ivec2 dest, int startGridState = 0)
    {
        Queue<(ivec2 pos, int steps)> states = new();
        HashSet<ivec3> visited = new();
        states.Enqueue((start, 0));

        int minSteps = int.MaxValue;

        // render("Start:", p1grid);
        while (states.TryDequeue(out var q))
        {
            if (q.steps > minSteps)
            {
                continue;
            }

            var next = grids[(startGridState + 1 + q.steps) % grids.Length];
            // check if we can wait
            if (next[q.pos.y][q.pos.x] == (int) cellType.open)
            {
                var visitedVec = new ivec3(q.pos.x, q.pos.y, q.steps + 1);
                if (!visited.Contains(visitedVec))
                {
                    states.Enqueue((q.pos, q.steps + 1));
                    visited.Add(visitedVec);
                }
            }

            // queue up all neighbor possibilities
            foreach (var n in q.pos.GetOrthogonalNeighbors())
            {
                if (n == dest)
                {
                    if (q.steps + 1 < minSteps)
                    {
                        minSteps = q.steps + 1;
                    }

                    continue;
                }

                if (n.x < 0 || n.y < 0 || n.x >= next[0].Length || n.y >= next.Length)
                {
                    continue;
                }

                if (next[n.y][n.x] == (int) cellType.open)
                {
                    var visitedVec = new ivec3(n.x, n.y, q.steps + 1);
                    if (!visited.Contains(visitedVec))
                    {
                        states.Enqueue((n, q.steps + 1));
                        visited.Add(visitedVec);
                    }
                }
            }

            // render($"After {i+1} minute{(i + 1 == 1 ? "" : "s")}:", p1grid);
        }

        return minSteps;
    }

    internal override string Part1()
    {
        var start = new ivec2(1, 0);
        var dest = new ivec2(grids[0][0].Length - 2, grids[0].Length - 1);

        var minSteps = getMinSteps(start, dest);

        return $"Minimum steps to reach the end: <+white>{minSteps}";
    }

    internal override string Part2()
    {
        var start = new ivec2(1, 0);
        var dest = new ivec2(grids[0][0].Length - 2, grids[0].Length - 1);

        var toEndOnce = getMinSteps(start, dest);
        var backToStart = getMinSteps(dest, start, toEndOnce);
        var toEndAgain = getMinSteps(start, dest, toEndOnce + backToStart);

        return $"Minimum steps to go there and back again = {toEndOnce} + {backToStart} + {toEndAgain} = <+white>{toEndOnce + backToStart + toEndAgain}";
    }
}
