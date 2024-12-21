namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/21"/></remarks>
[Name(@"Keypad Conundrum")]
public abstract class Day21Solution : Solution
{
	protected static readonly char[,] Numpad = CreateNumpad();
	protected static readonly char[,] Dirpad = CreateDirpad();

	static char[,] CreateNumpad()
	{
		var pad = new char[4, 3];
		pad[0, 0] = '7'; pad[0, 1] = '8'; pad[0, 2] = '9';
		pad[1, 0] = '4'; pad[1, 1] = '5'; pad[1, 2] = '6';
		pad[2, 0] = '1'; pad[2, 1] = '2'; pad[2, 2] = '3';
		                 pad[3, 1] = '0'; pad[3, 2] = 'A';
		return pad;
	}

	static char[,] CreateDirpad()
	{
		var pad = new char[2, 3];
		                 pad[0, 1] = '^'; pad[0, 2] = 'A';
		pad[1, 0] = '<'; pad[1, 1] = 'v'; pad[1, 2] = '>';
		return pad;
	}

	sealed class PadState(char[,] pad, Point2D position, PadState? child)
	{
		public Point2D Position { get; private set; } = position;

		public PadState? Child { get; } = child;

		private bool CanDoXFirst(int dx, int dy)
		{
			if(dx == 0 || dy == 0) return true;
			int x = Position.X;
			int y = Position.Y;
			var sx = Math.Sign(dx);
			var sy = Math.Sign(dy);
			while(dx != 0)
			{
				if(pad[y, x] == 0) return false;
				x  += sx;
				dx -= sx;
			}
			while(dy != 0)
			{
				if(pad[y, x] == 0) return false;
				y  += sy;
				dy -= sy;
			}
			return true;
		}

		private bool CanDoYFirst(int dx, int dy)
		{
			if(dx == 0 || dy == 0) return true;
			int x = Position.X;
			int y = Position.Y;
			var sx = Math.Sign(dx);
			var sy = Math.Sign(dy);
			while(dy != 0)
			{
				if(pad[y, x] == 0) return false;
				y  += sy;
				dy -= sy;
			}
			while(dx != 0)
			{
				if(pad[y, x] == 0) return false;
				x  += sx;
				dx -= sx;
			}
			return true;
		}

		private long Press(char c, int count)
		{
			if(count == 0) return 0;
			long sum = Press(c);
			--count;
			return count != 0 ? sum + count * Press(c) : sum;
		}

		public void Reset(Point2D position)
		{
			Position = position;
		}

		public char CurrentButton => Position.GetValue(pad);

		private readonly Dictionary<(char, char), long> _cache = [];

		private long InputSequence(PadState child, int dx, int dy)
		{
			if(dx == 0 && dy == 0) return child.Press('A');

			var cx = dx < 0 ? '<' : '>';
			var cy = dy < 0 ? '^' : 'v';

			if(dx == 0) return child.Press(cy, Math.Abs(dy)) + child.Press('A');
			if(dy == 0) return child.Press(cx, Math.Abs(dx)) + child.Press('A');

			var canDoXfirst = CanDoXFirst(dx, dy);
			var canDoYfirst = !canDoXfirst || CanDoYFirst(dx, dy);

			dx = Math.Abs(dx);
			dy = Math.Abs(dy);

			if(canDoXfirst && canDoYfirst)
			{
				var testXfirst = child.Press(cx, dx) + child.Press(cy, dy) + child.Press('A');
				var testYfirst = child.Press(cy, dy) + child.Press(cx, dx) + child.Press('A');
				return Math.Min(testXfirst, testYfirst);
			}
			if(canDoXfirst) return child.Press(cx, dx) + child.Press(cy, dy) + child.Press('A');
			if(canDoYfirst) return child.Press(cy, dy) + child.Press(cx, dx) + child.Press('A');

			throw new ApplicationException();
		}

		public long Press(char c)
		{
			if(Child is null) return 1;

			var current = Position.GetValue(pad);
			var pos = FindPosition(pad, c);
			if(_cache.TryGetValue((current, c), out var score))
			{
				Position = pos;
				return score;
			}

			var dx = pos.X - Position.X;
			var dy = pos.Y - Position.Y;
			score = InputSequence(Child, dx, dy);
			Position = pos;

			_cache.Add((current, c), score);

			return score;
		}
	}

	sealed class Calc
	{
		private readonly PadState[] _states;

		public Calc(int c)
		{
			_states = new PadState[c + 1];
			var child = default(PadState);
			for(int i = _states.Length - 1; i > 0; --i)
			{
				child = _states[i] = new(Dirpad, default, child);
			}
			_states[0] = new(Numpad, default, child);
		}

		public void Reset()
		{
			var numpadPos = FindPosition(Numpad, 'A');
			var dirpadPos = FindPosition(Dirpad, 'A');
			_states[0].Reset(numpadPos);
			for(int i = 1; i < _states.Length; ++i)
			{
				_states[i].Reset(dirpadPos);
			}
		}

		public long GetScore(ReadOnlySpan<char> sequence)
		{
			Reset();
			var state = _states[0];
			var score = 0L;
			foreach(var c in sequence)
			{
				score += state.Press(c);
			}
			return score;
		}
	}

	protected static long Solve(TextReader reader, int robotControlledDirectionalPads)
	{
		var sum = 0L;
		var calc = new Calc(robotControlledDirectionalPads + 1);
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
