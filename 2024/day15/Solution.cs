namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/15"/></remarks>
[Name(@"Warehouse Woes")]
public abstract class Day15Solution : Solution
{
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
}

public sealed class Day15SolutionPart1 : Day15Solution
{
	private static void Execute(char[,] map, Queue<Vector2D> instructions)
	{
		var pos = FindStartingPosition(map);
		while(instructions.TryDequeue(out var instruction))
		{
			var next = pos + instruction;
			if(!next.IsInside(map)) continue;
			var v = next.GetValue(map);
			if(v == '.')
			{
				pos = next;
			}
			else if(v == 'O')
			{
				var nn = next + instruction;
				while(nn.IsInside(map) && nn.GetValue(map) == 'O')
				{
					nn += instruction;
				}
				if(nn.IsInside(map) && nn.GetValue(map) == '.')
				{
					nn.GetValue(map) = 'O';
					next.GetValue(map) = '.';
					pos = next;
				}
			}
			else if(v == '#')
			{
				continue;
			}
		}
	}

	private static long GetScore(char[,] map)
	{
		var sum = 0L;
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] is 'O')
				{
					sum += y * 100 + x;
				}
			}
		}
		return sum;
	}

	public override string Process(TextReader reader)
	{
		Queue<Vector2D> instructions = new();

		var lines = new List<string>();
		string? line;
		var isReadingInstructions = false;
		while((line = reader.ReadLine()) is not null)
		{
			if(isReadingInstructions)
			{
				for(int i = 0; i < line.Length; ++i)
				{
					instructions.Enqueue(line[i] switch
					{
						'<' => Vector2D.Left,
						'^' => Vector2D.Up,
						'>' => Vector2D.Right,
						'v' => Vector2D.Down,
						_ => throw new InvalidDataException(),
					});
				}
			}
			else
			{
				if(line.Length == 0)
				{
					isReadingInstructions = true;
					continue;
				}
				lines.Add(line);
			}
		}
		var map = new char[lines.Count, lines[0].Length];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				map[y, x] = lines[y][x];
			}
		}
		Execute(map, instructions);
		return GetScore(map).ToString();
	}
}

public sealed class Day15SolutionPart2 : Day15Solution
{
	static char[,] Expand(char[,] map)
	{
		var expanded = new char[map.GetLength(0), map.GetLength(1) * 2];
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				switch(map[y, x])
				{
					case '@':
						expanded[y, x * 2 + 0] = '@';
						expanded[y, x * 2 + 1] = '.';
						break;
					case '.':
						expanded[y, x * 2 + 0] = '.';
						expanded[y, x * 2 + 1] = '.';
						break;
					case '#':
						expanded[y, x * 2 + 0] = '#';
						expanded[y, x * 2 + 1] = '#';
						break;
					case 'O':
						expanded[y, x * 2 + 0] = '[';
						expanded[y, x * 2 + 1] = ']';
						break;
					default: throw new InvalidDataException();
				}
			}
		}
		return expanded;
	}

	private static void Execute(char[,] map, Queue<Vector2D> instructions)
	{
		static bool TryMoveBoxHorizontal(char[,] map, Point2D p, Vector2D v)
		{
			var n = p + v;
			while(n.GetValue(map) is '[' or ']') n += v;
			if(n.GetValue(map) is '#') return false;
			var w = p + v;
			w.GetValue(map) = '.';
			w += v;
			n += v;
			var (c0, c1) = v.DeltaX > 0 ? ('[', ']') : (']', '[');
			while(w != n)
			{
				w.GetValue(map) = c0;
				w += v;
				w.GetValue(map) = c1;
				w += v;
			}
			return true;
		}

		static bool IsFree(char[,] map, Point2D n0, Point2D n1)
		{
			return n0.GetValue(map) == '.' && n1.GetValue(map) == '.';
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
			var b0 = n0.GetValue(map);
			Point2D n1;
			if(b0 == '[')
			{
				n1 = n0 + Vector2D.Right;
			}
			else if(b0 == ']')
			{
				n1 = n0 + Vector2D.Left;
				(n0, n1) = (n1, n0);
			}
			else return;
			if(!IsFree(map, n0 + v, n1 + v))
			{
				MoveBoxVertical(map, n0, v);
				MoveBoxVertical(map, n1, v);
			}
			(n0 + v).GetValue(map) = '[';
			(n1 + v).GetValue(map) = ']';
			n0.GetValue(map) = '.';
			n1.GetValue(map) = '.';
		}

		static bool TryMoveBoxVertical(char[,] map, Point2D p, Vector2D v)
		{
			if(!CanMoveVertical(map, p, v)) return false;
			MoveBoxVertical(map, p, v);
			return true;
		}

		var pos = FindStartingPosition(map);
		while(instructions.TryDequeue(out var instruction))
		{
			var next = pos + instruction;
			if(!next.IsInside(map)) continue;
			var v = next.GetValue(map);
			if(v == '.')
			{
				pos = next;
			}
			else if(v is '[' or ']')
			{
				if(instruction.DeltaY == 0)
				{
					if(TryMoveBoxHorizontal(map, pos, instruction))
					{
						pos = next;
					}
				}
				else
				{
					if(TryMoveBoxVertical(map, pos, instruction))
					{
						pos = next;
					}
				}
			}
			else if(v == '#')
			{
				continue;
			}
		}
	}

	private static long GetScore(char[,] map)
	{
		var sum = 0L;
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] is '[')
				{
					sum += y * 100 + x;
				}
			}
		}
		return sum;
	}

	public override string Process(TextReader reader)
	{
		Queue<Vector2D> instructions = new();

		var lines = new List<string>();
		string? line;
		var isReadingInstructions = false;
		while((line = reader.ReadLine()) is not null)
		{
			if(isReadingInstructions)
			{
				for(int i = 0; i < line.Length; ++i)
				{
					instructions.Enqueue(line[i] switch
					{
						'<' => Vector2D.Left,
						'^' => Vector2D.Up,
						'>' => Vector2D.Right,
						'v' => Vector2D.Down,
						_ => throw new InvalidDataException(),
					});
				}
			}
			else
			{
				if(line.Length == 0)
				{
					isReadingInstructions = true;
					continue;
				}
				lines.Add(line);
			}
		}
		var map = new char[lines.Count, lines[0].Length];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				map[y, x] = lines[y][x];
			}
		}
		map = Expand(map);
		Execute(map, instructions);

		return GetScore(map).ToString();
	}
}
