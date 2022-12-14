namespace aoc2022;

internal class Day14 : Day
{
    private record point(int x, int y);
    // Parses a string in the form "x,y" into a point
    private static point ParsePoint(string str)
    {
        var parts = str.Trim().Split(',');
        if (parts.Length != 2)
        {
            throw new Exception($"found {parts.Length} pieces of input string, expected 2");
        }

        var x = int.Parse(parts[0].Trim());
        var y = int.Parse(parts[1].Trim());
        return new point(x, y);
    }

    enum cellType
    {
        sand,
        wall,
    }

    private readonly Dictionary<point, cellType> grid = new();

    internal override void Parse()
    {
        foreach (var line in Util.Parsing.ReadAllLines("14"))
        {
            point? lastPoint = null;
            var parts = line.Split(" -> ");
            foreach (var part in parts)
            {
                var p = ParsePoint(part);
                if (lastPoint == null)
                {
                    lastPoint = p;
                    continue;
                }

                if (p.x != lastPoint.x)
                {
                    for (int i = lastPoint.x; i != p.x; i += Math.Sign(p.x - lastPoint.x))
                    {
                        grid[p with {x = i}] = cellType.wall;
                    }
                }
                else if (p.y != lastPoint.y)
                {
                    for (int i = lastPoint.y; i != p.y; i += Math.Sign(p.y - lastPoint.y))
                    {
                        grid[p with {y = i}] = cellType.wall;
                    }
                }
                grid[p] = cellType.wall;
                lastPoint = p;
            }
        }
    }

    internal override string Part1()
    {
        var g = new Dictionary<point, cellType>(grid);
        int lowestY = g.MaxBy(pair => pair.Key.y).Key.y;

        point dropPoint = new point(500, 0);
        bool hitVoid = false;
        int numDroppedSand = 0;
        while (!hitVoid)
        {
            bool atRest = false;
            point sandLoc = dropPoint with {y = dropPoint.y + 1};
            while (!atRest)
            {
                var nextPoint = sandLoc with {y = sandLoc.y + 1};
                if (g.ContainsKey(nextPoint))
                {
                    nextPoint = nextPoint with {x = sandLoc.x - 1};
                    if (g.ContainsKey(nextPoint))
                    {
                        nextPoint = nextPoint with {x = sandLoc.x + 1};
                        if (g.ContainsKey(nextPoint))
                        {
                            atRest = true;
                        }
                    }
                }

                if (!atRest)
                {
                    sandLoc = nextPoint;
                }

                if (nextPoint.y > lowestY)
                {
                    hitVoid = true;
                    break;
                }
            }

            if (!hitVoid)
            {
                numDroppedSand++;
                g[sandLoc] = cellType.sand;
            }
        }

        return $"Sand dropped before hitting the void: <+white>{numDroppedSand}";
    }

    internal override string Part2()
    {
        var g = new Dictionary<point, cellType>(grid);
        int lowestY = g.MaxBy(pair => pair.Key.y).Key.y;
        int floor = 2 + lowestY;

        point dropPoint = new point(500, 0);
        int numDroppedSand = 0;
        while (true)
        {
            bool atRest = false;
            point sandLoc = dropPoint;
            while (!atRest)
            {
                var nextPoint = sandLoc with {y = sandLoc.y + 1};
                if (nextPoint.y == floor)
                {
                    break;
                }

                if (g.ContainsKey(nextPoint))
                {
                    nextPoint = nextPoint with {x = sandLoc.x - 1};
                    if (g.ContainsKey(nextPoint))
                    {
                        nextPoint = nextPoint with {x = sandLoc.x + 1};
                        if (g.ContainsKey(nextPoint))
                        {
                            atRest = true;
                        }
                    }
                }

                if (!atRest)
                {
                    sandLoc = nextPoint;
                }
            }

            g[sandLoc] = cellType.sand;
            numDroppedSand++;

            if (sandLoc == dropPoint)
            {
                break;
            }
        }

        return $"Sand dropped before filling the space: <+white>{numDroppedSand}";
    }
}
