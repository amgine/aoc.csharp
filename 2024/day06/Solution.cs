namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/6"/></remarks>
[Name(@"Guard Gallivant")]
public abstract class Day06Solution : Solution
{
	protected static Point2D FindStartingPosition(char[,] map)
	{
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] is '^')
				{
					map[y, x] = '.';
					return new(x, y);
				}
			}
		}
		throw new InvalidDataException();
	}

	protected static Dictionary<Point2D, Direction2D> Visit(char[,] map, Point2D p)
	{
		var visited = new Dictionary<Point2D, Direction2D>();
		var direction = Direction2D.Up;
		var v = Vector2D.FromDirection(direction);
		visited.Add(p, direction);
		while(true)
		{
			var next = p + v;
			if(!next.IsInside(map)) return visited;
			if(next.GetValue(map) == '#')
			{
				direction = direction.RotateCW();
				v = Vector2D.FromDirection(direction);
				continue;
			}
			p = next;
			visited.TryAdd(p, direction);
		}
	}

	public sealed override string Process(TextReader reader)
	{
		var map   = LoadCharMap2D(reader);
		var start = FindStartingPosition(map);
		return Solve(map, start).ToString();
	}

	protected abstract int Solve(char[,] map, Point2D start);
}

public sealed class Day06SolutionPart1 : Day06Solution
{
	protected override int Solve(char[,] map, Point2D start)
		=> Visit(map, start).Count;
}

public sealed class Day06SolutionPart2 : Day06Solution
{
	static bool IsLoop(char[,] map, Point2D start, Point2D obstacle, Direction2D direction)
	{
		var visited = new HashSet<(Point2D, Direction2D)>();
		var p = start;
		var v = Vector2D.FromDirection(direction);
		while(true)
		{
			var next = p + v;
			if(!next.IsInside(map)) return false;
			if(next.GetValue(map) == '#' || next == obstacle)
			{
				if(!visited.Add((next, direction)))
				{
					return true;
				}
				direction = direction.RotateCW();
				v = Vector2D.FromDirection(direction);
				continue;
			}
			p = next;
		}
	}

	protected override int Solve(char[,] map, Point2D start)
	{
		var count = 0;
		var visits = Visit(map, start);
		visits.Remove(start);
		Parallel.ForEach(visits,
			p =>
			{
				var o = p.Key;
				var d = p.Value;
				var s = o - Vector2D.FromDirection(d);
				if(IsLoop(map, s, o, d))
				{
					Interlocked.Increment(ref count);
				}
			});
		return count;
	}
}
