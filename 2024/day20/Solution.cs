namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/20"/></remarks>
[Name(@"Race Condition")]
public abstract class Day20Solution : Solution
{
	protected static long CountCheats(Path path, int offset, int maxLength, int threshold)
	{
		var p   = path.Track[offset];
		var sum = 0L;
		for(int dx = -maxLength; dx <= maxLength; ++dx)
		{
			var adx  =  Math.Abs(dx);
			var minY = -maxLength + adx;
			var maxY =  maxLength - adx;
			for(int dy = minY; dy <= maxY; ++dy)
			{
				var len = adx + Math.Abs(dy);
				if(len < 2) continue;
				var dst = new Point2D(p.X + dx, p.Y + dy);
				if(!path.Visited.TryGetValue(dst, out var idx)) continue;
				if(idx > offset)
				{
					var save = idx - offset - len;
					if(save >= threshold) ++sum;
				}
			}
		}

		return sum;
	}

	protected static long CountCheats(Path path, int maxLength, int threshold)
	{
		var sum = 0L;
		for(int i = 0; i < path.Track.Count; ++i)
		{
			sum += CountCheats(path, i, maxLength, threshold);
		}
		return sum;
	}

	static readonly Direction2D[] Directions =
		[
			Direction2D.Left,
			Direction2D.Up,
			Direction2D.Right,
			Direction2D.Down
		];

	protected sealed class Path(Point2D posiiton)
	{
		public static Path CreateInitial(Point2D position)
		{
			var path = new Path(position);
			path.Visited.Add(position, 0);
			path.Track.Add(position);
			return path;
		}

		public Point2D Position { get; private set; } = posiiton;

		public Dictionary<Point2D, int> Visited { get; } = [];

		public List<Point2D> Track { get; } = [];

		public void Move(Point2D position)
		{
			Position = position;
			Visited.Add(position, Track.Count);
			Track.Add(position);
		}

		public Path Fork(Point2D position)
		{
			var p = new Path(Position);
			foreach(var kvp in Visited)
			{
				p.Visited.Add(kvp.Key, kvp.Value);
			}
			p.Track.AddRange(Track);
			p.Move(position);
			return p;
		}
	}

	protected abstract long CountCheats(Path path);

	private static Path FindPath(char[,] map)
	{
		var s = FindPosition(map, 'S');
		var e = FindPosition(map, 'E');

		var q = new Queue<Path>();
		var candidates = new List<Point2D>(capacity: 4);
		q.Enqueue(Path.CreateInitial(s));
		var bestPath = default(Path);
		var best = int.MaxValue;
		var scores = new Dictionary<Point2D, int>();
		while(q.TryDequeue(out var p))
		{
			if(p.Position == e)
			{
				if(p.Track.Count < best)
				{
					bestPath = p;
					best = p.Track.Count;
				}
				continue;
			}
			if(p.Track.Count >= best) continue;
			if(scores.TryGetValue(p.Position, out var score))
			{
				if(score < p.Track.Count) continue;
			}
			else
			{
				scores.Add(p.Position, score);
			}
			foreach(var d in Directions)
			{
				var n = p.Position + Vector2D.FromDirection(d);
				if(n.GetValue(map) != '#' && !p.Visited.ContainsKey(n))
				{
					candidates.Add(n);
				}
			}
			if(candidates.Count > 0)
			{
				if(candidates.Count == 1)
				{
					p.Move(candidates[0]);
					q.Enqueue(p);
				}
				else
				{
					foreach(var c in candidates)
					{
						q.Enqueue(p.Fork(c));
					}
				}
				candidates.Clear();
			}
		}
		return bestPath ?? throw new InvalidDataException("No valid path");
	}

	public static long CountCheats(TextReader reader, int maxLength, int threshold)
	{
		var map  = LoadCharMap2D(reader);
		var path = FindPath(map);
		return CountCheats(path, maxLength, threshold);
	}

	public sealed override string Process(TextReader reader)
	{
		var map  = LoadCharMap2D(reader);
		var path = FindPath(map);
		return CountCheats(path).ToString();
	}
}

public sealed class Day20SolutionPart1 : Day20Solution
{
	protected override long CountCheats(Path path)
		=> CountCheats(path, maxLength: 2, threshold: 100);
}

public sealed class Day20SolutionPart2 : Day20Solution
{
	protected override long CountCheats(Path path)
		=> CountCheats(path, maxLength: 20, threshold: 100);
}
