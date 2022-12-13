namespace aoc2022;

internal class Day12 : Day
{
    class point : IEquatable<point>
    {
        public bool Equals(point? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x == other.x && y == other.y;
        }

        public override int GetHashCode() => HashCode.Combine(x, y);

        public readonly int x;
        public readonly int y;

        public static point operator +(point p, point other) => new(p.x + other.x, p.y + other.y);
        public static bool operator ==(point p, point other) => p.x == other.x && p.y == other.y;
        public static bool operator !=(point p, point other) => !(p == other);
        public override bool Equals(object? obj) => obj is point p && p == this;
        public override string ToString() => $"{x},{y}";

        public point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    class DNode : IEquatable<DNode>
    {
        public bool Equals(DNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return location.Equals(other.location);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DNode) obj);
        }

        public static bool operator ==(DNode? left, DNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DNode? left, DNode? right)
        {
            return !Equals(left, right);
        }

        public readonly List<DNode> neighbors = new();
        public readonly point location;
        public long? distance;

        public DNode(point loc)
        {
            location = loc;
        }

        public override int GetHashCode() => location.GetHashCode();
        public override string ToString() => $"{location}, {neighbors.Count} neighbor{(neighbors.Count == 1 ? "" : "s")}, dist: {distance ?? -1}";
    }

    private readonly HashSet<DNode> allNodes = new();
    private DNode? startNode;
    private DNode? goalNode;

    private int[][]? heights;
    private point? start;
    private point? goal;

    private readonly point[] offsets = new point[]
    {
        new(-1, 0),
        new(1, 0),
        new(0, -1),
        new(0, 1),
    };

    internal override void Parse()
    {
        var lines = new List<string>(Util.Parsing.ReadAllLines("12"));
        heights = new int[lines.Count][];
        for (int x = 0; x < lines.Count; x++)
        {
            var line = lines[x];
            heights[x] = new int[line.Length];
            for (int y = 0; y < line.Length; y++)
            {
                var ch = line[y];
                switch (ch)
                {
                    case 'S':
                        start = new(x, y);
                        ch = 'a';
                        break;
                    case 'E':
                        goal = new(x, y);
                        ch = 'z';
                        break;
                }

                heights[x][y] = ch - 'a';
                allNodes.Add(new DNode(new point(x, y)));
            }
        }

        startNode = new DNode(start!);

        var unvisited = new HashSet<DNode>(allNodes);
        DNode? current = startNode;
        while (current != null)
        {
            foreach (var offset in offsets)
            {
                var test = current.location + offset;
                if (test.x < 0 || test.y < 0 || test.x >= heights.Length || test.y >= heights[0].Length)
                {
                    continue;
                }

                var found = allNodes.First(n => n.location == test);
                current.neighbors.Add(found);
            }

            unvisited.Remove(current);
            current = unvisited.FirstOrDefault();
        }

        goalNode = allNodes.First(n => n.location == goal!);
    }

    private void DijkstraFlood(DNode from, DNode to, bool reverse = false)
    {
        allNodes.ForEach(n =>
        {
            n.distance = null;
        });
        from.distance = 0;

        var unvisited = new HashSet<DNode>(allNodes);
        DNode? current = from;
        while (current != null)
        {
            var currentHeight = heights![current.location.x][current.location.y];
            var unvisitedNeighbors = current.neighbors.Where(n => unvisited.Contains(n));
            if (!reverse)
            {
                unvisitedNeighbors =
                    unvisitedNeighbors.Where(n => heights[n.location.x][n.location.y] <= currentHeight + 1);
            }
            else
            {
                unvisitedNeighbors =
                    unvisitedNeighbors.Where(n => currentHeight <= heights[n.location.x][n.location.y] + 1);
            }
            foreach (var n in unvisitedNeighbors)
            {
                if (n.distance == null || n.distance > current.distance + 1)
                {
                    n.distance = current.distance + 1;
                }
            }

            unvisited.Remove(current);
            if (current == to)
            {
                break;
            }

            current = unvisited.MinBy(n => n.distance);
        }
    }

    internal override string Part1()
    {
        DijkstraFlood(startNode!, goalNode!);
        return $"Shortest distance from start to end: <+white>{goalNode!.distance}";
    }

    internal override string Part2()
    {
        DijkstraFlood(goalNode!, startNode!, true);
        var shortest = allNodes.Where(n => n.distance != null && heights![n.location.x][n.location.y] == 0).MinBy(n => n.distance);
        return $"Shortest distance from any minimum-height point to end: <+white>{shortest!.distance}";
    }
}
