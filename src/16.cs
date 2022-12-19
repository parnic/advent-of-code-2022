using System.Text.RegularExpressions;

namespace aoc2022;

internal partial class Day16 : Day
{
    private class valve : IEquatable<valve>
    {
        public string name = string.Empty;
        public int flowRate = 0;
        public List<string> connectedValveNames = new();
        public readonly List<valve> connectedValves = new();
        public bool isOpen = false;

        public valve()
        {

        }

        public bool Equals(valve? other) => name == other?.name;
        public override bool Equals(object? obj) => obj is valve other && Equals(other);
        public override int GetHashCode() => name.GetHashCode();
        public static bool operator ==(valve left, valve right) => left.Equals(right);
        public static bool operator !=(valve left, valve right) => !left.Equals(right);

        public override string ToString() => $"[{name}] {flowRate} -> {string.Join(", ", connectedValveNames)}";
    }
    private readonly List<valve> valves = new();

    [GeneratedRegex("Valve ([^ ]+) has flow rate=(\\d+); tunnels? leads? to valves? (.+)", RegexOptions.Compiled)]
    private static partial Regex LineRegex();

    internal override void Parse()
    {
        var r = LineRegex();
        foreach (var line in Util.Parsing.ReadAllLines("16"))
        {
            var match = r.Match(line);
            valves.Add(new valve
            {
                name = match.Groups[1].Value,
                flowRate = int.Parse(match.Groups[2].Value),
                connectedValveNames = new List<string>(match.Groups[3].Value.Split(", ")),
            });
        }

        foreach (var valve in valves)
        {
            foreach (var connected in valve.connectedValveNames)
            {
                valve.connectedValves.Add(valves.First(v => v.name == connected));
            }
        }
    }

    private readonly Dictionary<(valve, valve), int> distanceMap = new();
    private int shortestPath(valve from, valve to)
    {
        if (from == to)
        {
            return 0;
        }

        if (distanceMap.ContainsKey((from, to)))
        {
            return distanceMap[(from, to)];
        }

        Dictionary<valve, int> d = new() {{from, 0}};
        HashSet<valve> unvisited = new(valves.Where(v => v != from));
        valve current = from;
        while (true)
        {
            int dist = d[current!];
            foreach (var v in unvisited.Where(v => current!.connectedValves.Contains(v)))
            {
                if (!d.ContainsKey(v) || d[v] > dist + 1)
                {
                    d[v] = dist + 1;
                }
            }

            unvisited.Remove(current!);
            if (current! == to)
            {
                break;
            }

            current = unvisited.MinBy(v => !d.ContainsKey(v) ? int.MaxValue : d[v])!;
        }

        distanceMap[(from, to)] = d[to];
        return d[to];
    }

    private readonly Dictionary<(int, valve, ICollection<valve>), int> routes = new();

    private int findBestRoute(int timeLeft, valve startValve, ICollection<valve> openValves)
    {
        // if (routes.TryGetValue((timeLeft, startValve, openValves), out int retval))
        // {
        //     return retval;
        // }

        if (timeLeft <= 2 || valves.All(v => v.isOpen || v.flowRate == 0))
        {
            // routes.Add((timeLeft, startValve, openValves), 0);
            return 0;
        }

        startValve.isOpen = true;

        var pq = new PriorityQueue<valve, int>();
        foreach (var v in valves.Where(v => v is {isOpen: false, flowRate: > 0}))
        {
            var dist = shortestPath(startValve, v);
            if (dist + 1 >= timeLeft)
            {
                continue;
            }

            var minutesValveContributes = (timeLeft - dist - 1);
            var priority = v.flowRate * minutesValveContributes;
            // var nextOpen = new HashSet<valve>(openValves) {v};
            var totalFlow = findBestRoute(minutesValveContributes, v, openValves);

            pq.Enqueue(v, -(priority + totalFlow));
        }

        startValve.isOpen = false;
        pq.TryDequeue(out valve _, out int p);
        // routes.Add((timeLeft, startValve, openValves), -p);
        return -p;
    }

    private int findBestRouteDuo(int timeLeftA, valve startValveA, int timeLeftB, valve startValveB, ICollection<valve> openValves)
    {
        // if (timeLeftB > timeLeftA)
        if (timeLeftA <= 2)
        {
            (timeLeftA, startValveA, timeLeftB, startValveB) = (timeLeftB, startValveB, timeLeftA, startValveA);
        }

        // if (routes.TryGetValue((timeLeftA, startValveA, openValves), out int retval))
        // {
        //     return retval;
        // }

        if (timeLeftA <= 2 || valves.All(v => v.isOpen || v.flowRate == 0))
        {
            // routes.Add((timeLeftA, startValveA, openValves), 0);
            return 0;
        }

        var pq = new PriorityQueue<valve, int>();
        foreach (var v in valves.Where(v => v is {isOpen: false, flowRate: > 0}))
        {
            var dist = shortestPath(startValveA, v);
            if (dist + 1 >= timeLeftA)
            {
                continue;
            }

            var minutesValveContributes = (timeLeftA - dist - 1);
            var priority = v.flowRate * minutesValveContributes;
            // var nextOpen = new HashSet<valve>(openValves) {v};
            v.isOpen = true;
            int totalFlow = findBestRouteDuo(minutesValveContributes, v, timeLeftB, startValveB, openValves);
            v.isOpen = false;
            pq.Enqueue(v, -(priority + totalFlow));
        }

        pq.TryDequeue(out valve _, out int p);
        // routes.Add((timeLeftA, startValveA, openValves), -p);
        return -p;
    }

    internal override string Part1()
    {
        var openValves = new HashSet<valve>(valves.Where(v => v.flowRate == 0));

        int timeLeft = 30;
        valve currentValve = valves.First(v => v.name == "AA");
        var totalFlow = findBestRoute(timeLeft, currentValve, openValves);
        return $"<+white>{totalFlow}";
    }

    internal override string Part2()
    {
        var openValves = new HashSet<valve>(valves.Where(v => v.flowRate == 0));

        int timeLeft = 26;
        valve currentValve = valves.First(v => v.name == "AA");
        var totalFlow = findBestRouteDuo(timeLeft, currentValve, timeLeft, currentValve, openValves);
        return $"<+white>{totalFlow}";
    }
}
