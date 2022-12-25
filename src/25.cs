using System.Collections.Immutable;

namespace aoc2022;

internal static class Day25Extensions
{
    private static readonly ImmutableArray<char> i2s = ImmutableArray.Create('0', '1', '2', '=', '-');

    private static readonly ImmutableDictionary<char, int> s2i = new Dictionary<char, int>
    {
        {'0', 0},
        {'1', 1},
        {'2', 2},
        {'=', -2},
        {'-', -1},
    }.ToImmutableDictionary();

    public static long FromSnafu(this string str)
    {
        long num = 0;

        for (int i = 0; i < str.Length; i++)
        {
            var idx = str.Length - i - 1;
            var place = (long) Math.Pow(5L, i);
            long placeValue = s2i[str[idx]];

            num += placeValue * place;
        }

        return num;
    }

    public static string ToSnafu(this long num)
    {
        Stack<char> sb = new();

        for (int i = 0; num > 0; i++)
        {
            var (quotient, remainder) = Math.DivRem(num, 5);
            sb.Push(i2s[(int)remainder]);
            num = quotient;

            if (remainder > 2)
            {
                num++;
            }
        }

        return new string(sb.ToArray());
    }
}

internal class Day25 : Day
{
    private readonly List<long> nums = new();
    internal override void Parse()
    {
        nums.AddRange(Util.Parsing.ReadAllLines($"{GetDayNum()}").Select(Day25Extensions.FromSnafu).ToList());
    }

    internal override string Part1()
    {
        var total = nums.Sum();
        var snafu = total.ToSnafu();
        return $"Total {total} as SNAFU: <+white>{snafu}";
    }

    internal override string Part2()
    {
        return $"<red>M<green>e<red>r<green>r<red>y <green>C<red>h<green>r<red>i<green>s<red>t<green>m<red>a<green>s<red>!";
    }
}
