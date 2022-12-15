namespace aoc2022;

internal class Day14 : Day
{
    class point : IEquatable<point>
    {
        public bool Equals(point? other)
        {
            return x == other?.x && y == other.y;
        }

        public override bool Equals(object? obj)
        {
            return Equals((point?) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static bool operator ==(point? left, point? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(point? left, point? right)
        {
            return !Equals(left, right);
        }

        public point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public override string ToString()
        {
            return $"{x},{y}";
        }

        public int x;
        public int y;

        // Parses a string in the form "x,y" into a point
        public static point Parse(string str)
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
    }

    private readonly HashSet<point> grid = new();

    internal override void Parse()
    {
        foreach (var line in Util.Parsing.ReadAllLines("14"))
        {
            point? lastPoint = null;
            var parts = line.Split(" -> ").Select(point.Parse);
            foreach (var p in parts)
            {
                if (lastPoint == null)
                {
                    lastPoint = p;
                    continue;
                }

                if (p.x != lastPoint.x)
                {
                    for (int i = lastPoint.x; i != p.x; i += Math.Sign(p.x - lastPoint.x))
                    {
                        grid.Add(new point(i, p.y));
                    }
                }
                else if (p.y != lastPoint.y)
                {
                    for (int i = lastPoint.y; i != p.y; i += Math.Sign(p.y - lastPoint.y))
                    {
                        grid.Add(new point(p.x, i));
                    }
                }
                lastPoint = p;
            }
        }
    }

    internal override string Part1()
    {
        var g = new HashSet<point>(grid);
        int lowestY = g.MaxBy(p => p.y)!.y;

        point dropPoint = new point(500, 0);
        bool hitVoid = false;
        int numDroppedSand = 0;
        point sandLoc = new point(dropPoint.x, dropPoint.y);
        point nextPoint = new point(sandLoc.x, sandLoc.y + 1);
        while (!hitVoid)
        {
            bool atRest = false;
            sandLoc.x = dropPoint.x;
            sandLoc.y = dropPoint.y;
            while (!atRest)
            {
                nextPoint.x = sandLoc.x;
                nextPoint.y = sandLoc.y + 1;
                if (g.Contains(nextPoint))
                {
                    nextPoint.x = sandLoc.x - 1;
                    if (g.Contains(nextPoint))
                    {
                        nextPoint.x = sandLoc.x + 1;
                        if (g.Contains(nextPoint))
                        {
                            atRest = true;
                        }
                    }
                }

                if (!atRest)
                {
                    sandLoc.x = nextPoint.x;
                    sandLoc.y = nextPoint.y;
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
                g.Add(new point(sandLoc.x, sandLoc.y));
            }
        }

        return $"Sand dropped before hitting the void: <+white>{numDroppedSand}";
    }

    internal override string Part2()
    {
        var g = new HashSet<point>(grid);
        int lowestY = g.MaxBy(p => p.y)!.y;
        int floor = 2 + lowestY;

        point dropPoint = new point(500, 0);
        int numDroppedSand = 0;
        point sandLoc = new point(dropPoint.x, dropPoint.y);
        point nextPoint = new point(sandLoc.x, sandLoc.y + 1);
        while (true)
        {
            bool atRest = false;
            sandLoc.x = dropPoint.x;
            sandLoc.y = dropPoint.y;
            while (!atRest)
            {
                nextPoint.x = sandLoc.x;
                nextPoint.y = sandLoc.y + 1;

                if (nextPoint.y == floor)
                {
                    break;
                }

                if (g.Contains(nextPoint))
                {
                    nextPoint.x = sandLoc.x - 1;
                    if (g.Contains(nextPoint))
                    {
                        nextPoint.x = sandLoc.x + 1;
                        if (g.Contains(nextPoint))
                        {
                            atRest = true;
                        }
                    }
                }

                if (!atRest)
                {
                    sandLoc.x = nextPoint.x;
                    sandLoc.y = nextPoint.y;
                }
            }

            g.Add(new point(sandLoc.x, sandLoc.y));
            numDroppedSand++;

            if (sandLoc == dropPoint)
            {
                break;
            }
        }

        return $"Sand dropped before filling the space: <+white>{numDroppedSand}";
    }
}
