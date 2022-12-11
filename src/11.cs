namespace aoc2022;

internal class Day11 : Day
{
    private class monkey
    {
        public readonly Queue<long> startingItems = new();
        public Queue<long> items = new();
        public string operation = string.Empty;
        public long testDivBy;
        public int trueTarget;
        public int falseTarget;
        public long timesInspected;
    }

    private readonly List<monkey> monkeys = new();
    private long divideByProduct = 1;

    internal override void Parse()
    {
        monkey? m = null;
        foreach (var line in Util.Parsing.ReadAllLines("11"))
        {
            if (line.StartsWith("Monkey"))
            {
                m = new monkey();
                monkeys.Add(m);
            }
            else if (line.Trim().StartsWith("Starting items"))
            {
                var parts = line.Split(':');
                var items = parts[1].Split(',');
                foreach (var item in items)
                {
                    m!.startingItems.Enqueue(int.Parse(item.Trim()));
                }
            }
            else if (line.Trim().StartsWith("Operation"))
            {
                m!.operation = line.Split(':')[1].Trim()["new = ".Length..];
            }
            else if (line.Trim().StartsWith("Test"))
            {
                m!.testDivBy = int.Parse(line.Trim().Split(' ')[3]);
                divideByProduct *= m.testDivBy;
            }
            else if (line.Trim().StartsWith("If true"))
            {
                m!.trueTarget = int.Parse(line.Trim().Split(' ')[5]);
            }
            else if (line.Trim().StartsWith("If false"))
            {
                m!.falseTarget = int.Parse(line.Trim().Split(' ')[5]);
            }
        }
    }

    private void monkeyRound(Func<long, long> worryReducer)
    {
        foreach (var m in monkeys)
        {
            while (m.items.Count > 0)
            {
                long item = m.items.Dequeue();

                long[] operands = new long[2];
                var op = m.operation.Replace("old", $"{item}");
                var parts = op.Split(' ');
                operands[0] = long.Parse(parts.First());
                operands[1] = long.Parse(parts.Last());
                if (m.operation.Contains('*'))
                {
                    item = operands[0] * operands[1];
                }
                else
                {
                    item = operands[0] + operands[1];
                }

                item = worryReducer(item);

                if (item % m.testDivBy == 0)
                {
                    monkeys[m.trueTarget].items.Enqueue(item);
                }
                else
                {
                    monkeys[m.falseTarget].items.Enqueue(item);
                }

                m.timesInspected++;
            }
        }
    }

    internal override string Part1()
    {
        monkeys.ForEach(m =>
        {
            m.timesInspected = 0;
            m.items = new(m.startingItems);
        });
        for (int round = 0; round < 20; round++)
        {
            monkeyRound(worry => worry / 3);
        }

        var sorted = monkeys.OrderByDescending(m => m.timesInspected);
        var monkeyBusiness = sorted.Take(2).Aggregate(1L, (x, y) => x * y.timesInspected);
        return $"Monkey business after 20 rounds, dividing worry by 3: <+white>{monkeyBusiness}";
    }

    internal override string Part2()
    {
        // still hurting for a deep copy utility...
        monkeys.ForEach(m =>
        {
            m.timesInspected = 0;
            m.items = new(m.startingItems);
        });

        for (int round = 0; round < 10000; round++)
        {
            monkeyRound(worry => worry % divideByProduct);
        }

        var sorted = monkeys.OrderByDescending(m => m.timesInspected);
        var monkeyBusiness = sorted.Take(2).Aggregate(1L, (x, y) => x * y.timesInspected);
        return $"Monkey business after 10,000 rounds, reducing worry 'another way': <+white>{monkeyBusiness}";
    }
}