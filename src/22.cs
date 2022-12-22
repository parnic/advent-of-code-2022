using System.Text;
using aoc2022.Util;

namespace aoc2022;

internal static class Day22Extensions
{
    public static int firstIndexOfTile(this Day22.tileType[] arr, Day22.tileType t)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == t)
            {
                return i;
            }
        }

        return -1;
    }

    public static int getLastNonEmptyTile(this Day22.tileType[] arr)
    {
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            if (arr[i] != Day22.tileType.empty)
            {
                return i;
            }
        }

        throw new Exception("no non-empty tiles in list");
    }

    public static int getFirstNonEmptyTile(this Day22.tileType[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != Day22.tileType.empty)
            {
                return i;
            }
        }

        throw new Exception("no non-empty tiles in list");
    }

    public static int getLastNonEmptyTile(this Day22.tileType[][] arr, int col)
    {
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            if (arr[i][col] != Day22.tileType.empty)
            {
                return i;
            }
        }

        throw new Exception("no non-empty tiles in list");
    }

    public static int getFirstNonEmptyTile(this Day22.tileType[][] arr, int col)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i][col] != Day22.tileType.empty)
            {
                return i;
            }
        }

        throw new Exception("no non-empty tiles in list");
    }

    // for sample data
    // private static readonly Dictionary<(int region, ivec2 facing), (int region, ivec2 facing)> cubeMap = new()
    // {
    //     {(1, ivec2.LEFT), (3, ivec2.DOWN)},
    //     {(1, ivec2.UP), (2, ivec2.DOWN)},
    //     {(1, ivec2.RIGHT), (6, ivec2.LEFT)},
    //     {(2, ivec2.LEFT), (6, ivec2.UP)},
    //     {(2, ivec2.UP), (1, ivec2.DOWN)},
    //     {(2, ivec2.DOWN), (5, ivec2.UP)},
    //     {(3, ivec2.UP), (1, ivec2.RIGHT)},
    //     {(3, ivec2.DOWN), (5, ivec2.RIGHT)},
    //     {(4, ivec2.RIGHT), (6, ivec2.DOWN)},
    //     {(5, ivec2.LEFT), (3, ivec2.UP)},
    //     {(5, ivec2.DOWN), (2, ivec2.UP)},
    //     {(6, ivec2.UP), (4, ivec2.LEFT)},
    //     {(6, ivec2.RIGHT), (1, ivec2.LEFT)},
    //     {(6, ivec2.DOWN), (2, ivec2.RIGHT)},
    // };
    // for my data
    private static readonly Dictionary<(int region, ivec2 facing), (int region, ivec2 facing)> cubeMap = new()
    {
        {(1, ivec2.LEFT), (4, ivec2.RIGHT)},
        {(1, ivec2.UP), (6, ivec2.RIGHT)},
        {(2, ivec2.DOWN), (3, ivec2.LEFT)},
        {(2, ivec2.RIGHT), (5, ivec2.LEFT)},
        {(2, ivec2.UP), (6, ivec2.UP)},
        {(3, ivec2.RIGHT), (2, ivec2.UP)},
        {(3, ivec2.LEFT), (4, ivec2.DOWN)},
        {(4, ivec2.LEFT), (1, ivec2.RIGHT)},
        {(4, ivec2.UP), (3, ivec2.RIGHT)},
        {(5, ivec2.RIGHT), (2, ivec2.LEFT)},
        {(5, ivec2.DOWN), (6, ivec2.LEFT)},
        {(6, ivec2.LEFT), (1, ivec2.DOWN)},
        {(6, ivec2.DOWN), (2, ivec2.DOWN)},
        {(6, ivec2.RIGHT), (5, ivec2.UP)},
    };

    private static ivec2 translatePoint(ivec2 currPos, int prevRegion, int region, int widthPerRegion)
    {
        long row = 0;
        long col = 0;

        // for sample data
        // if (prevRegion == 1)
        // {
        //     if (region == 3)
        //     {
        //         col = currPos.y - widthPerRegion;
        //         row = widthPerRegion;
        //     }
        //     else if (region == 2)
        //     {
        //         col = widthPerRegion - (currPos.x - (widthPerRegion * 2)) - 1;
        //         row = widthPerRegion;
        //     }
        //     else if (region == 6)
        //     {
        //         row = (widthPerRegion - currPos.y) + (widthPerRegion * 2) - 1;
        //         col = currPos.x + widthPerRegion;
        //     }
        // }
        // else if (prevRegion == 2)
        // {
        //     if (region == 1)
        //     {
        //         col = (widthPerRegion - 1 - currPos.x) + (widthPerRegion * 2);
        //         row = 0;
        //     }
        //     else if (region == 5)
        //     {
        //         col = (widthPerRegion - 1 - currPos.x) + (widthPerRegion * 2);
        //         row = (widthPerRegion * 3) - 1;
        //     }
        //     else if (region == 6)
        //     {
        //         col = ((widthPerRegion * 2) - 1 - currPos.y) + (widthPerRegion * 3);
        //         row = (widthPerRegion * 3) - 1;
        //     }
        // }
        // else if (prevRegion == 3)
        // {
        //     if (region == 1)
        //     {
        //         row = currPos.x - widthPerRegion;
        //         col = widthPerRegion * 2;
        //     }
        //     else if (region == 5)
        //     {
        //         col = widthPerRegion * 3;
        //         row = (widthPerRegion * 2) - currPos.x + (widthPerRegion * 3) - 1;
        //     }
        // }
        // else if (prevRegion == 4)
        // {
        //     if (region == 6)
        //     {
        //         row = widthPerRegion * 2;
        //         col = (widthPerRegion * 2) - currPos.y + (widthPerRegion * 3) - 1;
        //     }
        // }
        // else if (prevRegion == 5)
        // {
        //     if (region == 3)
        //     {
        //         row = (widthPerRegion * 2) - 1;
        //         col = ((widthPerRegion * 3) - currPos.y) + widthPerRegion;
        //     }
        //     else if (region == 2)
        //     {
        //         row = (widthPerRegion * 2) - 1;
        //         col = (widthPerRegion * 3) - currPos.x - 1;
        //     }
        // }
        // else if (prevRegion == 6)
        // {
        //     if (region == 1)
        //     {
        //         col = (widthPerRegion * 3) - 1;
        //         row = (widthPerRegion * 3) - currPos.y;
        //     }
        //     else if (region == 2)
        //     {
        //         col = 0;
        //         row = ((widthPerRegion * 4) - currPos.x) + widthPerRegion;
        //     }
        //     else if (region == 4)
        //     {
        //         col = (widthPerRegion * 3) - 1;
        //         row = ((widthPerRegion * 4) - currPos.x) + widthPerRegion;
        //     }
        // }
        // for my input
        if (currPos.x >= widthPerRegion || currPos.y >= widthPerRegion)
        {
            currPos = new ivec2(currPos.x % widthPerRegion, currPos.y % widthPerRegion);
        }

        if (prevRegion == 1)
        {
            if (region == 4)
            {
                row = widthPerRegion - 1 - currPos.y;
                col = 0;
            }
            else if (region == 6)
            {
                row = currPos.x;
                col = 0;
            }
        }
        else if (prevRegion == 2)
        {
            if (region == 3)
            {
                row = currPos.x;
                col = widthPerRegion - 1;
            }
            else if (region == 5)
            {
                row = widthPerRegion - 1 - currPos.y;
                col = widthPerRegion - 1;
            }
            else if (region == 6)
            {
                col = currPos.x;
                row = widthPerRegion - 1;
            }
        }
        else if (prevRegion == 3)
        {
            if (region == 2)
            {
                col = currPos.y;
                row = widthPerRegion - 1;
            }
            else if (region == 4)
            {
                col = currPos.y;
                row = 0;
            }
        }
        else if (prevRegion == 4)
        {
            if (region == 1)
            {
                col = 0;
                row = widthPerRegion - 1 - currPos.y;
            }
            else if (region == 3)
            {
                col = 0;
                row = currPos.x;
            }
        }
        else if (prevRegion == 5)
        {
            if (region == 2)
            {
                row = widthPerRegion - 1 - currPos.y;
                col = widthPerRegion - 1;
            }
            else if (region == 6)
            {
                row = currPos.x;
                col = widthPerRegion - 1;
            }
        }
        else if (prevRegion == 6)
        {
            if (region == 1)
            {
                col = currPos.y;
                row = 0;
            }
            else if (region == 2)
            {
                col = currPos.x;
                row = 0;
            }
            else if (region == 5)
            {
                col = currPos.y;
                row = widthPerRegion - 1;
            }
        }

        if (region == 1)
        {
            col += widthPerRegion;
        }
        else if (region == 2)
        {
            col += widthPerRegion * 2;
        }
        else if (region == 3)
        {
            col += widthPerRegion;
            row += widthPerRegion;
        }
        else if (region == 4)
        {
            row += widthPerRegion * 2;
        }
        else if (region == 5)
        {
            row += widthPerRegion * 2;
            col += widthPerRegion;
        }
        else if (region == 6)
        {
            row += widthPerRegion * 3;
        }

        return new ivec2(col, row);
    }

    public static (ivec2 pos, ivec2 facing) getCubeTransition(this Day22.tileType[][] arr, ivec2 curr, ivec2 facing)
    {
        int widthPerRegion = arr.Length < arr[0].Length ? arr.Length / 3 : arr[0].Length / 3;
        int region;
        // for sample data
        // if (curr.y < widthPerRegion)
        // {
        //     region = 1;
        // }
        // else if (curr.y < widthPerRegion * 2)
        // {
        //     if (curr.x < widthPerRegion)
        //     {
        //         region = 2;
        //     }
        //     else if (curr.x < widthPerRegion * 2)
        //     {
        //         region = 3;
        //     }
        //     else
        //     {
        //         region = 4;
        //     }
        // }
        // else
        // {
        //     if (curr.x < widthPerRegion * 3)
        //     {
        //         region = 5;
        //     }
        //     else
        //     {
        //         region = 6;
        //     }
        // }
        // for my data
        if (curr.y < widthPerRegion)
        {
            if (curr.x < widthPerRegion * 2)
            {
                region = 1;
            }
            else
            {
                region = 2;
            }
        }
        else if (curr.y < widthPerRegion * 2)
        {
            region = 3;
        }
        else if (curr.y < widthPerRegion * 3)
        {
            if (curr.x < widthPerRegion)
            {
                region = 4;
            }
            else
            {
                region = 5;
            }
        }
        else
        {
            region = 6;
        }

        var (newRegion, newFacing) = cubeMap[(region, facing)];
        var newPos = translatePoint(curr, region, newRegion, widthPerRegion);

        return (newPos, newFacing);
    }
}

internal class Day22 : Day
{
    internal enum tileType
    {
        empty,
        open,
        wall,
    }

    private record instruction(int fwd, char turn);

    private readonly List<instruction> instructions = new();

    private tileType[][] grid = Array.Empty<tileType[]>();

    internal override void Parse()
    {
        int phase = 0;
        var lines = new List<string>(Parsing.ReadAllLines("22"));
        var longestRow = lines.Take(lines.Count - 2).Select(line => line.Length).Max();
        grid = new tileType[lines.Count - 2][];
        for (int row = 0; row < lines.Count; row++)
        {
            var line = lines[row];

            if (phase == 1)
            {
                StringBuilder sb = new();
                foreach (var ch in line)
                {
                    if (ch is >= '0' and <= '9')
                    {
                        sb.Append(ch);
                    }
                    else
                    {
                        instructions.Add(new instruction(int.Parse(sb.ToString()), 'a'));
                        sb.Clear();
                        instructions.Add(new instruction(0, ch));
                    }
                }

                if (sb.Length > 0)
                {
                    instructions.Add(new instruction(int.Parse(sb.ToString()), 'a'));
                }

                break;
            }

            if (line.Length == 0)
            {
                phase++;
                continue;
            }

            grid[row] = new tileType[longestRow];
            for (int col = 0; col < line.Length; col++)
            {
                var ch = line[col];
                grid[row][col] = ch switch
                {
                    ' ' => tileType.empty,
                    '.' => tileType.open,
                    '#' => tileType.wall,
                    _ => throw new Exception($"unexpected char {ch}"),
                };
            }
        }
    }

    private static long getPassword(ivec2 pos, ivec2 facing)
    {
        var rowVal = pos.y + 1;
        var colVal = pos.x + 1;
        var facingVal = 0;
        if (facing == ivec2.DOWN)
        {
            facingVal = 1;
        }
        else if (facing == ivec2.LEFT)
        {
            facingVal = 2;
        }
        else if (facing == ivec2.UP)
        {
            facingVal = 3;
        }

        return (1000 * rowVal) + (4 * colVal) + facingVal;
    }

    internal override string Part1()
    {
        var start = new ivec2(grid[0].firstIndexOfTile(tileType.open), 0);

        var curr = start;
        var facing = ivec2.RIGHT;
        foreach (var inst in instructions)
        {
            if (inst.fwd > 0)
            {
                for (int i = 0; i < inst.fwd; i++)
                {
                    var next = curr + facing;
                    if (facing == ivec2.LEFT && (next.x < 0 || grid[next.y][next.x] == tileType.empty))
                    {
                        var col = grid[curr.y].getLastNonEmptyTile();
                        next = new ivec2(col, curr.y);
                    }
                    else if (facing == ivec2.RIGHT && (next.x >= grid[next.y].Length || grid[next.y][next.x] == tileType.empty))
                    {
                        var col = grid[curr.y].getFirstNonEmptyTile();
                        next = new ivec2(col, curr.y);
                    }
                    else if (facing == ivec2.DOWN && (next.y >= grid.Length || grid[next.y][next.x] == tileType.empty))
                    {
                        var row = grid.getFirstNonEmptyTile((int) next.x);
                        next = new ivec2(curr.x, row);
                    }
                    else if (facing == ivec2.UP && (next.y < 0 || grid[next.y][next.x] == tileType.empty))
                    {
                        var row = grid.getLastNonEmptyTile((int) next.x);
                        next = new ivec2(curr.x, row);
                    }

                    if (grid[next.y][next.x] == tileType.open)
                    {
                        curr = next;
                    }
                }
            }
            else
            {
                if (inst.turn == 'R')
                {
                    facing.RotateRight();
                }
                else if (inst.turn == 'L')
                {
                    facing.RotateLeft();
                }
            }
        }

        return $"Final password: <+white>{getPassword(curr, facing)}";
    }

    internal override string Part2()
    {
        var start = new ivec2(grid[0].firstIndexOfTile(tileType.open), 0);

        var curr = start;
        var facing = ivec2.RIGHT;
        foreach (var inst in instructions)
        {
            if (inst.fwd > 0)
            {
                for (int i = 0; i < inst.fwd; i++)
                {
                    var next = curr + facing;
                    var newFacing = facing;
                    if (facing == ivec2.LEFT && (next.x < 0 || grid[next.y][next.x] == tileType.empty))
                    {
                        (next, newFacing) = grid.getCubeTransition(curr, facing);
                    }
                    else if (facing == ivec2.RIGHT &&
                             (next.x >= grid[next.y].Length || grid[next.y][next.x] == tileType.empty))
                    {
                        (next, newFacing) = grid.getCubeTransition(curr, facing);
                    }
                    else if (facing == ivec2.DOWN && (next.y >= grid.Length || grid[next.y][next.x] == tileType.empty))
                    {
                        (next, newFacing) = grid.getCubeTransition(curr, facing);
                    }
                    else if (facing == ivec2.UP && (next.y < 0 || grid[next.y][next.x] == tileType.empty))
                    {
                        (next, newFacing) = grid.getCubeTransition(curr, facing);
                    }

                    if (grid[next.y][next.x] == tileType.open)
                    {
                        facing = newFacing;
                        curr = next;
                    }
                }
            }
            else
            {
                if (inst.turn == 'R')
                {
                    facing.RotateRight();
                }
                else if (inst.turn == 'L')
                {
                    facing.RotateLeft();
                }
            }
        }

        return $"Final cube password: <+white>{getPassword(curr, facing)}";
    }
}