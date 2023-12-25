namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/12"/></remarks>
[Name(@"Hot Springs")]
public abstract class Day12Solution : Solution
{
	static bool CanPlaceGroupAt(int offset, int groupLength, ReadOnlySpan<char> map)
	{
		if(offset > 0 && map[offset - 1] == '#')
		{
			// adjacent group on the left side - only ? or . allowed
			return false;
		}
		if(map.Slice(offset, groupLength).Contains('.'))
		{
			// separator inside -  only ? or # allowed
			return false;
		}
		if(offset + groupLength < map.Length && map[offset + groupLength] == '#')
		{
			// adjacent group on the right side - only ? or . allowed
			return false;
		}
		return true;
	}

	readonly record struct CacheKey(int GroupIndex, int Offset);

	/// <summary>
	/// Counts possible arrangements starting at the specified <paramref name="offset"/>
	/// with the specified <paramref name="group"/>.
	/// </summary>
	static long CountGroupArrangements(ReadOnlySpan<char> map, int[] groups,
		int offset, int group, Dictionary<CacheKey, long> cache)
	{
		if(cache.TryGetValue(new(group, offset), out var cached)) return cached;

		static int GetMaxGroupOffset(ReadOnlySpan<char> map, int[] groups, int group)
		{
			// need to keep at least minimal space for the remaining groups
			var max = map.Length + 1;
			for(int i = groups.Length - 1; i >= group; --i)
			{
				max -= groups[i] + 1;
			}
			return max;
		}

		static bool CanBeLastGroup(int i, int groupLength, ReadOnlySpan<char> map)
		{
			var r = i + groupLength + 1;
			if(r < map.Length)
			{
				if(map[r..].Contains('#'))
				{
					return false;
				}
			}
			return true;
		}

		var maxOffset   = GetMaxGroupOffset(map, groups, group);
		var groupLength = groups[group];
		var sum = 0L;
		for(int i = offset; i <= maxOffset; ++i)
		{
			if(!CanPlaceGroupAt(i, groupLength, map))
			{
				// cannot put the group at i, keep moving
				continue;
			}

			if(map[offset..i].Contains('#'))
			{
				// specified group must be the first one
				// no point in moving forward
				break;
			}

			if(group == groups.Length - 1)
			{
				// last group in the list has to be placed last to count
				// if map has '#' after group placement, just keep moving
				if(CanBeLastGroup(i, groupLength, map))
				{
					++sum;
				}
				continue;
			}

			// try to place the next group after the current one
			var nextOffset = i + groupLength + 1;
			sum += CountGroupArrangements(map, groups, nextOffset, group + 1, cache);
		}

		cache.Add(new(group, offset), sum);
		return sum;
	}

	protected static long RecursiveCountArrangements(ReadOnlySpan<char> map, int[] groups)
	{
		var cache = new Dictionary<CacheKey, long>(capacity: map.Length * groups.Length);
		return CountGroupArrangements(map, groups, 0, 0, cache);
	}

	protected static (string map, int[] groups) ParseInputLine(string line)
	{
		var separator = line.IndexOf(' ');
		if(separator < 0 || separator >= line.Length - 1)
			throw new InvalidDataException();
		var map    = line[..separator];
		var groups = Array.ConvertAll(line[(separator + 1)..].Split(','), int.Parse);
		return (map, groups);
	}

	protected sealed record class Input(string Map, int[] Groups);

	protected abstract Input ParseInput(string line);

	public override string Process(TextReader reader)
	{
		var sum    = 0L;
		var inputs = LoadListFromNonEmptyStrings(reader, ParseInput);

		if(inputs.Count == 1)
		{
			sum = RecursiveCountArrangements(inputs[0].Map, inputs[0].Groups);
		}
		else
		{
			Parallel.ForEach(inputs, input =>
			{
				var res = RecursiveCountArrangements(input.Map, input.Groups);
				Interlocked.Add(ref sum, res);
			});
		}

		return sum.ToString();
	}
}

public sealed class Day12SolutionPart1 : Day12Solution
{
	protected override Input ParseInput(string line)
	{
		var (map, groups) = ParseInputLine(line);
		return new(map, groups);
	}
}

public sealed class Day12SolutionPart2 : Day12Solution
{
	const int UnfoldCount = 5;

	private static string UnfoldMap(string map, int count)
	{
		const string Separator = @"?";

		var segmentLength = map.Length + Separator.Length;
		Span<char> unfolded = stackalloc char[segmentLength * count - Separator.Length];
		map.CopyTo(unfolded);
		for(int i = 1; i < count; ++i)
		{
			Separator.CopyTo(unfolded[(segmentLength * i - Separator.Length)..]);
			map.CopyTo(unfolded[(segmentLength * i)..]);
		}
		return new(unfolded);
	}

	private static int[] UnfoldGroups(ReadOnlySpan<int> groups, int count)
	{
		var unfolded = new int[groups.Length * count];
		for(int i = 0; i < count; ++i)
		{
			groups.CopyTo(unfolded.AsSpan(i * groups.Length));
		}
		return unfolded;
	}

	protected override Input ParseInput(string line)
	{
		var (map, groups) = ParseInputLine(line);
		map    = UnfoldMap   (map,    UnfoldCount);
		groups = UnfoldGroups(groups, UnfoldCount);
		return new(map, groups);
	}
}
