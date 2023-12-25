namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/5"/></remarks>
[Name("If You Give A Seed A Fertilizer")]
public abstract class Day5Solution : Solution
{
	protected readonly record struct Range(long Start, long Length);

	protected readonly record struct RangeMap(long DstStart, long SrcStart, long Length)
	{
		public bool Affects(Range range)
		{
			if(range.Start + range.Length <= SrcStart) return false;
			if(SrcStart + Length <= range.Start) return false;
			return true;
		}

		public static RangeMap Parse(string line)
		{
			Span<System.Range> partRanges = stackalloc System.Range[3];
			if(line.AsSpan().Split(partRanges, ' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) != 3)
			{
				throw new InvalidDataException();
			}
			return new RangeMap(
				long.Parse(line.AsSpan(partRanges[0])),
				long.Parse(line.AsSpan(partRanges[1])),
				long.Parse(line.AsSpan(partRanges[2])));
		}
	}

	protected static List<RangeMap>[] ParseMaps(TextReader reader)
	{
		var maps = new List<List<RangeMap>>();
		var currentMap = default(List<RangeMap>);

		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;

			if(line.EndsWith(" map:"))
			{
				maps.Add(currentMap = []);
				continue;
			}

			if(currentMap is null) throw new InvalidDataException();

			currentMap.Add(RangeMap.Parse(line));
		}

		return [.. maps];
	}
}

public sealed class Day5SolutionPart1 : Day5Solution
{
	private static long[] ParseSeedsToPlant(TextReader reader)
	{
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line)) continue;

			const string prefix = "seeds: ";

			if(line.StartsWith(prefix))
			{
				return line[prefix.Length..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
			}
		}
		throw new InvalidDataException();
	}

	static long MapValue(long value, List<RangeMap>[] maps)
	{
		static long Get(List<RangeMap> map, long id)
		{
			foreach(var range in map)
			{
				if(id >= range.SrcStart && id < range.SrcStart + range.Length)
				{
					return id - range.SrcStart + range.DstStart;
				}
			}
			return id;
		}

		foreach(var map in maps)
		{
			value = Get(map, value);
		}
		return value;
	}

	public override string Process(TextReader reader)
	{
		var seedsToPlant = ParseSeedsToPlant(reader);
		var maps = ParseMaps(reader);

		var min = long.MaxValue;

		foreach(var seed in seedsToPlant)
		{
			var id = MapValue(seed, maps);
			if(id < min) min = id;
		}

		return min.ToString();
	}
}

public sealed class Day5SolutionPart2 : Day5Solution
{
	private static Range[] ParseSeedsToPlant(TextReader reader)
	{
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line)) continue;

			if(line.StartsWith("seeds: "))
			{
				var values = line.Substring("seeds: ".Length).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
				var rangesToPlant = new Range[values.Length / 2];
				for(int i = 0; i < rangesToPlant.Length; ++i)
				{
					rangesToPlant[i] = new(values[i * 2], values[i * 2 + 1]);
				}
				return rangesToPlant;
			}
		}
		throw new InvalidDataException();
	}

	static void RemapRanges(List<Range> ranges, List<RangeMap> map)
	{
		for(int i = 0; i < ranges.Count; ++i)
		{
			var r = ranges[i];
			foreach(var entry in map)
			{
				if(!entry.Affects(r)) continue;

				if(r.Start >= entry.SrcStart && (r.Start + r.Length) <= (entry.SrcStart + entry.Length))
				{
					// entry ---[===========]---
					// r     -----[======]------
					// remap the whole range
					ranges[i] = new(r.Start - entry.SrcStart + entry.DstStart, r.Length);
					break;
				}
				if(r.Start < entry.SrcStart)
				{
					if(r.Start + r.Length > entry.SrcStart + entry.Length)
					{
						// entry -------[===]-------
						// r     ---[111|222|333]---
						// split in 3, middle is remapped
						var r1 = new Range(r.Start, entry.SrcStart - r.Start);
						var r2 = new Range(entry.DstStart, entry.Length);
						var r3 = new Range(entry.SrcStart + entry.Length, r.Start + r.Length - (entry.SrcStart + entry.Length));
						ranges[i] = r2;
						ranges.Add(r1);
						ranges.Add(r3);
					}
					else
					{
						// entry -------[====]---
						// r     ---[111|222]----
						// split in 2, right is remapped
						var r1 = new Range(r.Start, entry.SrcStart - r.Start);
						var r2 = new Range(entry.DstStart, r.Length - r1.Length);
						ranges[i] = r2;
						ranges.Add(r1);
					}
				}
				else
				{
					// entry ---[====]-------
					// r     ----[111|222]---
					// split in 2, left is remapped
					var intersectionLength = entry.Length - (r.Start - entry.SrcStart);
					var r1 = new Range(entry.DstStart + (r.Start - entry.SrcStart), intersectionLength);
					var r2 = new Range(r.Start + intersectionLength, r.Length - r1.Length);
					ranges[i] = r1;
					ranges.Add(r2);
				}
				break;
			}
		}
	}

	static List<Range> MapRange(in Range range, List<RangeMap>[] maps)
	{
		var ranges = new List<Range>() { range };
		foreach(var map in maps)
		{
			RemapRanges(ranges, map);
		}
		return ranges;
	}

	public override string Process(TextReader reader)
	{
		var seedsToPlant = ParseSeedsToPlant(reader);
		var maps         = ParseMaps(reader);

		var min = long.MaxValue;
		foreach(var range in seedsToPlant)
		{
			foreach(var mappedRange in MapRange(range, maps))
			{
				if(mappedRange.Start < min) min = mappedRange.Start;
			}
		}

		return min.ToString();
	}
}
