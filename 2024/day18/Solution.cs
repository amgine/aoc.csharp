namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/18"/></remarks>
[Name(@"RAM Run")]
public abstract class Day18Solution : Solution
{
	protected sealed class Path(Point2D position)
	{
		public static Path CreateInitial(Point2D position)
		{
			var path = new Path(position);
			path.Visited.Add(position);
			return path;
		}

		public HashSet<Point2D> Visited { get; } = [];

		public Point2D Position { get; private set; } = position;

		public void Move(Direction2D d)
		{
			Position += Vector2D.FromDirection(d);
			Visited.Add(Position);
		}

		public Path Fork(Direction2D d)
		{
			var path = new Path(Position);
			path.Visited.UnionWith(Visited);
			path.Move(d);
			return path;
		}
	}

	protected static char[,] LoadMap(List<Point2D> input, int w, int h, int count)
	{
		var map = new char[w, h];
		for(int y = 0; y < map.GetLength(0); y++)
		{
			for(int x = 0; x < map.GetLength(1); x++)
			{
				map[y, x] = '.';
			}
		}
		for(int i = 0; i < count; ++i)
		{
			input[i].GetValue(map) = '#';
		}
		return map;
	}

	protected static char[,] LoadMap(TextReader reader, int w, int h, int count)
	{
		var input = LoadListFromNonEmptyStrings(reader, line => Parsers.ParsePoint2D(line));
		return LoadMap(input, w, h, count);
	}

	protected static readonly Direction2D[] Directions = [ Direction2D.Down, Direction2D.Right, Direction2D.Up, Direction2D.Left ];
}

public sealed class Day18SolutionPart1 : Day18Solution
{
	static bool TryFindScore(char[,] map, out int bestScore)
	{
		var s = new Point2D(0, 0);
		var e = new Point2D(map.GetLength(1) - 1, map.GetLength(0) - 1);

		var q = new Queue<Path>();
		q.Enqueue(Path.CreateInitial(s));
		bestScore = int.MaxValue;
		var found = false;
		var candidates = new List<Direction2D>(capacity: 3);
		var scores = new Dictionary<Point2D, int>();
		while(q.TryDequeue(out var p))
		{
			if(p.Position == e)
			{
				if(p.Visited.Count < bestScore)
				{
					bestScore = p.Visited.Count;
					found = true;
				}
				continue;
			}
			if(p.Visited.Count >= bestScore) continue;
			if(scores.TryGetValue(p.Position, out var score) && score < p.Visited.Count)
			{
				continue;
			}
			else
			{
				scores[p.Position] = score;
			}
			foreach(var d in Directions)
			{
				var n = p.Position + Vector2D.FromDirection(d);
				if(n.IsInside(map) && n.GetValue(map) != '#' && !p.Visited.Contains(n))
				{
					candidates.Add(d);
				}
			}
			if(candidates.Count != 0)
			{
				foreach(var d in candidates)
				{
					q.Enqueue(p.Fork(d));
				}
				candidates.Clear();
			}
		}
		return found;
	}

	public static int Solve(TextReader reader, int w, int h, int count)
	{
		if(!TryFindScore(LoadMap(reader, w, h, count), out var score))
		{
			throw new InvalidDataException();
		}
		return score - 1;
	}


	public override string Process(TextReader reader)
		=> Solve(reader, 71, 71, 1024).ToString();
}

public sealed class Day18SolutionPart2 : Day18Solution
{
	static bool PathExists(char[,] map)
	{
		var s = new Point2D(0, 0);
		var e = new Point2D(map.GetLength(1) - 1, map.GetLength(0) - 1);

		var q = new Queue<Path>();
		q.Enqueue(Path.CreateInitial(s));
		var candidates = new List<Direction2D>(capacity: 3);
		var scores = new Dictionary<Point2D, int>();
		while(q.TryDequeue(out var p))
		{
			if(p.Position == e) return true;
			if(scores.TryGetValue(p.Position, out var score) && score < p.Visited.Count)
			{
				continue;
			}
			else
			{
				scores[p.Position] = score;
			}
			foreach(var d in Directions)
			{
				var n = p.Position + Vector2D.FromDirection(d);
				if(n.IsInside(map) && n.GetValue(map) != '#' && !p.Visited.Contains(n))
				{
					candidates.Add(d);
				}
			}
			if(candidates.Count != 0)
			{
				foreach(var d in candidates)
				{
					q.Enqueue(p.Fork(d));
				}
				candidates.Clear();
			}
		}
		return false;
	}

	public static Point2D Solve(TextReader reader, int w, int h, int count)
	{
		var input = LoadListFromNonEmptyStrings(reader,
			static line => Parsers.ParsePoint2D(line));

		var map = LoadMap(input, w, h, count);
		for(int i = count; i < input.Count; ++i)
		{
			input[i].GetValue(map) = '#';
			if(!PathExists(map)) return input[i];
		}
		throw new InvalidDataException();
	}

	static string PointToString(Point2D p) => $"{p.X},{p.Y}";

	public override string Process(TextReader reader)
		=> PointToString(Solve(reader, 71, 71, 1024));
}
