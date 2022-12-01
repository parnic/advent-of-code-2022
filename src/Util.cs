using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace aoc2022;

internal static class Util
{
    private static readonly char[] StripPreamble = { (char)8745, (char)9559, (char)9488, };
    private static void ReadData(string inputName, Action<string> processor)
    {
        if (Console.IsInputRedirected)
        {
            bool processedSomething = false;
            for (int i = 0; Console.In.ReadLine() is { } line; i++)
            {
                if (i == 0)
                {
                    var preamble = Encoding.UTF8.GetPreamble();
                    if (line[0..preamble.Length].SequenceEqual(preamble.Select(x => (char)x)))
                    {
                        line = line[preamble.Length..];
                    }
                    else if (line[0..StripPreamble.Length].ToCharArray().SequenceEqual(StripPreamble))
                    {
                        line = line[StripPreamble.Length..];
                    }
                }
                processor(line);
                if (!string.IsNullOrEmpty(line))
                {
                    processedSomething = true;
                }
            }

            if (processedSomething)
            {
                return;
            }
        }

        var filename = $"inputs/{inputName}.txt";
        if (File.Exists(filename))
        {
            foreach (var line in File.ReadLines(filename))
            {
                processor(line);
            }

            return;
        }

        // typeof(Util) is not technically correct since what we need is the "default namespace,"
        // but "default namespace" is a Project File concept, not a C#/.NET concept, so it's not
        // accessible at runtime. instead, we assume Util is also part of the "default namespace"
        var resourceName = $"{typeof(Util).Namespace}.inputs.{inputName}.txt";
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        using StreamReader reader = new(stream!);
        while (reader.ReadLine() is { } readLine)
        {
            processor(readLine);
        }
    }

    internal static string ReadAllText(string filename)
    {
        string contents = string.Empty;
        ReadData(filename, (line) => contents = line);
        return contents;
    }

    internal static IEnumerable<string> ReadAllLines(string filename)
    {
        List<string> lines = new();
        ReadData(filename, (line) => lines.Add(line));
        return lines;
    }

    internal static IEnumerable<long> ReadAllLinesAsInts(string filename)
    {
        return ReadAllLines(filename).Select(long.Parse);
    }

    internal static void StartTestSet(string name)
    {
        Logger.Log($"<underline>test: {name}<r>");
    }

    internal static void StartTest(string label)
    {
        Logger.Log($"<magenta>{label}<r>");
    }

    internal static void TestCondition(Func<bool> a, bool printResult = true)
    {
        if (a.Invoke() == false)
        {
            Debug.Assert(false);
            if (printResult)
            {
                Logger.Log("<red>x<r>");
            }
        }
        else
        {
            if (printResult)
            {
                Logger.Log("<green>✓<r>");
            }
        }
    }
}
