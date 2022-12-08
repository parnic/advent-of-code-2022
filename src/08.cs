namespace aoc2022;

internal class Day08 : Day
{
    private int[][]? trees;

    internal override void Parse()
    {
        var lines = new List<string>(Util.ReadAllLines("08"));
        trees = new int[lines.Count][];
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            trees[i] = new int[line.Length];
            var row = trees[i];
            for (int j = 0; j < line.Length; j++)
            {
                var ch = line[j];
                row[j] = ch - '0';
            }
        }
    }

    internal override string Part1()
    {
        int total = 0;
        for (int row = 0; row < trees!.Length; row++)
        {
            for (int col = 0; col < trees[row].Length; col++)
            {
                if (row == 0 || row == trees.Length - 1 || col == 0 || col == trees[row].Length - 1)
                {
                    total++;
                    continue;
                }

                // from left
                bool blocked = false;
                for (int checkCol = 0; !blocked && checkCol < col; checkCol++)
                {
                    if (trees[row][checkCol] >= trees[row][col])
                    {
                        blocked = true;
                    }
                }
                if (!blocked)
                {
                    total++;
                    continue;
                }

                // from right
                blocked = false;
                for (int checkCol = trees[row].Length - 1; !blocked && checkCol > col; checkCol--)
                {
                    if (trees[row][checkCol] >= trees[row][col])
                    {
                        blocked = true;
                    }
                }
                if (!blocked)
                {
                    total++;
                    continue;
                }

                // from top
                blocked = false;
                for (int checkRow = 0; !blocked && checkRow < row; checkRow++)
                {
                    if (trees[checkRow][col] >= trees[row][col])
                    {
                        blocked = true;
                    }
                }
                if (!blocked)
                {
                    total++;
                    continue;
                }

                // from bottom
                blocked = false;
                for (int checkRow = trees[row].Length - 1; !blocked && checkRow > row; checkRow--)
                {
                    if (trees[checkRow][col] >= trees[row][col])
                    {
                        blocked = true;
                    }
                }
                if (!blocked)
                {
                    total++;
                }
            }
        }

        return $"Total trees visible: <+white>{total}";
    }

    internal override string Part2()
    {
        int highestScore = 0;
        for (int row = 1; row < trees!.Length - 1; row++)
        {
            for (int col = 1; col < trees[row].Length - 1; col++)
            {
                int[] scores = new int[4];
                // to left
                for (int checkCol = col - 1; checkCol >= 0; checkCol--)
                {
                    scores[0]++;
                    if (trees[row][checkCol] >= trees[row][col])
                    {
                        break;
                    }
                }

                // to right
                for (int checkCol = col + 1; checkCol < trees[row].Length; checkCol++)
                {
                    scores[1]++;
                    if (trees[row][checkCol] >= trees[row][col])
                    {
                        break;
                    }
                }

                // to top
                for (int checkRow = row - 1; checkRow >= 0; checkRow--)
                {
                    scores[2]++;
                    if (trees[checkRow][col] >= trees[row][col])
                    {
                        break;
                    }
                }

                // to bottom
                for (int checkRow = row + 1; checkRow < trees.Length; checkRow++)
                {
                    scores[3]++;
                    if (trees[checkRow][col] >= trees[row][col])
                    {
                        break;
                    }
                }

                var finalScore = scores[0] * scores[1] * scores[2] * scores[3];
                if (finalScore > highestScore)
                {
                    highestScore = finalScore;
                }
            }
        }

        return $"Best scenic score: <+white>{highestScore}";
    }
}
