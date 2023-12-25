using System;
using System.Collections.Generic;

namespace day7;

internal class Program
{
    class Directory(Directory? parent, string name)
    {
        public Directory? Parent { get; } = parent;

        public string Name { get; } = name;

        public Dictionary<string, Directory> Directories { get; } = new();

        public Dictionary<string, File> Files { get; } = new();

        public long TotalSize { get; private set; }

        public void AddSize(long size)
        {
            TotalSize += size;
            var p = Parent;
            while(p is not null)
            {
                p.TotalSize += size;
                p = p.Parent;
            }
        }
    }

    class File(Directory parent, string name, long size)
    {
        public Directory Parent { get; } = parent;

        public string Name { get; } = name;

        public long Size { get; } = size;
    }

    class Parser
    {
        private Directory _current = null!;

        public Parser()
        {
            _current = Root;
        }

        public Directory Root { get; } = new(null, "/");

        public void ParseLine(string line)
        {
            if(line.StartsWith('$'))
            {
                if(line.StartsWith("$ cd "))
                {
                    if(line == "$ cd ..")
                    {
                        _current = _current.Parent;
                        return;
                    }

                    var name = line.Substring(5);
                    if(name == "/")
                    {
                        _current = Root;
                        return;
                    }
                    if(!_current.Directories.TryGetValue(name, out var dir))
                    {
                        dir = new Directory(_current, name);
                        _current.Directories.Add(name, dir);
                    }
                    _current = dir;
                    return;
                }
                if(line == "$ ls")
                {
                    //
                    return;
                }
            }
            else
            {
                var s = line.IndexOf(' ');
                var name = line.Substring(s + 1);
                if(line.AsSpan(0, s).SequenceEqual("dir"))
                {
                    _current.Directories.TryAdd(name, new(_current, name));
                    return;
                }
                else
                {
                    var size = long.Parse(line.AsSpan(0, s));
                    _current.Files.Add(name, new(_current, name, size));
                    _current.AddSize(size);
                }
            }
        }
    }

    static long Calc(Directory directory)
    {
        const long max = 100000;

        var total = 0L;
        if(directory.TotalSize <= max)
        {
            total += directory.TotalSize;
        }
        foreach(var child in directory.Directories.Values)
        {
            total += Calc(child);
        }
        return total;
    }

    static long Calc2(Directory directory)
    {
        long Total = 70000000;

        var minToRemove = 30_000_000 - (Total - directory.TotalSize);

        const long max = 100000;

        var smallest = default(Directory);

        var stack = new Stack<Directory>();
        stack.Push(directory);
        while(stack.Count > 0)
        {
            var dir = stack.Pop();
            foreach(var child in dir.Directories.Values)
            {
                stack.Push(child);
            }
            if(dir.TotalSize >= minToRemove && (smallest is null || dir.TotalSize < smallest.TotalSize))
            {
                smallest = dir;
            }
        }
        return smallest.TotalSize;
    }

    static void Main(string[] args)
    {
        var parser = new Parser();
        foreach(var line in System.IO.File.ReadAllLines(@"input.txt"))
        {
            if(string.IsNullOrWhiteSpace(line)) continue;

            parser.ParseLine(line);
        }
        Console.WriteLine(Calc2(parser.Root));
    }
}
