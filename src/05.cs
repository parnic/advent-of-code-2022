using System.Text;

namespace aoc2022;

internal class Day05 : Day
{
    struct Instruction
    {
        public int Num;
        public int From;
        public int To;
    }

    private readonly List<string> defaultLines = new();
    private readonly List<Instruction> instructions = new();

    internal override void Parse()
    {
        int state = 0;
        foreach (var line in Util.ReadAllLines("05"))
        {
            if (state == 0)
            {
                if (line.Contains('['))
                {
                    int col = 0;
                    for (int i = 1; i < line.Length; i += 4)
                    {
                        if (line[i] != ' ')
                        {
                            while (defaultLines.Count <= col)
                            {
                                defaultLines.Add(string.Empty);
                            }
                            defaultLines[col] += line[i];
                        }

                        col++;
                    }
                }
                else if (string.IsNullOrEmpty(line))
                {
                    state = 1;
                }

                continue;
            }

            var inst = line.Split(' ');
            instructions.Add(new Instruction()
            {
                Num = int.Parse(inst[1]),
                From = int.Parse(inst[3]) - 1,
                To = int.Parse(inst[5]) - 1,
            });
        }
    }

    internal override string Part1()
    {
        List<string> lines = new(defaultLines);
        foreach (var inst in instructions)
        {
            for (int i = 0; i < inst.Num; i++)
            {
                var val = lines[inst.From].First();
                lines[inst.From] = lines[inst.From][1..];
                lines[inst.To] = lines[inst.To].Insert(0, val.ToString());
            }
        }

        StringBuilder sb = new();
        foreach (var line in lines)
        {
            sb.Append(line.First());
        }
        return $"CrateMover9000 final order: <+white>{sb}";
    }

    internal override string Part2()
    {
        List<string> lines = new(defaultLines);
        foreach (var inst in instructions)
        {
            for (int i = 0; i < inst.Num; i++)
            {
                var val = lines[inst.From].First();
                lines[inst.From] = lines[inst.From][1..];
                lines[inst.To] = lines[inst.To].Insert(i, val.ToString());
            }
        }

        StringBuilder sb = new();
        foreach (var line in lines)
        {
            sb.Append(line.First());
        }
        return $"CrateMover9001 final order: <+white>{sb}";
    }
}
