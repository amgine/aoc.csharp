using System.Diagnostics.CodeAnalysis;

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
	/// <summary>Sets all 0 flag values to <paramref name="flag"/> value.</summary>
	/// <param name="queue">Quque for other neighbors that require check/update.</param>
	/// <param name="lookup">Cell flags lookup.</param>
	/// <param name="p">Cell coordinates.</param>
	/// <param name="flag">Flag value (1 or 2).</param>
	/// <returns><see langword="true"/>, if cell neighbor flags were updated, <see langword="false"/> otherwise.</returns>
	static bool TrySetFlag(ref Queue<Point2D>? queue, Dictionary<Point2D, int> lookup, Point2D p, int flag)
	{
		for(int x = p.X - 1, ex = p.X + 1; x <= ex; ++x)
		{
			for(int y = p.Y - 1, ey = p.Y + 1; y <= ey; ++y)
			{
				if(x == p.X && y == p.Y) continue;
				var np = new Point2D(x, y);
				if(lookup.TryGetValue(np, out var value))
				{
					// 0 can be changed
					if(value == 0)
					{
						// neighbors of former 0 also should be looked at
						(queue ??= new()).Enqueue(np);
						lookup[np] = flag;
						continue;
					}
					// cannot change 2-> 1 or 1->2
					if(value != flag) return false;
				}
			}
		}
		return true;
	}

	/// <summary>Try to determine and set cell flag value.</summary>
	/// <param name="lookup">Cell flag values.</param>
	/// <param name="p">PCell coordinates.</param>
	/// <param name="w">Map width.</param>
	/// <param name="h">Map height.</param>
	/// <returns>
	/// <see langword="true"/>, if cell flag was set, <see langword="false"/> otherwise.<br/>
	/// If cell flag cannot be set, <paramref name="p"/> is the one that breaks the path.
	/// </returns>
	static bool TrySetFlag(Dictionary<Point2D, int> lookup, Point2D p, int w, int h)
	{
		if(lookup.ContainsKey(p)) return true;

		var leftEdge   = p.X == 0;
		var topEdge    = p.Y == 0;
		var rightEdge  = p.X == w - 1;
		var bottomEdge = p.Y == h - 1;

		if(leftEdge   && topEdge)   return false; // top left is start - cannot put obstacles there
		if(bottomEdge && rightEdge) return false; // bottom right is finish - cannot put obstacles there

		var flag = 0;
		if(leftEdge  || bottomEdge) flag = 1; // paint everything left-bottom with 1
		if(rightEdge || topEdge)    flag = 2; // paint everything top-right with 2

		if(flag == 0)
		{
			// try to find flag value to paint with using cell neighbors
			var found1 = false;
			var found2 = false;
			for(int x = p.X - 1, ex = p.X + 1; x <= ex; ++x)
			{
				for(int y = p.Y - 1, ey = p.Y + 1; y <= ey; ++y)
				{
					if(x == p.X && y == p.Y) continue;
					if(!lookup.TryGetValue(new(x, y), out var value)) continue;

					switch(value)
					{
						case 1: found1 = true; break;
						case 2: found2 = true; break;
					}
				}
			}
			// if have neighbors with different flags then p will cut the path - it is the answer
			if(found1 && found2) return false;
			// if have neighbors with single flag then use the same flag
			if(found1) flag = 1;
			if(found2) flag = 2;
		}

		lookup.Add(p, flag);

		// not near borders and no flagged neighbors - leave it as undetermined
		if(flag == 0) return true;

		var q = default(Queue<Point2D>);
		// re-flag neighbors with 0 flag
		// can only change 0->1 and 0->2, if 1-> 2 or 2->1 change is detected - p is the answer
		if(!TrySetFlag(ref q, lookup, p, flag)) return false;
		while(q is not null && q.TryDequeue(out var np))
		{
			// chained search
			if(!TrySetFlag(ref q, lookup, np, flag)) return false;
		}

		// all flags were updated successfully
		return true;
	}

	public static Point2D Solve(TextReader reader, int w, int h, int count)
	{
		var lookup = new Dictionary<Point2D, int>(capacity: w * h);
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var p = Parsers.ParsePoint2D(line);
			if(!TrySetFlag(lookup, p, w, h)) return p;
		}
		throw new InvalidDataException("No path-breaking cells were discovered in the input.");
	}

	static string PointToString(Point2D p) => $"{p.X},{p.Y}";

	public override string Process(TextReader reader)
		=> PointToString(Solve(reader, 71, 71, 1024));
}
