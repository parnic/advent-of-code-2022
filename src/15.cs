using System.Drawing;
using System.Text.RegularExpressions;
using aoc2022.Util;
using Math = System.Math;

namespace aoc2022;

internal class Day15 : Day
{
    private List<ivec2> knownSensors = new();
    private List<ivec2> knownBeacons = new();

    internal override void Parse()
    {
        Regex r = new(@"=(-?\d+)", RegexOptions.Compiled);
        foreach (var line in Parsing.ReadAllLines("15"))
        {
            var m = r.Matches(line);
            knownSensors.Add(new ivec2(int.Parse(m[0].Groups[1].Value), int.Parse(m[1].Groups[1].Value)));
            knownBeacons.Add(new ivec2(int.Parse(m[2].Groups[1].Value), int.Parse(m[3].Groups[1].Value)));
        }
    }

    private bool IsReachableFromSensor(ivec2 sensor, long maxDist, ivec2 point)
    {
        return point.ManhattanDistanceTo(sensor) <= maxDist;
    }

    private bool IsReachableFromAnySensor(ivec2 point)
    {
        for (int i = 0; i < knownSensors.Count; i++)
        {
            var s = knownSensors[i];
            var b = knownBeacons[i];
            var dist = s.ManhattanDistanceTo(b);
            if (IsReachableFromSensor(s, dist, point))
            {
                return true;
            }
        }

        return false;
    }

    internal override string Part1()
    {
        int interestedY = 2000000;
        HashSet<long> emptySpace = new(10_000_000);
        for (int i = 0; i < knownSensors.Count; i++)
        {
            var s = knownSensors[i];
            var b = knownBeacons[i];
            var dist = s.ManhattanDistanceTo(b);

            var yDist = Math.Abs(interestedY - s.y);
            var xWidth = dist - yDist;
            if (xWidth >= 0)
            {
                for (long j = s.x - xWidth; j <= s.x + xWidth; j++)
                {
                    emptySpace.Add(j);
                }
            }
        }

        knownBeacons.Where(b => b.y == interestedY).ForEach(b => emptySpace.Remove(b.x));
        return $"Empty spaces at y={interestedY:N0}: <+white>{emptySpace.Count}";
    }

    internal override string Part2()
    {
        int pointsTested = 0;
        int max = 4000000;
        ivec2? answer = null;
        for (int i = 0; i < knownSensors.Count; i++)
        {
            var s = knownSensors[i];
            var b = knownBeacons[i];
            var exclusionDist = s.ManhattanDistanceTo(b);
            var top = new ivec2(s.x, s.y - exclusionDist - 1);
            var dir = new ivec2(-1, 1);
            var pt = top;
            while (answer == null)
            {
                pointsTested++;
                if (pt.x >= 0 && pt.y >= 0 && pt.x <= max && pt.y <= max && !IsReachableFromAnySensor(pt))
                {
                    answer = pt;
                    break;
                }

                if (pt.y == s.y)
                {
                    break;
                }

                pt += dir;
            }

            dir = new ivec2(1, dir.y);
            while (answer == null)
            {
                pointsTested++;
                if (pt.x >= 0 && pt.y >= 0 && pt.x <= max && pt.y <= max && !IsReachableFromAnySensor(pt))
                {
                    answer = pt;
                    break;
                }

                if (pt.x == s.x)
                {
                    break;
                }

                pt += dir;
            }

            dir = new ivec2(dir.x, -1);
            while (answer == null)
            {
                pointsTested++;
                if (pt.x >= 0 && pt.y >= 0 && pt.x <= max && pt.y <= max && !IsReachableFromAnySensor(pt))
                {
                    answer = pt;
                    break;
                }

                if (pt.y == s.y)
                {
                    break;
                }

                pt += dir;
            }

            dir = new ivec2(-1, dir.y);
            while (answer == null)
            {
                pointsTested++;
                if (pt.x >= 0 && pt.y >= 0 && pt.x <= max && pt.y <= max && !IsReachableFromAnySensor(pt))
                {
                    answer = pt;
                    break;
                }

                if (pt == top)
                {
                    break;
                }

                pt += dir;
            }
        }

        return $"After testing {pointsTested:N0} points, found distress beacon at {answer} with tuning frequency: <+white>{(answer!.Value.x * 4000000L) + answer.Value.y}";
    }
}
