namespace AoC.Year2023;

[Name(@"The Floor Will Be Lava")]
public abstract class Day16Solution : Solution
{
	struct Cell
	{
		byte _state;

		public bool EnergizeOnce(Direction direction)
		{
			var mask = 1 << (int)direction;
			if((_state & mask) != 0) return false;
			_state = (byte)(_state | mask);
			return true;
		}

		public readonly bool IsEnergized => _state != 0;
	}

	static Cell[,] InitCells(char[,] map)
		=> new Cell[map.GetLength(0), map.GetLength(1)];

	protected enum Direction
	{
		Left,
		Top,
		Right,
		Bottom,
	}

	protected readonly record struct Offset(int DeltaX = 0, int DeltaY = 0);

	static readonly Offset[] Offsets = [new(DeltaX: -1), new(DeltaY: -1), new(DeltaX: 1), new(DeltaY: 1)];

	protected readonly record struct Position(int X, int Y)
	{
		public bool IsInside<T>(T[,] map)
			=> X >= 0
			&& Y >= 0
			&& X < map.GetLength(1)
			&& Y < map.GetLength(0);

		public Position Offset(Direction direction)
			=> this + Offsets[(int)direction];

		public static Position operator+(Position position, Offset offset)
			=> new(position.X + offset.DeltaX, position.Y + offset.DeltaY);
	}

	protected readonly record struct Beam(Position Position, Direction Direction)
	{
		public Beam PassThrough() => new(Position.Offset(Direction), Direction);

		public Beam Reflect(char cell)
		{
			var direction = ReflectDirection(cell, Direction);
			return new(Position.Offset(direction), direction);
		}

		public (Beam, Beam) Split() => IsHorizontal(Direction)
			? (new(Position.Offset(Direction.Top),  Direction.Top),  new(Position.Offset(Direction.Bottom), Direction.Bottom))
			: (new(Position.Offset(Direction.Left), Direction.Left), new(Position.Offset(Direction.Right),  Direction.Right));
	}

	static Direction ReflectDirection(char mirror, Direction direction)
		=> mirror switch
		{
			'\\' => direction switch
			{
				Direction.Left   => Direction.Top,
				Direction.Right  => Direction.Bottom,
				Direction.Top    => Direction.Left,
				Direction.Bottom => Direction.Right,
				_ => throw new ArgumentException($"Unknown direction: {direction}", nameof(direction)),
			},
			'/' => direction switch
			{
				Direction.Left   => Direction.Bottom,
				Direction.Right  => Direction.Top,
				Direction.Top    => Direction.Right,
				Direction.Bottom => Direction.Left,
				_ => throw new ArgumentException($"Unknown direction: {direction}", nameof(direction)),
			},
			_ => throw new ArgumentException($"Non-reflecting cell: {mirror}", nameof(mirror)),
		};

	enum BeamInteraction { PassThrough, Split, Reflect }

	static BeamInteraction GetInteraction(char cell, Direction direction)
		=> cell switch
		{
			'.' => BeamInteraction.PassThrough,
			'\\' or '/' => BeamInteraction.Reflect,
			'|' => IsHorizontal(direction) ? BeamInteraction.Split : BeamInteraction.PassThrough,
			'-' => IsHorizontal(direction) ? BeamInteraction.PassThrough : BeamInteraction.Split,
			_ => throw new InvalidDataException(),
		};

	static bool IsHorizontal(Direction direction)
		=> direction is Direction.Left or Direction.Right;

	static ref T GetCell<T>(T[,] map, Position position)
		=> ref map[position.Y, position.X];

	static void CastBeam(char[,] map, Cell[,] cells, Stack<Beam> activeBeams, Beam beam)
	{
		var pos = beam.Position;

		if(!pos.IsInside(map)) return;
		if(!GetCell(cells, pos).EnergizeOnce(beam.Direction)) return;

		var cell = GetCell(map, pos);
		switch(GetInteraction(cell, beam.Direction))
		{
			case BeamInteraction.PassThrough:
				activeBeams.Push(beam.PassThrough());
				break;
			case BeamInteraction.Split:
				var (beam1, beam2) = beam.Split();
				activeBeams.Push(beam1);
				activeBeams.Push(beam2);
				break;
			case BeamInteraction.Reflect:
				activeBeams.Push(beam.Reflect(cell));
				break;
			default: throw new ApplicationException("Invalid beam interaction.");
		}
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

	static int CountEnrgizedCells(Cell[,] cells)
	{
		var count = 0;
		foreach(var cell in cells)
		{
			if(cell.IsEnergized) ++count;
		}
		return count;
	}

	protected static int CountEnergized(char[,] map, Beam initialBeam)
	{
		var cells = InitCells(map);
		var beams = new Stack<Beam>();
		beams.Push(initialBeam);
		do
		{
			CastBeam(map, cells, beams, beams.Pop());
		} while(beams.Count > 0);
		return CountEnrgizedCells(cells);
	}
}

public sealed class Day16SolutionPart1 : Day16Solution
{
	static readonly Beam InitialBeam = new(new(0, 0), Direction.Right);

	public override string Process(TextReader reader)
		=> CountEnergized(LoadMap(reader), InitialBeam).ToString();
}

public sealed class Day16SolutionPart2 : Day16Solution
{
	static IEnumerable<Beam> GetInitialBeams<T>(T[,] map)
	{
		var width  = map.GetLength(1);
		var height = map.GetLength(0);

		if(width <= 0 || height <= 0) yield break;

		const int x0 = 0;
		var       x1 = width - 1;

		const int y0 = 0;
		var       y1 = height - 1;

		for(int y = 0; y < height; ++y)
		{
			yield return new Beam(new(x0, y), Direction.Right);
			yield return new Beam(new(x1, y), Direction.Left);
		}
		for(int x = 0; x < width; ++x)
		{
			yield return new Beam(new(x, y0), Direction.Bottom);
			yield return new Beam(new(x, y1), Direction.Top);
		}
	}

	public override string Process(TextReader reader)
	{
		var map = LoadMap(reader);
		var max = 0;
		foreach(var beam in GetInitialBeams(map))
		{
			var count = CountEnergized(map, beam);
			if(count > max) max = count;
		}
		return max.ToString();
	}
}
