namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/6"/></remarks>
[Name(@"Guard Gallivant")]
public abstract class Day06Solution : Solution
{
	protected static bool IsInside<T>(T[,] map, Point2D p)
		=> p.X >= 0 && p.Y >= 0 && p.X < map.GetLength(1) && p.Y < map.GetLength(0);

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

	protected static HashSet<Point2D> Visit(char[,] map, Point2D p)
	{
		var visited = new HashSet<Point2D>();
		var direction = Direction2D.Up;
		while(true)
		{
			var v = Vector2D.FromDirection(direction);
			var leftTheMap = true;
			while(p.X >= 0 && p.Y >= 0 && p.X < map.GetLength(1) && p.Y < map.GetLength(0))
			{
				if(map[p.Y, p.X] == '#')
				{
					p -= v;
					direction = direction.RotateCW();
					v = Vector2D.FromDirection(direction);
					leftTheMap = false;
					break;
				}
				visited.Add(p);
				p += v;
			}
			if(leftTheMap)
			{
				break;
			}
		}
		return visited;
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
	static bool CheckLoop(char[,] map, Point2D p, Direction2D d)
	{
		var visited = new Dictionary<Point2D, int>();
		while(true)
		{
			var leftTheMap = true;
			var started = false;
			var v = Vector2D.FromDirection(d);
			while(IsInside(map, p))
			{
				if(map[p.Y, p.X] == '#')
				{
					p -= v;
					d = d.RotateCW();
					leftTheMap = false;
					break;
				}
				if(started)
				{
					if(visited.TryGetValue(p, out var flags))
					{
						var f = flags | (1 << (int)d);
						if(flags == f) return true;
						visited[p] = f;
					}
					else
					{
						visited.Add(p, 1 << (int)d);
					}
				}
				else started = true;
				p += v;
			}
			if(leftTheMap) return false;
		}
	}

	protected override int Solve(char[,] map, Point2D start)
	{
		var count = 0;
		foreach(var p in Visit(map, start))
		{
			if(p == start) continue;
			map[p.Y, p.X] = '#';
			try
			{
				if(CheckLoop(map, start, Direction2D.Up))
				{
					++count;
				}
			}
			finally
			{
				map[p.Y, p.X] = '.';
			}
		}
		return count;
	}
}
