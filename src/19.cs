using System.Text.RegularExpressions;

namespace aoc2022;

internal partial class Day19 : Day
{
    private enum resource
    {
        ore = 1,
        clay = 2,
        obsidian = 3,
        geode = 4,
    }

    private class cost
    {
        public resource type;
        public int amount;

        public override string ToString() => $"{amount} {type}";
    }

    private class blueprint
    {
        public int id;
        public Dictionary<resource, List<cost>> robots = new();

        public override string ToString() => $"({robots.Count}) {string.Join("; ", robots)}";
    }

    private class miningSim
    {
        public miningSim(blueprint bp, int timeLimit = 24)
        {
            this.bp = bp;
            this.timeLimit = timeLimit;
        }

        public override string ToString() => $"{resources[resource.ore]}r {clayCount}c {obsidianCount}o {geodeCount}g @{timePassed}m with bots {robots[resource.ore]}r {clayRobotCount}c {obsidianRobotCount}o {geodeRobotCount}g";

        private int? cachedUpperBound;
        public readonly int timeLimit;

        public int GetFinalValueUpperBound()
        {
            cachedUpperBound ??= GetFinalGeodesUpperBound(this);
            return cachedUpperBound.Value;
        }

        private miningSim Clone()
        {
            var tmp = (miningSim) MemberwiseClone();
            tmp.cachedUpperBound = null;
            tmp.robots = new Dictionary<resource, int>(tmp.robots);
            tmp.resources = new Dictionary<resource, int>(tmp.resources);
            tmp.shouldWait = false;
            return tmp;
        }

        public miningSim advance()
        {
            var copy = Clone();
            copy.internalAdvance();
            return copy;
        }

        private void internalAdvance()
        {
            shouldWait = false;
            timePassed++;
            foreach (var r in robots)
            {
                resources[r.Key] += r.Value;
            }
        }

        public miningSim buildRobot(resource type)
        {
            var copy = Clone();
            copy.internalBuildRobot(type);
            return copy;
        }

        private static readonly cost freeCost = new() {type = resource.ore, amount = 0};

        private (cost cost1, cost cost2) getCosts(resource r)
        {
            var robotDef = bp.robots[r];
            var cost1 = robotDef.First();
            var cost2 = (robotDef.Count() > 1 ? robotDef.Last() : freeCost);
            return (cost1, cost2);
        }

        private void internalBuildRobot(resource r)
        {
            var (cost1, cost2) = getCosts(r);

            resources[cost1.type] -= cost1.amount;
            if (resources[cost1.type] < 0)
            {
                throw new Exception($"sim {cost1.type} amount too low");
            }
            resources[cost2.type] -= cost2.amount;
            if (resources[cost2.type] < 0)
            {
                throw new Exception($"sim {cost2.type} amount too low");
            }

            internalAdvance();

            robots[r]++;
        }

        private enum buildStatus
        {
            cantBuild,
            canBuild,
            buildableEventually,
        }

        buildStatus getBuildStatus(resource r)
        {
            var (cost1, cost2) = getCosts(r);

            if (resources[cost1.type] >= cost1.amount && resources[cost2.type] >= cost2.amount)
            {
                return buildStatus.canBuild;
            }

            if (robots[cost1.type] > 0 && robots[cost2.type] > 0)
            {
                return buildStatus.buildableEventually;
            }

            return buildStatus.cantBuild;
        }

        private IEnumerable<KeyValuePair<resource, List<cost>>> getBotsRequiring(resource res)
        {
            return bp.robots.Where(bot => bot.Value.Any(r => r.type == res));
        }

        public IEnumerable<resource> GetBuildableRobots()
        {
            foreach (var robot in bp.robots)
            {
                var status = getBuildStatus(robot.Key);
                if (status == buildStatus.canBuild)
                {
                    yield return robot.Key;
                }
                else if (status == buildStatus.buildableEventually)
                {
                    shouldWait = true;
                }
            }
        }

        private readonly blueprint bp;
        public int timePassed { get; private set; }
        public int geodeCount => resources[resource.geode];
        public int geodeRobotCount => robots[resource.geode];

        public int obsidianCount => resources[resource.obsidian];
        public int obsidianRobotCount => robots[resource.obsidian];
        public int geodeRobotObsidianCost => bp.robots[resource.geode][1].amount;

        public int clayCount => resources[resource.clay];
        public int clayRobotCount => robots[resource.clay];
        public int obsidianRobotClayCost => bp.robots[resource.obsidian][1].amount;

        public double geodesPerMinute => 1.0 * geodeCount / timePassed;
        public bool shouldWait { get; private set; }

        private Dictionary<resource, int> robots = new()
        {
            {resource.ore, 1},
            {resource.clay, 0},
            {resource.obsidian, 0},
            {resource.geode, 0},
        };

        private Dictionary<resource, int> resources = new()
        {
            {resource.ore, 0},
            {resource.clay, 0},
            {resource.obsidian, 0},
            {resource.geode, 0},
        };
    }

    private readonly List<blueprint> blueprints = new();

    [GeneratedRegex(
        "Blueprint (?<num>\\d+): Each (?<robottype1>.+?) robot costs (?<count1>\\d+) (?<costtype1>.+?)\\. Each (?<robottype2>.+?) robot costs (?<count2>\\d+) (?<costtype2>.+?)\\. Each (?<robottype3>.+?) robot costs (?<count3_1>\\d+) (?<costtype3_1>.+?) and (?<count3_2>\\d+) (?<costtype3_2>.+?)\\. Each (?<robottype4>.+?) robot costs (?<count4_1>\\d+) (?<costtype4_1>.+?) and (?<count4_2>\\d+) (?<costtype4_2>.+?)\\.")]
    private static partial Regex BlueprintRegex();

    internal override void Parse()
    {
        Regex r = BlueprintRegex();
        foreach (var line in Util.Parsing.ReadAllLines("19"))
        {
            var match = r.Match(line);

            cost orecost = new()
            {
                type = Enum.Parse<resource>(match.Groups["costtype1"].Value),
                amount = int.Parse(match.Groups["count1"].Value),
            };

            cost claycost = new()
            {
                type = Enum.Parse<resource>(match.Groups["costtype2"].Value),
                amount = int.Parse(match.Groups["count2"].Value),
            };

            cost obsidiancost1 = new()
            {
                type = Enum.Parse<resource>(match.Groups["costtype3_1"].Value),
                amount = int.Parse(match.Groups["count3_1"].Value),
            };
            cost obsidiancost2 = new()
            {
                type = Enum.Parse<resource>(match.Groups["costtype3_2"].Value),
                amount = int.Parse(match.Groups["count3_2"].Value),
            };

            cost geodecost1 = new()
            {
                type = Enum.Parse<resource>(match.Groups["costtype4_1"].Value),
                amount = int.Parse(match.Groups["count4_1"].Value),
            };
            cost geodecost2 = new()
            {
                type = Enum.Parse<resource>(match.Groups["costtype4_2"].Value),
                amount = int.Parse(match.Groups["count4_2"].Value),
            };

            blueprint bp = new()
            {
                id = blueprints.Count + 1,
                robots = new Dictionary<resource, List<cost>>
                {
                    {resource.ore, new List<cost> {orecost}},
                    {resource.clay, new List<cost> {claycost}},
                    {resource.obsidian, new List<cost> {obsidiancost1, obsidiancost2}},
                    {resource.geode, new List<cost> {geodecost1, geodecost2}},
                },
            };

            blueprints.Add(bp);
        }
    }

    private static int GetFinalGeodesUpperBound(miningSim sim)
    {
        int geodeRobotCount = sim.geodeRobotCount;
        int obsidianRobotCount = sim.obsidianRobotCount;
        int clayRobotCount = sim.clayRobotCount;

        int obsidianCount = sim.obsidianCount;
        int obsidianCost = sim.geodeRobotObsidianCost;

        int clayCount = sim.clayCount;
        int clayCost = sim.obsidianRobotClayCost;

        int geodeCount = sim.geodeCount;
        for (int i = sim.timePassed; i < sim.timeLimit; i++)
        {
            var newClayRobotCount = clayRobotCount;
            var newObsidianRobotCount = obsidianRobotCount;
            var newGeodeRobotCount = geodeRobotCount;

            if (obsidianCount >= obsidianCost)
            {
                obsidianCount -= obsidianCost;
                newGeodeRobotCount++;
            }

            if (clayCount >= clayCost)
            {
                clayCount -= clayCost;
                newObsidianRobotCount++;
            }

            newClayRobotCount++;

            clayCount += clayRobotCount;
            obsidianCount += obsidianRobotCount;
            geodeCount += geodeRobotCount;

            geodeRobotCount = newGeodeRobotCount;
            obsidianRobotCount = newObsidianRobotCount;
            clayRobotCount = newClayRobotCount;
        }

        return geodeCount;
    }

    private int FindMaxGeodes(miningSim sim)
    {
        var queue = new PriorityQueue<miningSim,
            (double rate, int timePassed, int estimatedFutureValue)>();
        queue.Enqueue(sim,
            (0.0, 0, 0)
        );

        int bestGeodes = 0;

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            if (state.timePassed > state.timeLimit)
            {
                continue;
            }

            if (state.geodeCount > bestGeodes)
            {
                bestGeodes = state.geodeCount;
            }

            if (state.timePassed == state.timeLimit)
            {
                continue;
            }

            var finalGeodesUpperBound = state.GetFinalValueUpperBound();
            if (finalGeodesUpperBound <= bestGeodes)
            {
                continue;
            }

            foreach (var toBuild in state.GetBuildableRobots())
            {
                var nextState = state.buildRobot(toBuild);
                queue.Enqueue(nextState,
                    (-nextState.geodesPerMinute, -nextState.timePassed, -nextState.GetFinalValueUpperBound())
                );
            }

            if (state.shouldWait)
            {
                var nextState = state.advance();
                queue.Enqueue(nextState,
                    (-nextState.geodesPerMinute, -nextState.timePassed, -nextState.GetFinalValueUpperBound())
                );
            }
        }

        return bestGeodes;
    }

    internal override string Part1()
    {
        var lst = blueprints.Select(sim => (sim.id, maxGeodes: FindMaxGeodes(new miningSim(sim))));

        var qualitySum = lst.Select(x => x.id * x.maxGeodes).Sum();
        return $"Quality level of all blueprints for 24 minutes: <+white>{qualitySum}";
    }

    internal override string Part2()
    {
        var g1 = FindMaxGeodes(new miningSim(blueprints[0], 32));
        var g2 = FindMaxGeodes(new miningSim(blueprints[1], 32));
        var g3 = FindMaxGeodes(new miningSim(blueprints[2], 32));
        return $"Sum of geodes opened by first 3 blueprints after 32 minutes: <+white>{g1*g2*g3}";
    }
}