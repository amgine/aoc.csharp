namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/16"/></remarks>
[Name(@"Reindeer Maze")]
public abstract class Day16Solution : Solution
{
	protected sealed class Path(Point2D position, Direction2D direction = Direction2D.Right)
	{
		public static Path CreateInitial(Point2D position)
		{
			var path = new Path(position);
			path.Nodes.Add(position);
			return path;
		}

		public Point2D Position { get; private set; } = position;

		public Direction2D Direction { get; private set; } = direction;

		public HashSet<Point2D> Nodes { get; } = [];

		public long Score { get; private set; }

		public bool Visited(Point2D position)
			=> Nodes.Contains(position);

		public void Move(Direction2D direction)
		{
			const int MoveCost = 1;
			const int TurnCost = 1000;

			Position += Vector2D.FromDirection(direction);
			if(Direction == direction)
			{
				Score += MoveCost;
			}
			else
			{
				Direction  = direction;
				Score     += MoveCost + TurnCost;
			}
			Nodes.Add(Position);
		}

		public Path Fork(Direction2D direction)
		{
			var p = new Path(Position, Direction)
			{
				Score = Score,
			};
			p.Nodes.UnionWith(Nodes);
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
		var bestScore  = long.MaxValue;
		var bestScores = new Dictionary<Point2D, long>(capacity: map.Length);
		var paths      = new Queue<Path>();
		var iterator   = new PathIterator(map, paths);
		paths.Enqueue(Path.CreateInitial(s));
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

		private bool CanVisit(Path p, Point2D position)
			=> position.IsInside(map)
			&& position.GetValue(map) != '#'
			&& !p.Visited(position);

		private void GetNextDirections(Path p)
		{
			var d0 = p.Direction;
			var d1 = d0.RotateCW();
			var d2 = d0.RotateCCW();

			var p0 = p.Position + Vector2D.FromDirection(d0);
			var p1 = p.Position + Vector2D.FromDirection(d1);
			var p2 = p.Position + Vector2D.FromDirection(d2);

			_candidates.Clear();
			if(CanVisit(p, p0)) _candidates.Add(d0);
			if(CanVisit(p, p1)) _candidates.Add(d1);
			if(CanVisit(p, p2)) _candidates.Add(d2);
		}

		private void ScheduleMoves(Path p)
		{
			if(_candidates.Count == 0) return;
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
		}

		public void Next(Path p)
		{
			GetNextDirections(p);
			ScheduleMoves(p);
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

		var bestScore  = GetBestScore(map, s, e);
		var bestScores = new Dictionary<(Point2D, Direction2D), long>();
		var paths      = new Queue<Path>();
		var iterator   = new PathIterator(map, paths);
		var sitPlaces  = new HashSet<Point2D> { s, e };
		paths.Enqueue(Path.CreateInitial(s));
		while(paths.TryDequeue(out var p))
		{
			if(p.Position == e)
			{
				if(p.Score == bestScore)
				{
					sitPlaces.UnionWith(p.Nodes);
				}
				continue;
			}
			if(p.Score >= bestScore) continue;
			if(bestScores.TryGetValue((p.Position, p.Direction), out var bs) && bs < p.Score)
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
