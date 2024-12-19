namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/19"/></remarks>
[Name(@"Linen Layout")]
public abstract class Day19Solution : Solution
{
	protected static string[] ReadPatterns(TextReader reader)
	{
		var patterns = (reader.ReadLine() ?? throw new InvalidDataException())
			.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		reader.ReadLine();
		return patterns;
	}
}

public sealed class Day19SolutionPart1 : Day19Solution
{
	static bool IsPossible(Dictionary<string, bool> cache, string[] patterns, ReadOnlySpan<char> design)
	{
		if(design.Length == 0) return true;

		var lookup = cache.GetAlternateLookup<ReadOnlySpan<char>>();
		if(lookup.TryGetValue(design, out var isPossible)) return isPossible;

		foreach(var p in patterns)
		{
			if(design.StartsWith(p))
			{
				if(IsPossible(cache, patterns, design[p.Length..]))
				{
					lookup.TryAdd(design, true);
					return true;
				}
			}
		}
		lookup.TryAdd(design, false);
		return false;
	}

	public override string Process(TextReader reader)
	{
		var patterns = ReadPatterns(reader);
		var cache    = new Dictionary<string, bool>();
		return SumFromNonEmptyLines(reader, line => IsPossible(cache, patterns, line) ? 1 : 0).ToString();
	}
}

public sealed class Day19SolutionPart2 : Day19Solution
{
	static long Count(Dictionary<string, long> cache, string[] patterns, ReadOnlySpan<char> design)
	{
		if(design.Length == 0) return 1;

		var lookup = cache.GetAlternateLookup<ReadOnlySpan<char>>();
		if(lookup.TryGetValue(design, out var count)) return count;

		count = 0;
		foreach(var p in patterns)
		{
			if(design.StartsWith(p))
			{
				count += Count(cache, patterns, design[p.Length..]);
			}
		}

		lookup.TryAdd(design, count);
		return count;
	}

	public override string Process(TextReader reader)
	{
		var patterns = ReadPatterns(reader);
		var cache    = new Dictionary<string, long>();
		return SumFromNonEmptyLines(reader, line => Count(cache, patterns, line.AsSpan())).ToString();
	}
}
