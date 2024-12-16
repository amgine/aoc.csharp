namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/15"/></remarks>
[Name(@"Warehouse Woes")]
public abstract class Day15Solution : Solution
{
	protected record class Input(char[,] Map, Queue<Vector2D> Instructions);

	protected virtual char[,] CreateMap(List<string> lines)
	{
		var map = new char[lines.Count, lines[0].Length];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				map[y, x] = lines[y][x];
			}
		}
		return map;
	}

	private static Queue<Vector2D> ReadInstructions(TextReader reader)
	{
		Queue<Vector2D> instructions = new();
		while(reader.ReadLine() is { } line)
		{
			foreach(var c in line)
			{
				instructions.Enqueue(c switch
				{
					'<' => Vector2D.Left,
					'^' => Vector2D.Up,
					'>' => Vector2D.Right,
					'v' => Vector2D.Down,
					_ => throw new InvalidDataException(),
				});
			}
		}
		return instructions;
	}

	private char[,] ReadMap(TextReader reader)
	{
		var lines = new List<string>();
		while(reader.ReadLine() is { Length: not 0 } line)
		{
			lines.Add(line);
		}
		return CreateMap(lines);
	}

	private Input ReadInput(TextReader reader)
		=> new(ReadMap(reader), ReadInstructions(reader));

	protected static long GetScore(char[,] map, char box)
	{
		var sum = 0L;
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] == box)
				{
					sum += y * 100 + x;
				}
			}
		}
		return sum;
	}

	protected abstract long Solve(Input input);

	protected static Point2D FindStartingPosition(char[,] map)
	{
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] is '@')
				{
					map[y, x] = '.';
					return new(x, y);
				}
			}
		}
		throw new InvalidDataException();
	}

	public sealed override string Process(TextReader reader)
		=> Solve(ReadInput(reader)).ToString();
}

public sealed class Day15SolutionPart1 : Day15Solution
{
	private static void Execute(Input input)
	{
		var map = input.Map;
		var pos = FindStartingPosition(map);
		while(input.Instructions.TryDequeue(out var v))
		{
			var next = pos + v;
			if(!next.IsInside(map)) continue;
			switch(next.GetValue(map))
			{
				case '#': continue;
				case '.': pos = next; continue;
				case 'O':
					var nn = next + v;
					while(nn.IsInside(map) && nn.GetValue(map) == 'O')
					{
						nn += v;
					}
					if(nn.IsInside(map) && nn.GetValue(map) == '.')
					{
						nn.GetValue(map) = 'O';
						next.GetValue(map) = '.';
						pos = next;
					}
					break;
				default: throw new InvalidDataException();
			}
		}
	}

	protected override long Solve(Input input)
	{
		Execute(input);
		return GetScore(input.Map, 'O');
	}
}

public sealed class Day15SolutionPart2 : Day15Solution
{
	protected override char[,] CreateMap(List<string> lines)
	{
		static (char, char) Expand(char v) =>
			v switch
			{
				'@' => ('@', '.'),
				'.' => ('.', '.'),
				'#' => ('#', '#'),
				'O' => ('[', ']'),
				_ => throw new InvalidDataException(),
			};

		var map = new char[lines.Count, lines[0].Length * 2];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				(map[y, x * 2 + 0], map[y, x * 2 + 1]) = Expand(lines[y][x]);
			}
		}
		return map;
	}

	private static void Execute(Input input)
	{
		static bool TryMoveBoxHorizontal(char[,] map, Point2D p, Vector2D v)
		{
			var n = p + v;
			var (c0, c1) = v.DeltaX > 0 ? ('[', ']') : (']', '[');
			while(n.GetValue(map) == c0)
			{
				n += v;
				n += v;
			}
			if(n.GetValue(map) is '#') return false;
			var w = p + v;
			w.GetValue(map) = '.';
			do
			{
				(w += v).GetValue(map) = c0;
				(w += v).GetValue(map) = c1;
			}
			while(w != n);
			return true;
		}

		static bool CanMoveVertical(char[,] map, Point2D p, Vector2D v)
		{
			var n0 = p + v;
			var b0 = n0.GetValue(map);
			Point2D n1;

			     if(b0 == '[') n1 = n0 + Vector2D.Right;
			else if(b0 == ']') n1 = n0 + Vector2D.Left;
			else return b0 == '.';

			return CanMoveVertical(map, n0, v)
				&& CanMoveVertical(map, n1, v);
		}

		static void MoveBoxVertical(char[,] map, Point2D p, Vector2D v)
		{
			var n0 = p + v;
			Point2D n1;
			switch(n0.GetValue(map))
			{
				case '[':
					n1 = n0 + Vector2D.Right;
					break;
				case ']':
					n1 = n0 + Vector2D.Left;
					(n0, n1) = (n1, n0);
					break;
				default: return;
			}
			var n0n = n0 + v;
			var n1n = n1 + v;
			if(n0n.GetValue(map) != '.') MoveBoxVertical(map, n0, v);
			if(n1n.GetValue(map) != '.') MoveBoxVertical(map, n1, v);
			(n0n.GetValue(map), n1n.GetValue(map)) = ('[', ']');
			(n0 .GetValue(map), n1 .GetValue(map)) = ('.', '.');
		}

		static bool TryMoveBoxVertical(char[,] map, Point2D p, Vector2D v)
		{
			if(!CanMoveVertical(map, p, v)) return false;
			MoveBoxVertical(map, p, v);
			return true;
		}

		var map = input.Map;
		var pos = FindStartingPosition(map);
		while(input.Instructions.TryDequeue(out var v))
		{
			var next = pos + v;
			if(!next.IsInside(map)) continue;
			switch(next.GetValue(map))
			{
				case '#': continue;
				case '.': pos = next; continue;
				case '[' or ']':
					if(v.DeltaY == 0)
					{
						if(TryMoveBoxHorizontal(map, pos, v)) pos = next;
					}
					else
					{
						if(TryMoveBoxVertical(map, pos, v)) pos = next;
					}
					continue;
				default: throw new InvalidDataException();
			}
		}
	}

	protected override long Solve(Input input)
	{
		Execute(input);
		return GetScore(input.Map, '[');
	}
}
