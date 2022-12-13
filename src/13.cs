using System.Text;

namespace aoc2022;

internal class Day13 : Day
{
    abstract class entry
    {
    }

    class packet : entry
    {
        public packet(params entry[] _entries)
        {
            entries = new List<entry>(_entries);
        }

        public readonly List<entry> entries;
        public packet? parent;

        public override string ToString() => $"[{string.Join(",", entries)}]";
    }

    class entryint : entry
    {
        public entryint(int _num)
        {
            num = _num;
        }

        public readonly int num;
        public override string ToString() => $"{num}";
    }

    private readonly List<(packet, packet)> packets = new();
    internal override void Parse()
    {
        packet? one = null;
        packet? two = null;
        foreach (var line in Util.Parsing.ReadAllLines("13"))
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (one == null)
            {
                one = parsePacket(line);
            }
            else if (two == null)
            {
                two = parsePacket(line);

                packets.Add((one, two));
                one = null;
                two = null;
            }
        }
    }

    private packet parsePacket(string line)
    {
        packet? curr = new();

        StringBuilder sb = new();
        for (int i = 1; i < line.Length-1; i++)
        {
            var ch = line[i];
            switch (ch)
            {
                case '[':
                    packet embedded = new() { parent = curr };
                    curr!.entries.Add(embedded);
                    curr = embedded;
                    break;

                case ']':
                    if (sb.Length > 0)
                    {
                        curr!.entries.Add(new entryint(int.Parse(sb.ToString())));
                    }

                    sb.Clear();
                    curr = curr!.parent;
                    break;

                case ',':
                    if (sb.Length > 0)
                    {
                        curr!.entries.Add(new entryint(int.Parse(sb.ToString())));
                    }

                    sb.Clear();
                    break;

                default:
                    sb.Append(ch);
                    break;
            }
        }
        if (sb.Length > 0)
        {
            curr!.entries.Add(new entryint(int.Parse(sb.ToString())));
        }

        return curr!;
    }

    private bool? isValid((packet, packet) pair)
    {
        for (int i = 0; i < pair.Item1.entries.Count; i++)
        {
            if (pair.Item2.entries.Count <= i)
            {
                return false;
            }

            var left = pair.Item1.entries[i];
            var right = pair.Item2.entries[i];
            if (left is entryint li && right is entryint ri)
            {
                if (li.num < ri.num)
                {
                    return true;
                }

                if (li.num > ri.num)
                {
                    return false;
                }
            }
            else if (left is packet ll && right is packet rl)
            {
                var valid = isValid((ll, rl));
                if (valid.HasValue)
                {
                    return valid;
                }
            }
            else
            {
                var one = left as packet;
                var two = right as packet;
                if (left is entryint ei1)
                {
                    one = new packet(ei1);
                }
                else if (right is entryint ei2)
                {
                    two = new packet(ei2);
                }

                var valid = isValid((one!, two!));
                if (valid.HasValue)
                {
                    return valid;
                }
            }
        }

        if (pair.Item1.entries.Count < pair.Item2.entries.Count)
        {
            return true;
        }

        return null;
    }

    internal override string Part1()
    {
        List<int> validIndices = new();
        for (int i = 0; i < packets.Count; i++)
        {
            var pair = packets[i];
            var valid = isValid(pair);
            if (valid != false)
            {
                validIndices.Add(i + 1);
            }
        }

        return $"Sum of valid indices (1-based): <+white>{validIndices.Sum()}";
    }

    internal override string Part2()
    {
        var sentinels = new List<packet>
        {
            parsePacket("[[2]]"),
            parsePacket("[[6]]"),
        };
        var l = new List<packet>(sentinels);
        foreach (var p in packets)
        {
            l.Add(p.Item1);
            l.Add(p.Item2);
        }
        l.Sort((a, b) => isValid((a, b)) != false ? -1 : 1);

        int total = 1;
        foreach (var s in sentinels)
        {
            total *= l.FindIndex(p => p.ToString() == s.ToString()) + 1;
        }

        return $"Sum of sentinel packet indices (1-based) after sorting: <+white>{total}";
    }
}
