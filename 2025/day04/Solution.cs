namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/4"/></remarks>
[Name(@"Printing Department")]
public abstract class Day04Solution : Solution
{
	private static Vector2D[] _offsets =
		[
			new(-1, -1),
			new( 0, -1),
			new(+1, -1),
			new(-1,  0),
			new(+1,  0),
			new(-1, +1),
			new( 0, +1),
			new(+1, +1),
		];

	protected static int CountAdjacent(char[,] map, Point2D location)
	{
		var count = 0;
		foreach(var offset in _offsets)
		{
			var p = location + offset;
			if(!p.IsInside(map)) continue;
			if(p.GetValue(map) == '@') ++count;
		}
		return count;
	}

	protected static bool CanRemove(char[,] map, Point2D p)
		=> p.GetValue(map) == '@' && CountAdjacent(map, p) < 4;
}

public sealed class Day04SolutionPart1 : Day04Solution
{
	public override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		var h = map.GetLength(0);
		var w = map.GetLength(1);
		var count = 0;
		for(int y = 0; y < h; ++y)
		{
			for(int x = 0; x < w; ++x)
			{
				var p = new Point2D(x, y);
				if(CanRemove(map, p))
				{
					++count;
				}
			}
		}
		return count.ToString();
	}
}

public sealed class Day04SolutionPart2 : Day04Solution
{
	public override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		var h = map.GetLength(0);
		var w = map.GetLength(1);
		var removed = new List<Point2D>();
		var count = 0;
		while(true)
		{
			for(int y = 0; y < h; ++y)
			{
				for(int x = 0; x < w; ++x)
				{
					var p = new Point2D(x, y);
					if(CanRemove(map, p))
					{
						removed.Add(p);
					}
				}
			}
			if(removed.Count == 0) break;
			foreach(var r in removed)
			{
				r.GetValue(map) = 'x';
			}
			count += removed.Count;
			removed.Clear();
		}
		return count.ToString();
	}
}
