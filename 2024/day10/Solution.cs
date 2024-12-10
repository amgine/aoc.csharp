namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/10"/></remarks>
[Name(@"Hoof It")]
public abstract class Day10Solution : Solution
{
	protected static List<Point2D> FindStarts(int[,] map)
		=> FindPositions(map, static v => v == 0);

	protected abstract int GetScore(int[,] map, Point2D start);

	public sealed override string Process(TextReader reader)
	{
		var map = LoadDigitMap2D(reader);
		var sum = 0;
		foreach(var start in FindStarts(map))
		{
			sum += GetScore(map, start);
		}
		return sum.ToString();
	}
}

public sealed class Day10SolutionPart1 : Day10Solution
{
	protected override int GetScore(int[,] map, Point2D start)
	{
		var visited = new HashSet<Point2D>();
		var next    = new Queue<Point2D>();
		var score   = 0;
		next.Enqueue(start);
		while(next.TryDequeue(out var p))
		{
			if(!visited.Add(p)) continue;

			var v = p.GetValue(map);
			if(v == 9)
			{
				++score;
				continue;
			}

			++v;

			static void CheckNext(int[,] map, Point2D p, HashSet<Point2D> visited, Queue<Point2D> next, int v)
			{
				if(!p.IsInside(map))     return;
				if(p.GetValue(map) != v) return;
				if(visited.Contains(p))  return;
				next.Enqueue(p);
			}

			CheckNext(map, p + Vector2D.Up,    visited, next, v);
			CheckNext(map, p + Vector2D.Down,  visited, next, v);
			CheckNext(map, p + Vector2D.Right, visited, next, v);
			CheckNext(map, p + Vector2D.Left,  visited, next, v);
		}

		return score;
	}
}

public sealed class Day10SolutionPart2 : Day10Solution
{
	static int Next(int[,] map, Point2D p)
	{
		var v = p.GetValue(map);
		if(v == 9) return 1;
		++v;

		static int CheckNext(int[,] map, Point2D p, int v)
			=> p.IsInside(map) && p.GetValue(map) == v ? Next(map, p) : 0;

		return
			CheckNext(map, p + Vector2D.Up,    v) +
			CheckNext(map, p + Vector2D.Down,  v) +
			CheckNext(map, p + Vector2D.Right, v) +
			CheckNext(map, p + Vector2D.Left,  v);
	}

	protected override int GetScore(int[,] map, Point2D start)
		=> Next(map, start);
}
