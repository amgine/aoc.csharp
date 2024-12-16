namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/16"/></remarks>
[Name(@"Reindeer Maze")]
public abstract class Day16Solution : Solution
{
	protected sealed class Path(Point2D position, Direction2D direction = Direction2D.Right)
	{
		public Point2D Position { get; private set; } = position;

		public Direction2D Direction { get; private set; } = direction;

		public Dictionary<Point2D, long> Scores { get; } = [];

		public long Score { get; private set; }

		public bool Visited(Point2D position)
			=> Scores.ContainsKey(position);

		public void Move(Direction2D direction)
		{
			Position += Vector2D.FromDirection(direction);
			if(Direction == direction)
			{
				Score += 1;
			}
			else
			{
				Direction  = direction;
				Score     += 1001;
			}
			Scores.Add(Position, Score);
		}

		public Path Fork(Direction2D direction)
		{
			var p = new Path(Position, Direction)
			{
				Score = Score,
			};
			foreach(var kvp in Scores)
			{
				p.Scores.Add(kvp.Key, kvp.Value);
			}
			p.Move(direction);
			return p;
		}
	}

	protected static Point2D FindPosition(char[,] map, char c)
	{
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] == c)
				{
					map[y, x] = '.';
					return new(x, y);
				}
			}
		}
		throw new InvalidDataException();
	}

	protected static long GetBestScore(char[,] map, Point2D s, Point2D e)
	{
		var path = new Path(s);
		path.Scores.Add(s, 0);

		var bestScore = long.MaxValue;
		var bestScores = new Dictionary<Point2D, long>();
		var paths = new Queue<Path>();
		paths.Enqueue(path);
		var iterator = new PathIterator(map, paths);
		while(paths.TryDequeue(out var p))
		{
			if(p.Position == e)
			{
				if(p.Score < bestScore) bestScore = p.Score;
				continue;
			}
			if(p.Score >= bestScore || bestScores.TryGetValue(p.Position, out var bs) && bs <= p.Score)
			{
				continue;
			}
			else
			{
				bestScores[p.Position] = p.Score;
			}

			iterator.Next(p);
		}

		return bestScore;
	}

	protected sealed class PathIterator(char[,] map, Queue<Path> paths)
	{
		private readonly List<Direction2D> _candidates = new(capacity: 3);
		private readonly List<Path>        _nextpaths  = new(capacity: 3);

		public void Next(Path p)
		{
			var d0 = p.Direction;
			var d1 = d0.RotateCW();
			var d2 = d0.RotateCCW();

			var p0 = p.Position + Vector2D.FromDirection(d0);
			var p1 = p.Position + Vector2D.FromDirection(d1);
			var p2 = p.Position + Vector2D.FromDirection(d2);

			if(p0.IsInside(map) && p0.GetValue(map) != '#' && !p.Visited(p0))
			{
				_candidates.Add(d0);
			}
			if(p1.IsInside(map) && p1.GetValue(map) != '#' && !p.Visited(p1))
			{
				_candidates.Add(d1);
			}
			if(p2.IsInside(map) && p2.GetValue(map) != '#' && !p.Visited(p2))
			{
				_candidates.Add(d2);
			}

			if(_candidates.Count != 0)
			{
				_nextpaths.Add(p);
				for(int i = 1; i < _candidates.Count; ++i)
				{
					_nextpaths.Add(p.Fork(_candidates[i]));
				}
				p.Move(_candidates[0]);
				foreach(var n in _nextpaths)
				{
					paths.Enqueue(n);
				}
				_nextpaths.Clear();
				_candidates.Clear();
			}
		}
	}
}

public sealed class Day16SolutionPart1 : Day16Solution
{
	public override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		var s   = FindPosition(map, 'S');
		var e   = FindPosition(map, 'E');

		return GetBestScore(map, s, e).ToString();
	}
}

public sealed class Day16SolutionPart2 : Day16Solution
{
	public override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		var s   = FindPosition(map, 'S');
		var e   = FindPosition(map, 'E');

		var path = new Path(s);
		path.Scores.Add(s, 0);

		var bestScore  = GetBestScore(map, s, e);
		var bestScores = new Dictionary<(Point2D, Direction2D), long>();
		var paths      = new Queue<Path>();
		var iterator   = new PathIterator(map, paths);
		var sitPlaces  = new HashSet<Point2D> { s, e };
		paths.Enqueue(path);
		while(paths.TryDequeue(out var p))
		{
			if(p.Position == e)
			{
				if(p.Score == bestScore)
				{
					sitPlaces.UnionWith(p.Scores.Keys);
				}
				continue;
			}
			if(p.Score >= bestScore || bestScores.TryGetValue((p.Position, p.Direction), out var bs) && bs < p.Score)
			{
				continue;
			}
			else
			{
				bestScores[(p.Position, p.Direction)] = p.Score;
			}

			iterator.Next(p);
		}

		return sitPlaces.Count.ToString();
	}
}
