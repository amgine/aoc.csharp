using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/21"/></remarks>
[Name(@"Keypad Conundrum")]
public abstract class Day21Solution : Solution
{
	static readonly Pad Numpad = Pad.CreateNumeric();
	static readonly Pad Dirpad = Pad.CreateDirectional();

	sealed class Pad(char[,] map)
	{
		public static Pad CreateNumeric()
		{
			var pad = new char[4, 3];
			pad[0, 0] = '7'; pad[0, 1] = '8'; pad[0, 2] = '9';
			pad[1, 0] = '4'; pad[1, 1] = '5'; pad[1, 2] = '6';
			pad[2, 0] = '1'; pad[2, 1] = '2'; pad[2, 2] = '3';
							 pad[3, 1] = '0'; pad[3, 2] = 'A';
			return new(pad);
		}

		public static Pad CreateDirectional()
		{
			var pad = new char[2, 3];
							 pad[0, 1] = '^'; pad[0, 2] = 'A';
			pad[1, 0] = '<'; pad[1, 1] = 'v'; pad[1, 2] = '>';
			return new(pad);
		}

		private static ImmutableDictionary<char, Point2D> CreateLookup(char[,] map)
		{
			var builder = ImmutableDictionary.CreateBuilder<char, Point2D>();
			for(int y = 0, height = map.GetLength(0); y < height; ++y)
			{
				for(int x = 0, width = map.GetLength(1); x < width; ++x)
				{
					var c = map[y, x];
					if(c == 0) continue;
					builder.Add(c, new(x, y));
				}
			}
			return builder.ToImmutable();
		}

		private readonly ImmutableDictionary<char, Point2D> _lookup = CreateLookup(map);

		public Point2D GetButtonPosition(char button)
			=> _lookup[button];

		public char GetButtonAt(Point2D position)
			=> map[position.Y, position.X];

		private bool CanMoveH(ref int x, int y, int dx)
		{
			var sx = Math.Sign(dx);
			while(dx != 0)
			{
				if(map[y, x] == 0) return false;
				x  += sx;
				dx -= sx;
			}
			return true;
		}

		private bool CanMoveV(int x, ref int y, int dy)
		{
			var sy = Math.Sign(dy);
			while(dy != 0)
			{
				if(map[y, x] == 0) return false;
				y  += sy;
				dy -= sy;
			}
			return true;
		}

		public bool CanMoveXFirst(Point2D position, int dx, int dy)
		{
			int x = position.X;
			int y = position.Y;
			return CanMoveH(ref x, y, dx)
				&& CanMoveV(x, ref y, dy);
		}

		public bool CanMoveYFirst(Point2D position, int dx, int dy)
		{
			int x = position.X;
			int y = position.Y;
			return CanMoveV(x, ref y, dy)
				&& CanMoveH(ref x, y, dx);
		}
	}

	sealed class PadState(Pad pad, Point2D position, PadState? next)
	{
		private readonly Point2D _initialPosition = position;

		public Point2D Position { get; private set; } = position;

		public Pad Pad { get; } = pad;

		public PadState? Next { get; } = next;

		private long Press(char c, int count)
		{
			if(count == 0) return 0;
			long sum = Press(c);
			--count;
			return count != 0 ? sum + count * Press(c) : sum;
		}

		private void ResetCore()
		{
			Position = _initialPosition;
		}

		public void Reset()
		{
			ResetCore();
			var next = Next;
			while(next is not null)
			{
				next.ResetCore();
				next = next.Next;
			}
		}

		readonly struct LookupKey(char c1, char c2)
		{
			public readonly char C1 = c1;

			public readonly char C2 = c2;

			sealed class EqualityComparerImpl : IEqualityComparer<LookupKey>
			{
				public bool Equals(LookupKey x, LookupKey y)
					=> x.C1 == y.C1 && x.C2 == y.C2;

				public int GetHashCode(LookupKey obj)
					=> (obj.C1 << 16) | obj.C2;
			}

			public static readonly IEqualityComparer<LookupKey> EqualityComparer
				= new EqualityComparerImpl();
		}

		private readonly Dictionary<LookupKey, long> _cache = new(LookupKey.EqualityComparer);

		private bool CanMoveXFirst(int dx, int dy)
			=> Pad.CanMoveXFirst(Position, dx, dy);

		private bool CanMoveYFirst(int dx, int dy)
			=> Pad.CanMoveYFirst(Position, dx, dy);

		private long InputSequence(PadState parent, Point2D pos)
		{
			var dx = pos.X - parent.Position.X;
			var dy = pos.Y - parent.Position.Y;

			if(dx == 0 && dy == 0) return Press('A');

			var cx = dx < 0 ? '<' : '>';
			var cy = dy < 0 ? '^' : 'v';

			if(dx == 0) return Press(cy, Math.Abs(dy)) + Press('A');
			if(dy == 0) return Press(cx, Math.Abs(dx)) + Press('A');

			var canDoXfirst = parent.CanMoveXFirst(dx, dy);
			var canDoYfirst = !canDoXfirst || parent.CanMoveYFirst(dx, dy);

			dx = Math.Abs(dx);
			dy = Math.Abs(dy);

			if(canDoXfirst && canDoYfirst)
			{
				return Math.Min(
					Press(cx, dx) + Press(cy, dy) + Press('A'),
					Press(cy, dy) + Press(cx, dx) + Press('A'));
			}
			if(canDoXfirst) return Press(cx, dx) + Press(cy, dy) + Press('A');
			if(canDoYfirst) return Press(cy, dy) + Press(cx, dx) + Press('A');

			throw new ApplicationException();
		}

		public long Press(char c)
		{
			if(Next is null) return 1;

			var current = Pad.GetButtonAt(Position);
			var pos     = Pad.GetButtonPosition(c);
			var key     = new LookupKey(current, c);
			if(!_cache.TryGetValue(key, out var score))
			{
				score = Next.InputSequence(this, pos);
				_cache.Add(key, score);
			}
			Position = pos;
			return score;
		}
	}

	sealed class Calc(int robotControlledDirectionalPads)
	{
		private readonly PadState _state = CreateSequence(robotControlledDirectionalPads);

		private static PadState CreateSequence(int robotControlledDirectionalPads)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(robotControlledDirectionalPads);

			var numpadPos = Numpad.GetButtonPosition('A');
			var dirpadPos = Dirpad.GetButtonPosition('A');
			var child = default(PadState);
			while(robotControlledDirectionalPads-- >= 0)
			{
				child = new(Dirpad, dirpadPos, child);
			}
			return new(Numpad, numpadPos, child);
		}

		public long GetScore(ReadOnlySpan<char> sequence)
		{
			_state.Reset();
			var score = 0L;
			foreach(var c in sequence)
			{
				score += _state.Press(c);
			}
			return score;
		}
	}

	protected static long Solve(TextReader reader, int robotControlledDirectionalPads)
	{
		var sum = 0L;
		var calc = new Calc(robotControlledDirectionalPads);
		string? code;
		while((code = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(code)) continue;
			if(!code.EndsWith('A')) throw new InvalidDataException();
			var shortest = calc.GetScore(code);
			var complexity = shortest * int.Parse(code.AsSpan(0, code.Length - 1));
			sum += complexity;
		}
		return sum;
	}
}

public sealed class Day21SolutionPart1 : Day21Solution
{
	public override string Process(TextReader reader)
		=> Solve(reader, robotControlledDirectionalPads: 2).ToString();
}

public sealed class Day21SolutionPart2 : Day21Solution
{
	public override string Process(TextReader reader)
		=> Solve(reader, robotControlledDirectionalPads: 25).ToString();
}
