namespace aoc2022;

internal class Day21 : Day
{
    private record monkeyOp(int num, string m1, string m2, string op);

    private readonly Dictionary<string, monkeyOp> monkeys = new();

    internal override void Parse()
    {
        foreach (var line in Util.Parsing.ReadAllLines("21"))
        {
            var parts = line.Split(": ");
            if (parts[1].Contains(" "))
            {
                var op = parts[1].Split(' ');
                monkeys.Add(parts[0], new monkeyOp(0, op[0], op[2], op[1]));
            }
            else
            {
                monkeys.Add(parts[0], new monkeyOp(int.Parse(parts[1]), string.Empty, string.Empty, string.Empty));
            }
        }
    }

    private long getResultFromMonkey(string monkey)
    {
        var op = monkeys[monkey];
        if (string.IsNullOrEmpty(op.m1))
        {
            return op.num;
        }

        var left = getResultFromMonkey(op.m1);
        var right = getResultFromMonkey(op.m2);

        return op.op switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            _ => throw new Exception("unhandled"),
        };
    }

    private long getRequiredHumnResult(string monkey, long? target = null)
    {
        if (monkey == "humn")
        {
            return target!.Value;
        }

        var leftMonkey = monkeys[monkey].m1;
        if (string.IsNullOrEmpty(leftMonkey))
        {
            return monkeys[monkey].num;
        }

        var rightMonkey = monkeys[monkey].m2;
        var left1 = getResultFromMonkey(leftMonkey);
        var right1 = getResultFromMonkey(rightMonkey);

        monkeys["humn"] = new monkeyOp(monkeys["humn"].num == 0 ? 1000000 : 0, string.Empty, string.Empty, string.Empty);
        var left2 = getResultFromMonkey(leftMonkey);

        long result;
        if (left1 != left2)
        {
            target = monkeys[monkey].op switch
            {
                "+" => target - right1,
                "-" => target + right1,
                "*" => target / right1,
                "/" => target * right1,
                _ => throw new Exception("unhandled"),
            };
            result = getRequiredHumnResult(leftMonkey, target ?? right1);
        }
        else
        {
            target = monkeys[monkey].op switch
            {
                "+" => target - left1,
                "-" => left1 - target,
                "*" => target / left1,
                "/" => left1 / target,
                _ => throw new Exception("unhandled"),
            };
            result = getRequiredHumnResult(rightMonkey, target ?? left1);
        }

        return result;
    }

    internal override string Part1()
    {
        var result = getResultFromMonkey("root");

        return $"<+white>{result}";
    }

    internal override string Part2()
    {
        var result = getRequiredHumnResult("root");

        return $"<+white>{result}";
    }
}
