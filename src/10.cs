using System.Text;

namespace aoc2022;

internal class Day10 : Day
{
    private record instruction(int mode, int? val);

    private readonly List<instruction> instructions = new();

    internal override void Parse()
    {
        foreach (var line in Util.Parsing.ReadAllLines("10"))
        {
            var vals = line.Split(' ');
            instructions.Add(new instruction(
                vals[0] == "noop" ? 0 : 1,
                vals.Length > 1 ? int.Parse(vals[1]) : null
            ));
        }
    }

    private static void run(IList<instruction> instructions, Action<int, int> duringCallback)
    {
        int instructionPtr = 0;
        int register = 1;
        int? cyclesUntilAdvance = null;
        for (int cycle = 1; instructionPtr < instructions.Count; cycle++)
        {
            // during
            var inst = instructions[instructionPtr];
            switch (inst.mode)
            {
                case 0:
                    instructionPtr++;
                    break;

                case 1:
                    if (cyclesUntilAdvance.HasValue)
                    {
                        cyclesUntilAdvance--;
                        if (cyclesUntilAdvance == 0)
                        {
                            instructionPtr++;
                            cyclesUntilAdvance = null;
                        }
                    }
                    else
                    {
                        cyclesUntilAdvance = 1;
                    }
                    break;

                default:
                    throw new Exception();
            }

            duringCallback(cycle, register);

            // after
            if (cyclesUntilAdvance == null && inst.val.HasValue)
            {
                register += inst.val.Value;
            }
        }
    }

    internal override string Part1()
    {
        List<(int cycle, int signal)> signals = new();
        var importantCycles = new int[] {20, 60, 100, 140, 180, 220};
        run(instructions, (int cycle, int register) =>
        {
            if (importantCycles.Contains(cycle))
            {
                int signal = cycle * register;
                signals.Add((cycle, signal));
            }
        });

        return $"Sum of important cycle values: <+white>{signals.Sum(x => x.signal)}";
    }

    internal override string Part2()
    {
        var bitmap = new bool[6, 40];
        run(
            instructions,
            (cycle, register) =>
            {
                int row = (int)Math.Floor((cycle - 1) / (double)bitmap.GetLength(1));
                int col = (cycle - 1) % bitmap.GetLength(1);
                bitmap[row, col] = Math.Abs(register - col) < 2;
            }
        );

        StringBuilder sb = new();
        sb.Append(Environment.NewLine);
        for (int row = 0; row < bitmap.GetLength(0); row++)
        {
            for (int col = 0; col < bitmap.GetLength(1); col++)
            {
                sb.Append(bitmap[row, col] ? "█" : " ");
            }

            sb.Append(Environment.NewLine);
        }

        return $"CRT display after instructions: <+white>{sb}";
    }
}
