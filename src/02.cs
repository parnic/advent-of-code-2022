namespace aoc2022;

internal class Day02 : Day
{
    IEnumerable<string>? lines;

    internal override void Parse()
    {
        lines = Util.Parsing.ReadAllLines("02");
    }

    internal override string Part1()
    {
        int score = 0;
        foreach (var line in lines!)
        {
            int roundScore = 0;
            switch (line[0])
            {
                case 'A' when line[2] == 'X':
                    roundScore += 1;
                    roundScore += 3;
                    break;
                case 'A' when line[2] == 'Y':
                    roundScore += 2;
                    roundScore += 6;
                    break;
                case 'A' when line[2] == 'Z':
                    roundScore += 3;
                    roundScore += 0;
                    break;
                case 'B' when line[2] == 'X':
                    roundScore += 1;
                    roundScore += 0;
                    break;
                case 'B' when line[2] == 'Y':
                    roundScore += 2;
                    roundScore += 3;
                    break;
                case 'B' when line[2] == 'Z':
                    roundScore += 3;
                    roundScore += 6;
                    break;
                case 'C' when line[2] == 'X':
                    roundScore += 1;
                    roundScore += 6;
                    break;
                case 'C' when line[2] == 'Y':
                    roundScore += 2;
                    roundScore += 0;
                    break;
                case 'C' when line[2] == 'Z':
                    roundScore += 3;
                    roundScore += 3;
                    break;
            }

            score += roundScore;
        }

        return $"Score: <+white>{score}";
    }

    internal override string Part2()
    {
        int score = 0;
        foreach (var line in lines!)
        {
            int roundScore = 0;
            switch (line[0])
            {
                case 'A' when line[2] == 'X':
                    roundScore += 3;
                    roundScore += 0;
                    break;
                case 'A' when line[2] == 'Y':
                    roundScore += 1;
                    roundScore += 3;
                    break;
                case 'A' when line[2] == 'Z':
                    roundScore += 2;
                    roundScore += 6;
                    break;
                case 'B' when line[2] == 'X':
                    roundScore += 1;
                    roundScore += 0;
                    break;
                case 'B' when line[2] == 'Y':
                    roundScore += 2;
                    roundScore += 3;
                    break;
                case 'B' when line[2] == 'Z':
                    roundScore += 3;
                    roundScore += 6;
                    break;
                case 'C' when line[2] == 'X':
                    roundScore += 2;
                    roundScore += 0;
                    break;
                case 'C' when line[2] == 'Y':
                    roundScore += 3;
                    roundScore += 3;
                    break;
                case 'C' when line[2] == 'Z':
                    roundScore += 1;
                    roundScore += 6;
                    break;
            }

            score += roundScore;
        }

        return $"Score: <+white>{score}";
    }
}
