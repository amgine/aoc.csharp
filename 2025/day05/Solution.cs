using System.Globalization;

namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/5"/></remarks>
[Name(@"Cafeteria")]
public abstract class Day05Solution : Solution
{
	protected readonly record struct Range(long Min, long Max)
	{
		public static Range Parse(ReadOnlySpan<char> line)
		{
			var sep = line.IndexOf('-');
			if(sep <= 0) throw new InvalidDataException();
			var min = long.Parse(line[..sep]);
			var max = long.Parse(line[(sep + 1)..]);
			return new(min, max);
		}

		public bool Contains(Range range)
			=> Min <= range.Min
			&& Max >= range.Max;

		public static bool TryMerge(Range r1, Range r2, out Range merged)
		{
			if(r1.Contains(r2))
			{
				merged = r1;
				return true;
			}
			if(r2.Contains(r1))
			{
				merged = r2;
				return true;
			}
			if(r1.Max == r2.Min - 1)
			{
				merged = new(r1.Min, r2.Max);
				return true;
			}
			if(r2.Max == r1.Min - 1)
			{
				merged = new(r2.Min, r1.Max);
				return true;
			}
			if(r1.Min > r2.Max || r2.Min > r1.Max)
			{
				merged = default;
				return false;
			}
			merged = new(Math.Min(r1.Min, r2.Min), Math.Max(r1.Max, r2.Max));
			return true;
		}

		public long Length => Max - Min + 1;

		public bool Contains(long id)
			=> id >= Min
			&& id <= Max;
	}
}

public sealed class Day05SolutionPart1 : Day05Solution
{
	public override string Process(TextReader reader)
	{
		bool readingRanges = true;

		var ranges = new List<Range>();
		var ids = new List<long>();

		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(line))
			{
				readingRanges = false;
				continue;
			}
			if(readingRanges)
			{
				ranges.Add(Range.Parse(line));
			}
			else
			{
				ids.Add(long.Parse(line, NumberStyles.Integer, CultureInfo.InvariantCulture));
			}
		}

		var fresh = 0;
		foreach(var id in ids)
		{
			if(ranges.Any(r => r.Contains(id))) ++fresh;
		}
		return fresh.ToString();
	}
}

public sealed class Day05SolutionPart2 : Day05Solution
{
	public override string Process(TextReader reader)
	{
		var ranges = new List<Range>();

		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(line))
			{
				break;
			}
			ranges.Add(Range.Parse(line));
		}

		bool done;
		do
		{
			done = true;
			for(int i = 0; i < ranges.Count - 1; ++i)
			{
				for(int j = ranges.Count - 1; j > i; --j)
				{
					if(Range.TryMerge(ranges[i], ranges[j], out var merged))
					{
						ranges.RemoveAt(j);
						ranges[i] = merged;
						done = false;
					}
				}
			}
		}
		while(!done);

		return ranges.Sum(static r => r.Length).ToString();
	}
}
