namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/12"/></remarks>
[Name(@"Garden Groups")]
public abstract class Day12Solution : Solution
{
	protected static readonly Vector2D[] Directions =
		[
			Vector2D.Left,
			Vector2D.Up,
			Vector2D.Right,
			Vector2D.Down,
		];

	protected abstract long GetPrice(char[,] map, HashSet<Point2D> visited, Point2D p);

	public sealed override string Process(TextReader reader)
	{
		var visited = new HashSet<Point2D>();
		var map = LoadCharMap2D(reader);
		var sum = 0L;
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				var p = new Point2D(x, y);
				if(!visited.Add(p)) continue;
				sum += GetPrice(map, visited, p);
			}
		}
		return sum.ToString();
	}
}

public sealed class Day12SolutionPart1 : Day12Solution
{
	protected override long GetPrice(char[,] map, HashSet<Point2D> visited, Point2D start)
	{
		var queue = new Queue<Point2D>();
		queue.Enqueue(start);
		var c = start.GetValue(map);
		var perimeter = 0;
		var area = 0;
		while(queue.TryDequeue(out var p))
		{
			void CheckNext(Point2D n)
			{
				if(!n.IsInside(map) || n.GetValue(map) != c)
				{
					++perimeter;
					return;
				}
				if(visited.Add(n)) queue.Enqueue(n);
			}

			foreach(var d in Directions)
			{
				CheckNext(p + d);
			}

			++area;
		}
		return perimeter * area;
	}
}

public sealed class Day12SolutionPart2 : Day12Solution
{
	static int CountSides(HashSet<(Point2D, int)> perimeter)
	{
		var sides = 0;
		while(perimeter.TryGetAny(out var pair))
		{
			perimeter.Remove(pair);
			var (p, d) = pair;
			foreach(var dir in Directions)
			{
				var n = p + dir;
				if(perimeter.Remove((n, d)))
				{
					var x = n + dir;
					while(perimeter.Remove((x, d))) x += dir;
					x = p - dir;
					while(perimeter.Remove((x, d))) x -= dir;
					break;
				}
			}
			++sides;
		}
		return sides;
	}

	protected override long GetPrice(char[,] map, HashSet<Point2D> visited, Point2D start)
	{
		var c = start.GetValue(map);
		var perimeter = new HashSet<(Point2D, int)>();
		var area = 0;

		var queue = new Queue<Point2D>();
		queue.Enqueue(start);
		while(queue.TryDequeue(out var p))
		{
			void CheckNext(int d, Point2D p)
			{
				var n = p + Directions[d];
				if(!n.IsInside(map) || n.GetValue(map) != c)
				{
					perimeter.Add((n, d));
					return;
				}
				if(visited.Add(n)) queue.Enqueue(n);
			}

			for(int i = 0; i < Directions.Length; ++i)
			{
				CheckNext(i, p);
			}
			++area;
		}

		return CountSides(perimeter) * area;
	}
}
