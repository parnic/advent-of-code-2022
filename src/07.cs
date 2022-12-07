namespace aoc2022;

internal class Day07 : Day
{
    private class file
    {
        public long size;
        // ReSharper disable once NotAccessedField.Local
        public string name = string.Empty;

        public override string ToString() => $"{name}, {size:N0}b";
    }

    private class dir
    {
        public dir? outer;
        public readonly List<dir> dirs = new();
        public readonly List<file> files = new();
        public string name = string.Empty;

        public long size => files.Sum(x => x.size) + dirs.Sum(x => x.size);

        public override string ToString() => $"{name}, {size:N0}b, {dirs.Count} dir{(dirs.Count == 1 ? "" : "s")}, {files.Count} file{(files.Count == 1 ? "" : "s")}{(outer != null ? $", parent '{outer.name}'" : "")}";
    }

    private readonly dir rootDir = new() {name = "/"};

    internal override void Parse()
    {
        dir? curr = null;

        foreach (var line in Util.ReadAllLines("07"))
        {
            if (line.StartsWith("$"))
            {
                var cmd = line[2..];
                string? arg = null;
                if (cmd.Contains(' '))
                {
                    arg = cmd[(cmd.IndexOf(' ') + 1)..];
                    cmd = cmd[..cmd.IndexOf(' ')];
                }

                if (cmd == "cd")
                {
                    if (arg == "/")
                    {
                        curr = rootDir;
                    }
                    else if (arg == "..")
                    {
                        curr = curr!.outer;
                    }
                    else
                    {
                        curr = curr!.dirs.First(x => x.name == arg);
                    }
                }
            }
            else
            {
                var parts = line.Split(' ');
                if (parts[0] == "dir")
                {
                    curr!.dirs.Add(new dir() { name = parts[1], outer = curr });
                }
                else
                {
                    curr!.files.Add(new file { size = long.Parse(parts[0]), name = parts[1] });
                }
            }
        }
    }

    private static IEnumerable<dir> GetCandidates(dir root, long? threshold = null)
    {
        if (threshold == null || root.size <= threshold)
        {
            yield return root;
        }

        foreach (var dir in root.dirs)
        {
            if (threshold == null || dir.size <= threshold)
            {
                yield return dir;
            }

            foreach (var d in dir.dirs.SelectMany(d2 => GetCandidates(d2, threshold)))
            {
                yield return d;
            }
        }
    }

    internal override string Part1()
    {
        List<dir> candidates = new(GetCandidates(rootDir, 100000));

        return $"Sum of directories below 100,000 bytes: <+white>{candidates.Sum(x => x.size)}";
    }

    internal override string Part2()
    {
        List<dir> flatDirList = new(GetCandidates(rootDir));
        var rootSize = rootDir.size;
        const int totalSize = 70000000;
        var currentFreeSpace = totalSize - rootSize;
        const int totalNeededFreeSpace = 30000000;
        var neededFreeSpace = totalNeededFreeSpace - currentFreeSpace;

        var smallestCandidate = flatDirList.Where(x => x.size >= neededFreeSpace).MinBy(x => x.size);

        return $"Smallest directory that can free the required {neededFreeSpace:N0} bytes: <+white>{smallestCandidate!.size}";
    }
}
