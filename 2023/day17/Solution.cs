namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/17"/></remarks>
[Name(@"Clumsy Crucible")]
public abstract class Day17Solution : Solution
{
	protected interface IVisitTracker<TSelf>
	{
		static abstract TSelf Create(Size2D size);

		bool TryVisit(in Crucible crucible);
	}

	protected static T[,] InitArray2D<T>(Size2D size, T value)
	{
		var array = new T[size.Height, size.Width];
		for(int i = 0; i < size.Height; ++i)
		{
			for(int j = 0; j < size.Width; ++j)
			{
				array[i, j] = value;
			}
		}
		return array;
	}

	protected readonly record struct Crucible(
		Point2D  Position,
		Direction2D Direction,
		uint        Counter,
		int         Score)
	{
		public Crucible Move(Point2D position, Direction2D direction, int cellScore)
		{
			var counter = Direction == direction
				? Counter + 1
				: 1;
			return new Crucible(position, direction, counter, Score + cellScore);
		}
	}

	protected readonly record struct Limits(
		int MinStraight,
		int MaxStraight);

	protected static Span<Crucible> GetMoves(in Crucible crucible, Limits limits, int[,] map, Span<Crucible> moves)
	{
		int offset = 0;
		if(crucible.Counter < limits.MaxStraight)
		{
			var pos = crucible.Position + Vector2D.FromDirection(crucible.Direction);
			if(pos.IsInside(map))
			{
				moves[offset++] = crucible.Move(pos, crucible.Direction, pos.GetValue(map));
			}
		}
		if(crucible.Counter >= limits.MinStraight)
		{
			var d1   = crucible.Direction.RotateCCW();
			var d2   = crucible.Direction.RotateCW();
			var pos1 = crucible.Position + Vector2D.FromDirection(d1);
			var pos2 = crucible.Position + Vector2D.FromDirection(d2);
			if(pos1.IsInside(map)) moves[offset++] = crucible.Move(pos1, d1, pos1.GetValue(map));
			if(pos2.IsInside(map)) moves[offset++] = crucible.Move(pos2, d2, pos2.GetValue(map));
		}
		return moves[..offset];
	}

	protected static int Solve<T>(TextReader reader, Limits limits)
		where T : IVisitTracker<T>
	{
		var map    = LoadDigitMap2D(reader);
		var size   = Size2D.FromArray(map);
		var visits = T.Create(size);

		var start = Point2D.Zero;
		var finish = new Point2D(
			X: size.Width - 1,
			Y: size.Height - 1);

		var crucibles = new PriorityQueue<Crucible, int>();
		crucibles.Enqueue(new Crucible(start, Direction2D.Right, 0, 0), 0);
		crucibles.Enqueue(new Crucible(start, Direction2D.Down, 0, 0), 0);

		Span<Crucible> moves = stackalloc Crucible[3];

		var bestScore = int.MaxValue;
		while(crucibles.TryDequeue(out var crucible, out _))
		{
			foreach(var next in GetMoves(crucible, limits, map, moves))
			{
				if(next.Score >= bestScore) continue;
				if(!visits.TryVisit(next)) continue;

				if(next.Position != finish)
				{
					crucibles.Enqueue(next, next.Score);
				}
				else if(next.Counter >= limits.MinStraight && next.Score < bestScore)
				{
					bestScore = next.Score;
				}
			}
		}

		return bestScore;
	}
}

public sealed class Day17SolutionPart1 : Day17Solution
{
	const int MinStraight = 0;
	const int MaxStraight = 3;

	sealed class VisitTracker(Size2D size) : IVisitTracker<VisitTracker>
	{
		public static VisitTracker Create(Size2D size) => new(size);

		[System.Runtime.CompilerServices.InlineArray(MaxStraight * 4)]
		struct Cell
		{
			public static readonly Cell Default = CreateDefault();

			private static Cell CreateDefault()
			{
				var v = new Cell();
				for(int i = 0; i < MaxStraight * 4; ++i)
				{
					v[i] = int.MaxValue;
				}
				return v;
			}

			#pragma warning disable IDE0044 // Add readonly modifier
			#pragma warning disable IDE0051 // Remove unused private members
			private int _element0;
			#pragma warning restore IDE0051 // Remove unused private members
			#pragma warning restore IDE0044 // Add readonly modifier
		}

		private readonly Cell[,] _visits = InitArray2D(size, Cell.Default);

		public bool TryVisit(in Crucible crucible)
		{
			ref Cell cell = ref crucible.Position.GetValue(_visits);
			var offset = (int)crucible.Direction * MaxStraight + (int)crucible.Counter - 1;
			var score  = cell[offset];
			if(score <= crucible.Score) return false;
			cell[offset] = crucible.Score;
			return true;
		}
	}

	public override string Process(TextReader reader)
		=> Solve<VisitTracker>(reader, new(MinStraight, MaxStraight)).ToString();
}

public sealed class Day17SolutionPart2 : Day17Solution
{
	const int MinStraight =  4;
	const int MaxStraight = 10;

	sealed class VisitTracker(Size2D size) : IVisitTracker<VisitTracker>
	{
		public static VisitTracker Create(Size2D size) => new(size);

		[System.Runtime.CompilerServices.InlineArray(MaxStraight * 4)]
		struct Cell
		{
			public static readonly Cell Default = CreateDefault();

			private static Cell CreateDefault()
			{
				var v = new Cell();
				for(int i = 0; i < MaxStraight * 4; ++i)
				{
					v[i] = int.MaxValue;
				}
				return v;
			}

			#pragma warning disable IDE0044 // Add readonly modifier
			#pragma warning disable IDE0051 // Remove unused private members
			private int _element0;
			#pragma warning restore IDE0051 // Remove unused private members
			#pragma warning restore IDE0044 // Add readonly modifier
		}

		private readonly Cell[,] _visits = InitArray2D(size, Cell.Default);

		public bool TryVisit(in Crucible crucible)
		{
			ref Cell cell = ref crucible.Position.GetValue(_visits);
			var offset = (int)crucible.Direction * MaxStraight + (int)crucible.Counter - 1;
			var score  = cell[offset];
			if(score <= crucible.Score) return false;
			cell[offset] = crucible.Score;
			return true;
		}
	}

	public override string Process(TextReader reader)
		=> Solve<VisitTracker>(reader, new(MinStraight, MaxStraight)).ToString();
}
