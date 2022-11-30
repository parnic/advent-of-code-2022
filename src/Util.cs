using System.Diagnostics;
using System.Text;

namespace aoc2022;

internal static class Util
{
    private static readonly char[] StripPreamble = new char[] { (char)8745, (char)9559, (char)9488, };
    internal static void ReadData(string filename, Action<string> processor)
    {
        if (Console.IsInputRedirected)
        {
            string? line;
            bool processedSomething = false;
            for (int i = 0; (line = Console.In.ReadLine()) != null; i++)
            {
                if (i == 0)
                {
                    var preamble = Encoding.UTF8.GetPreamble();
                    if (Enumerable.SequenceEqual(line[0..preamble.Length], preamble.Select(x => (char)x)))
                    {
                        line = line[preamble.Length..];
                    }
                    else if (Enumerable.SequenceEqual(line[0..StripPreamble.Length].ToCharArray(), StripPreamble))
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

        foreach (var line in File.ReadLines(filename))
        {
            processor(line);
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
        if (a?.Invoke() == false)
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
