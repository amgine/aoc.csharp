namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/21"/></remarks>
[Name(@"Step Counter")]
public abstract class Day21Solution : Solution
{
	protected class Visitor(char[,] map)
	{
		sealed class State
		{
			public HashSet<Point2D> Visited { get; } = [];

			public List<Point2D> VisitedRecently { get; } = [];
		}

		static State GetInitialState(char[,] map)
		{
			var state = new State();
			var start = FindStart(map);
			state.Visited.Add(start);
			state.VisitedRecently.Add(start);
			return state;
		}

		protected readonly char[,] _map  = map;
		protected readonly int _width    = map.GetLength(1);
		protected readonly int _height   = map.GetLength(0);
		private readonly State[] _states = [GetInitialState(map), new State()];

		private int _steps;

		protected virtual bool CanVisit(Point2D pos)
			=> pos.X >= 0 && pos.Y >= 0 && pos.X < _width && pos.Y < _height
			&& pos.GetValue(_map) is '.' or 'S';

		private static void GetNextPositions(Point2D position,
			Span<Point2D> positions)
		{
			positions[0] = new Point2D(position.X + 1, position.Y);
			positions[1] = new Point2D(position.X - 1, position.Y);
			positions[2] = new Point2D(position.X, position.Y + 1);
			positions[3] = new Point2D(position.X, position.Y - 1);
		}

		public int Steps => _steps;

		private void GetStates(out State prev, out State current)
		{
			prev    = _states[   _steps  & 1];
			current = _states[(++_steps) & 1];
		}

		public int NextStep()
		{
			GetStates(out var prev, out var current);

			Span<Point2D> positions = stackalloc Point2D[4];
			foreach(var p in prev.VisitedRecently)
			{
				GetNextPositions(p, positions);
				foreach(var pos in positions)
				{
					if(CanVisit(pos) && current.Visited.Add(pos))
					{
						current.VisitedRecently.Add(pos);
					}
				}
			}
			prev.VisitedRecently.Clear();
			return current.Visited.Count;
		}
	}

	protected static Point2D FindStart(char[,] map)
	{
		var width = map.GetLength(0);
		for(int y = 0, height = map.GetLength(1); y < height; ++y)
		{
			for(int x = 0; x < width; ++x)
			{
				if(map[y, x] == 'S') return new(y, x);
			}
		}
		throw new InvalidDataException();
	}
}

public sealed class Day21SolutionPart1 : Day21Solution
{
	public static int Process(TextReader reader, int targetSteps)
	{
		var map     = LoadCharMap2D(reader);
		var visitor = new Visitor(map);
		int count;
		do { count = visitor.NextStep(); }
		while(visitor.Steps < targetSteps);
		return count;
	}

	public override string Process(TextReader reader)
	{
		const int TargetSteps = 64;

		return Process(reader, TargetSteps).ToString();
	}
}

public sealed class Day21SolutionPart2 : Day21Solution
{
	sealed class WrappingVisitor(char[,] map) : Visitor(map)
	{
		private Point2D Wrap(Point2D pos)
		{
			var x = pos.X % _width;
			var y = pos.Y % _height;

			if(x < 0) x += _width;
			if(y < 0) y += _height;

			return new(x, y);
		}

		protected override bool CanVisit(Point2D pos)
			=> Wrap(pos).GetValue(_map) is '.' or 'S';
	}

	static void FindMatchingStepCounts(char[,] map, Span<int> counts, long targetSteps)
	{
		var h = map.GetLength(0);
		var w = map.GetLength(1);

		if(w != h)
		{
			throw new NotSupportedException("Supports square maps only.");
		}

		var visitor = new WrappingVisitor(map);
		var current = 0;
		do
		{
			var count = visitor.NextStep();
			if(visitor.Steps % w == targetSteps % w)
			{
				counts[current++] = count;
			}
		}
		while(current < counts.Length);
	}

	static long Extrapolate(ReadOnlySpan<int> counts, long n)
	{
		long diff0 = counts[0];
		long diff1 = counts[1] - counts[0];
		long diff2 = counts[2] - counts[1];
		return diff0 + diff1 * n + (n * (n - 1) / 2) * (diff2 - diff1);
	}

	public override string Process(TextReader reader)
	{
		const int TargetSteps = 26501365;

		var map = LoadCharMap2D(reader);
		Span<int> counts = stackalloc int[3];
		FindMatchingStepCounts(map, counts, TargetSteps);
		var answer = Extrapolate(counts, TargetSteps / map.GetLength(0));
		return answer.ToString();
	}
}
