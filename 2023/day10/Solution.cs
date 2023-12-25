namespace AoC.Year2023;

[Name(@"Pipe Maze")]
public abstract class Day10Solution : Solution
{
	protected readonly record struct Offset(int DeltaX = 0, int DeltaY = 0)
	{
		public static readonly Offset Left   = new(DeltaX: -1);
		public static readonly Offset Top    = new(DeltaY: -1);
		public static readonly Offset Right  = new(DeltaX:  1);
		public static readonly Offset Bottom = new(DeltaY:  1);
	};

	protected readonly record struct Position(int X, int Y)
	{
		public static Position operator +(Position position, Offset offset)
			=> new(position.X + offset.DeltaX, position.Y + offset.DeltaY);

		public bool IsInside(char[,] map)
			=> X >= 0 && Y >= 0 && X < map.GetLength(0) && Y < map.GetLength(1);
	}

	static Position FindPosition(char[,] map, char value)
	{
		for(int i = 0; i < map.GetLength(0); ++i)
		{
			for(int j = 0; j < map.GetLength(1); ++j)
			{
				if(map[i, j] == value) return new(i, j);
			}
		}
		throw new InvalidDataException();
	}

	static List<Position> GetExits(char[,] map, Position from)
	{
		var exits = new List<Position>(capacity: 2);
		foreach(var offset in Offsets)
		{
			var to = from + offset;
			if(!CanNavigate(map, from, to)) continue;
			exits.Add(to);
		}
		return exits;
	}

	protected static Position FindStart(char[,] map, out Position exit1, out Position exit2)
	{
		var start = FindPosition(map, 'S');
		var exits = GetExits(map, start);

		if(exits.Count != 2) throw new InvalidDataException();
		exit1 = exits[0];
		exit2 = exits[1];

		return start;
	}

	protected static char[,] LoadMap(TextReader reader)
	{
		var lines = new List<string>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			lines.Add(line);
		}

		var map = new char[lines[0].Length, lines.Count];
		for(int i = 0; i < lines.Count; ++i)
		{
			for(int j = 0; j < lines[i].Length; ++j)
			{
				map[j, i] = lines[i][j];
			}
		}
		return map;
	}

	readonly record struct Connections(
		bool Left   = false,
		bool Top    = false,
		bool Right  = false,
		bool Bottom = false);

	private static readonly Dictionary<char, Connections> _connections = new()
	{
		['S'] = new(Left:  true, Top:    true, Right: true, Bottom: true),
		['-'] = new(Left:  true, Right:  true),
		['|'] = new(Top:   true, Bottom: true),
		['L'] = new(Top:   true, Right:  true),
		['F'] = new(Right: true, Bottom: true),
		['J'] = new(Left:  true, Top:    true),
		['7'] = new(Left:  true, Bottom: true),
	};

	protected static bool CanNavigate(char[,] map, Position from, Position to)
	{
		if(!to.IsInside(map)) return false;

		if(!_connections.TryGetValue(map[from.X, from.Y], out var cfrom)) return false;
		if(!_connections.TryGetValue(map[to.X,   to.Y],   out var cto))   return false;

		if(from.Y == to.Y)
		{
			if(to.X == from.X + 1) return cfrom.Right && cto.Left;
			if(to.X == from.X - 1) return cfrom.Left  && cto.Right;
		}

		if(from.X == to.X)
		{
			if(to.Y == from.Y + 1) return cfrom.Bottom && cto.Top;
			if(to.Y == from.Y - 1) return cfrom.Top    && cto.Bottom;
		}

		return false;
	}

	static readonly Offset[] Offsets = [Offset.Left, Offset.Top, Offset.Right, Offset.Bottom];

	protected static Position Next(char[,] map, Position prev, Position pos)
	{
		foreach(var offset in Offsets)
		{
			var test = pos + offset;
			if(test != prev && CanNavigate(map, pos, test)) return test;
		}
		throw new InvalidDataException();
	}

	protected static void Print(char[,] map)
	{
		for(int y = 0; y < map.GetLength(1); ++y)
		{
			for(int x = 0; x < map.GetLength(0); ++x)
			{
				var c = map[x, y];
				if(c == '\0') c = '.';
				Console.Write(c);
			}
			Console.WriteLine();
		}
	}
}

public sealed class Day10SolutionPart1 : Day10Solution
{
	public override string Process(TextReader reader)
	{
		var map   = LoadMap(reader);
		var start = FindStart(map, out var pos0, out var pos1);

		var prev0 = start;
		var prev1 = start;

		var length = 1;
		while(pos0 != pos1)
		{
			var next0 = Next(map, prev0, pos0);
			var next1 = Next(map, prev1, pos1);

			prev0 = pos0;
			prev1 = pos1;
			pos0 = next0;
			pos1 = next1;

			if(pos1 == prev0 || pos0 == prev1) break;

			++length;
		}

		return length.ToString();
	}
}

public sealed class Day10SolutionPart2 : Day10Solution
{
	static int GetArea(char[,] fill)
	{
		var count = 0;
		for(int y = 0; y < fill.GetLength(1); ++y)
		{
			for(int x = 0; x < fill.GetLength(0); ++x)
			{
				if(fill[x, y] == 'I') ++count;
			}
		}
		return count;
	}

	static void PaintBorder(char[,] map, char[,] fill)
	{
		var start = FindStart(map, out var pos, out _);
		var prev = start;

		fill[start.X, start.Y] = '#';
		fill[pos.X,   pos.Y]   = '#';

		while(pos != start)
		{
			var next = Next(map, prev, pos);
			prev = pos;
			pos  = next;
			fill[pos.X, pos.Y] = '#';
		}
	}

	static void FillInside(char[,] map, char[,] fill, int horizontalSign, int verticalSign)
	{
		static void Fill(char[,] fill, Position pos, Offset offset)
		{
			for(pos += offset; pos.IsInside(fill); pos += offset)
			{
				if(fill[pos.X, pos.Y] == '#') break;
				fill[pos.X, pos.Y] = 'I';
			}
		}

		static Offset GetFillOffset(Position pos, Position next, int horizontalSign, int verticalSign) => new(
			DeltaX: (next.Y - pos.Y) * horizontalSign,
			DeltaY: (next.X - pos.X) * verticalSign);

		var start = FindStart(map, out var pos, out _);
		var prev  = start;

		while(pos != start)
		{
			var next   = Next(map, prev, pos);
			var offset = GetFillOffset(pos, next, horizontalSign, verticalSign);
			Fill(fill, pos,  offset);
			Fill(fill, next, offset);
			prev = pos;
			pos = next;
		}
	}

	static void GetSigns(char[,] map, char[,] fill, out int horizontalSign, out int verticalSign)
	{
		static bool CheckIfBorderIsVisible(char[,] fill, Position pos, Offset offset)
		{
			for(pos += offset; pos.IsInside(fill); pos += offset)
			{
				if(fill[pos.X, pos.Y] == '#') return true;
			}
			return false;
		}

		static bool UpdateSign(char[,] fill, Position pos, Position next,
			Offset offset1, Offset offset2, ref int sign)
		{
			if(sign != 0) return false;

			if( !CheckIfBorderIsVisible(fill, pos,  offset1) ||
				!CheckIfBorderIsVisible(fill, next, offset1))
			{
				sign =
					(next.Y - pos.Y) * -offset1.DeltaX +
					(next.X - pos.X) * -offset1.DeltaY;

				return true;
			}
			if( !CheckIfBorderIsVisible(fill, pos,  offset2) ||
				!CheckIfBorderIsVisible(fill, next, offset2))
			{
				sign =
					(next.Y - pos.Y) * -offset2.DeltaX +
					(next.X - pos.X) * -offset2.DeltaY;

				return true;
			}

			return false;
		}

		horizontalSign = 0;
		verticalSign   = 0;

		var start = FindStart(map, out var pos, out _);
		var prev  = start;

		while(pos != start)
		{
			var next = Next(map, prev, pos);

			if(next.X == pos.X)
			{
				UpdateSign(fill, pos, next, Offset.Left, Offset.Right, ref horizontalSign);
			}
			else if(next.Y == pos.Y)
			{
				UpdateSign(fill, pos, next, Offset.Top, Offset.Bottom, ref verticalSign);
			}

			if(verticalSign != 0 && horizontalSign != 0) break;

			prev = pos;
			pos = next;
		}

		if(verticalSign == 0 || horizontalSign == 0)
		{
			throw new InvalidDataException();
		}
	}

	public override string Process(TextReader reader)
	{
		var map  = LoadMap(reader);
		var fill = new char[map.GetLength(0), map.GetLength(1)];

		PaintBorder(map, fill);
		GetSigns   (map, fill, out var horizontalSign, out var verticalSign);
		FillInside (map, fill, horizontalSign, verticalSign);

		//Print(fill);

		return GetArea(fill).ToString();
	}
}
