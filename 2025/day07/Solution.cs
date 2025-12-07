namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/7"/></remarks>
[Name(@"Laboratories")]
public abstract class Day07Solution : Solution
{
	protected static int FindStartX(char[,] map)
	{
		for(int x = 0, w = map.GetLength(1); x < w; ++x)
		{
			if(map[0, x] == 'S') return x;
		}
		throw new InvalidDataException();
	}
}

public sealed class Day07SolutionPart1 : Day07Solution
{
	public override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		var w = map.GetLength(1);
		var h = map.GetLength(0);

		map[1, FindStartX(map)] = '|';

		var splits = 0;
		for(int y = 1; y < h - 1; ++y)
		{
			for(int x = 0; x < w; ++x)
			{
				if(map[y, x] == '|')
				{
					switch(map[y + 1, x])
					{
						case '^':
							map[y + 1, x - 1] = '|';
							map[y + 1, x + 1] = '|';
							++splits;
							break;
						case '.':
							map[y + 1, x] = '|';
							break;
					}
				}
			}
		}
		return splits.ToString();
	}
}

public sealed class Day07SolutionPart2 : Day07Solution
{
	static long GetTimelines(char[,] map, Point2D pos, Dictionary<Point2D, long> cache)
	{
		if(cache.TryGetValue(pos, out var count)) return count;

		var h = map.GetLength(0);
		for(int y = pos.Y; y < h; ++y)
		{
			var p = pos with { Y = y };
			if(p.GetValue(map) == '^')
			{
				var timelines =
					GetTimelines(map, new(pos.X - 1, y + 1), cache) +
					GetTimelines(map, new(pos.X + 1, y + 1), cache);
				cache.Add(pos, timelines);
				return timelines;
			}
		}
		return 1;
	}

	public override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		return GetTimelines(map, new(FindStartX(map), 1), []).ToString();
	}
}
