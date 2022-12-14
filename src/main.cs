using aoc2022;

using aoc2022.Timer t = new("Full program");
var types = System.Reflection.Assembly
             .GetExecutingAssembly()
             .GetTypes()
             .Where(t => t.IsSubclassOf(typeof(Day)) && !t.IsAbstract && t.Name != "DayTemplate")
             .OrderBy(t => t.Name);

bool runAll = false;
bool? runPart1 = null;
bool? runPart2 = null;
List<string> desiredDays = new();
foreach (var arg in args)
{
    if (arg.Equals("-part1", StringComparison.CurrentCultureIgnoreCase))
    {
        runPart1 = true;
    }
    else if (arg.Equals("-part2", StringComparison.CurrentCultureIgnoreCase))
    {
        runPart2 = true;
    }
    else if (arg.Equals("all", StringComparison.CurrentCultureIgnoreCase))
    {
        runAll = true;
    }
    else
    {
        desiredDays.Add(arg);
    }
}

if (runPart1 != null || runPart2 != null)
{
    runPart1 ??= false;
    runPart2 ??= false;
}

if (runAll)
{
    foreach (var type in types)
    {
        using var day = (Day)Activator.CreateInstance(type)!;
        day.Go(runPart1 ?? true, runPart2 ?? true);
    }
}
else
{
    if (desiredDays.Count == 0)
    {
        desiredDays.Add("");
    }

    foreach (var desiredDay in desiredDays)
    {
        Day? day = null;
        if (string.IsNullOrEmpty(desiredDay))
        {
            day = (Day) Activator.CreateInstance(types.Last())!;
        }
        else
        {
            var type = types.FirstOrDefault(x => x.Name == $"Day{desiredDay.PadLeft(2, '0')}");
            if (type == null)
            {
                Logger.Log($"Unknown day <cyan>{desiredDay}<r>");
            }
            else
            {
                day = (Day?) Activator.CreateInstance(type);
            }
        }

        day?.Go(runPart1 ?? true, runPart2 ?? true);
        day?.Dispose();
    }
}
