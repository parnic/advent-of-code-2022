namespace aoc2022;

internal class Day09 : Day
{
    private struct move
    {
        public char dir;
        public int steps;

        public override string ToString() => $"{dir} {steps}";
    }

    private class knot
    {
        public int x;
        public int y;
    }

    private readonly List<move> moves = new();

    internal override void Parse()
    {
        foreach (var line in Util.Parsing.ReadAllLines("09"))
        {
            var parts = line.Split(' ');
            moves.Add(new move
            {
                dir = parts[0][0],
                steps = int.Parse(parts[1]),
            });
        }
    }

    private static double dist((int, int) a, (int, int) b) => Math.Sqrt(Math.Pow(b.Item1 - a.Item1, 2) + Math.Pow(b.Item2 - a.Item2, 2));
    private static double dist(knot a, knot b) => dist((a.x, a.y), (b.x, b.y));

    internal override string Part1()
    {
        var headPos = (x: 0, y: 0);
        var tailPos = (x: 0, y: 0);
        HashSet<(int, int)> visited = new()
        {
            { (0, 0) },
        };

        foreach (var move in moves)
        {
            for (int i = 0; i < move.steps; i++)
            {
                switch (move.dir)
                {
                    case 'R':
                        headPos.x += 1;
                        break;
                    case 'L':
                        headPos.x -= 1;
                        break;
                    case 'D':
                        headPos.y += 1;
                        break;
                    case 'U':
                        headPos.y -= 1;
                        break;
                }

                var d = dist(headPos, tailPos);
                if (d < 2)
                {
                    continue;
                }

                if (headPos.x == tailPos.x || headPos.y == tailPos.y)
                {
                    if (headPos.x > tailPos.x)
                    {
                        tailPos.x++;
                    }
                    else if (headPos.x < tailPos.x)
                    {
                        tailPos.x--;
                    }
                    else if (headPos.y > tailPos.y)
                    {
                        tailPos.y++;
                    }
                    else if (headPos.y < tailPos.y)
                    {
                        tailPos.y--;
                    }
                }
                else
                {
                    if (headPos.x > tailPos.x && headPos.y > tailPos.y)
                    {
                        tailPos.x++;
                        tailPos.y++;
                    }
                    else if (headPos.x > tailPos.x && headPos.y < tailPos.y)
                    {
                        tailPos.x++;
                        tailPos.y--;
                    }
                    else if (headPos.x < tailPos.x && headPos.y > tailPos.y)
                    {
                        tailPos.x--;
                        tailPos.y++;
                    }
                    else if (headPos.x < tailPos.x && headPos.y < tailPos.y)
                    {
                        tailPos.x--;
                        tailPos.y--;
                    }
                }

                visited.Add(tailPos);
            }
        }

        return $"Locations tail visited with 2 knots: <+white>{visited.Count}";
    }

    internal override string Part2()
    {
        List<knot> knots = new();
        for (int i = 0; i < 10; i++)
        {
            knots.Add(new knot() {x = 0, y = 0});
        }

        HashSet<(int, int)> visited = new()
        {
            { (0, 0) },
        };

        foreach (var move in moves)
        {
            for (int i = 0; i < move.steps; i++)
            {
                switch (move.dir)
                {
                    case 'R':
                        knots[0].x += 1;
                        break;
                    case 'L':
                        knots[0].x -= 1;
                        break;
                    case 'D':
                        knots[0].y += 1;
                        break;
                    case 'U':
                        knots[0].y -= 1;
                        break;
                }

                for (int k = 1; k < knots.Count; k++)
                {
                    var d = dist(knots[k-1], knots[k]);
                    if (d >= 2)
                    {
                        if (knots[k-1].x == knots[k].x || knots[k-1].y == knots[k].y)
                        {
                            if (knots[k-1].x > knots[k].x)
                            {
                                knots[k].x++;
                            }
                            else if (knots[k-1].x < knots[k].x)
                            {
                                knots[k].x--;
                            }
                            else if (knots[k-1].y > knots[k].y)
                            {
                                knots[k].y++;
                            }
                            else if (knots[k-1].y < knots[k].y)
                            {
                                knots[k].y--;
                            }
                        }
                        else
                        {
                            if (knots[k-1].x > knots[k].x && knots[k-1].y > knots[k].y)
                            {
                                knots[k].x++;
                                knots[k].y++;
                            }
                            else if (knots[k-1].x > knots[k].x && knots[k-1].y < knots[k].y)
                            {
                                knots[k].x++;
                                knots[k].y--;
                            }
                            else if (knots[k-1].x < knots[k].x && knots[k-1].y > knots[k].y)
                            {
                                knots[k].x--;
                                knots[k].y++;
                            }
                            else if (knots[k-1].x < knots[k].x && knots[k-1].y < knots[k].y)
                            {
                                knots[k].x--;
                                knots[k].y--;
                            }
                        }
                    }

                    var tailPos = (knots.Last().x, knots.Last().y);
                    visited.Add(tailPos);
                }
            }
        }

        return $"Locations tail visited with {knots.Count} knots: <+white>{visited.Count}";
    }
}
