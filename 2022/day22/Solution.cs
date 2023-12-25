namespace AoC.Year2022;

[Name(@"Monkey Map")]
public abstract class Day22Solution : Solution
{
	protected readonly record struct Position(int X, int Y)
	{
		public static Position operator+(Position position, Offset offset)
			=> new(position.X + offset.DeltaX, position.Y + offset.DeltaY);

		public bool IsInside<T>(T[,] map)
			=> X >= 0 && Y >= 0 && X < map.GetLength(0) && Y < map.GetLength(1);
	}

	protected readonly record struct Offset(int DeltaX = 0, int DeltaY = 0);

	protected readonly record struct State(Position Position, Direction Direction)
	{
		public int GetPassword()
		{
			var directionScore = Direction switch
			{
				Direction.Left  => 2,
				Direction.Up    => 3,
				Direction.Right => 0,
				Direction.Down  => 1,
				_ => throw new Exception(),
			};
			return 1000 * (Position.Y + 1)
				+     4 * (Position.X + 1)
				+          directionScore;
		}
	};

	protected enum Direction { Left, Up, Right, Down }

	enum RotateDirection { Left, Right };

	protected interface ICommand
	{
		State Execute(char[,] map, State state);
	}

	protected interface IWrapper
	{
		State Wrap(State state);
	}

	sealed class RotateCommand(RotateDirection rotateDirection) : ICommand
	{
		public static RotateCommand Parse(char command)
			=> new(command switch
			{
				'L' => RotateDirection.Left,
				'R' => RotateDirection.Right,
				_ => throw new InvalidDataException(),
			});

		private Direction Rotate(Direction direction)
		{
			var d = rotateDirection switch
			{
				RotateDirection.Left  => direction - 1,
				RotateDirection.Right => direction + 1,
				_ => throw new Exception()
			};
			if(d < 0) d = Direction.Down;
			else if(d > Direction.Down) d = Direction.Left;
			return d;
		}

		public State Execute(char[,] map, State state)
			=> state with { Direction = Rotate(state.Direction) };

		public override string ToString() => rotateDirection switch
		{
			RotateDirection.Left  => "L",
			RotateDirection.Right => "R",
			_ => throw new Exception(),
		};
	}

	protected static Offset GetOffset(Direction direction)
		=> direction switch
		{
			Direction.Left  => new Offset(DeltaX: -1),
			Direction.Up    => new Offset(DeltaY: -1),
			Direction.Right => new Offset(DeltaX:  1),
			Direction.Down  => new Offset(DeltaY:  1),
			_ => throw new Exception(),
		};

	protected static Position FindFirstCell(char[,] map, Position position, Offset offset)
	{
		while(map[position.X, position.Y] == ' ')
		{
			position += offset;
		}
		return position;
	}

	protected static bool IsWall(char[,] map, Position position)
		=> map[position.X, position.Y] == '#';

	sealed class MoveCommand(int length, IWrapper wrapper) : ICommand
	{
		public State Execute(char[,] map, State state)
		{
			var position  = state.Position;
			var direction = state.Direction;
			var offset    = GetOffset(direction);
			var count     = length;
			while(count > 0)
			{
				var next = position + offset;
				var wrap = !next.IsInside(map) || map[next.X, next.Y] == ' ';
				if(wrap)
				{
					var wrapped = wrapper.Wrap(state);
					if(IsWall(map, wrapped.Position)) break;
					position = wrapped.Position;
					if(direction != wrapped.Direction)
					{
						direction = wrapped.Direction;
						offset    = GetOffset(direction);
					}
				}
				else
				{
					if(IsWall(map, next)) break;
					position = next;
				}
				--count;
			}
			return new(position, direction);
		}

		public override string ToString() => length.ToString();
	}

	protected static char[,] LoadMap(TextReader reader)
	{
		var lines = new List<string>();
		var width = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) break;
			lines.Add(line);
			if(line.Length > width) width = line.Length;
		}
		var map = new char[width, lines.Count];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				map[x, y] = lines[y][x];
			}
			for(int x = lines[y].Length; x < width; ++x)
			{
				map[x, y] = ' ';
			}
		}
		return map;
	}

	protected static Position FindStart(char[,] map)
	{
		for(int x = 0; x < map.GetLength(0); ++x)
		{
			if(map[x, 0] == '.') return new(x, 0);
		}
		throw new InvalidDataException();
	}

	protected static List<ICommand> LoadCommands(TextReader reader, IWrapper wrapper)
	{
		var line = reader.ReadLine();
		if(line is null) throw new InvalidDataException();
		var commands = new List<ICommand>();
		var i = 0;
		while(i < line.Length)
		{
			if(char.IsAsciiLetter(line[i]))
			{
				commands.Add(RotateCommand.Parse(line[i++]));
				continue;
			}
			if(!char.IsAsciiDigit(line[i]))
			{
				throw new InvalidDataException();
			}
			var s = i;
			while(i < line.Length && char.IsAsciiDigit(line[i]))
			{
				++i;
			}
			commands.Add(new MoveCommand(int.Parse(line.AsSpan(s, i - s)), wrapper));
		}
		return commands;
	}

	protected static int GetPassword(Position position, Direction direction)
	{
		var directionScore = direction switch
		{
			Direction.Left => 2,
			Direction.Up => 3,
			Direction.Right => 0,
			Direction.Down => 1,
			_ => throw new Exception(),
		};
		return 1000 * (position.Y + 1)
			+     4 * (position.X + 1)
			+          directionScore;
	}
}

public class Day22SolutionPart1 : Day22Solution
{
	sealed class Wrapper(char[,] map) : IWrapper
	{
		public State Wrap(State state)
		{
			var position = state.Position;
			var offset   = GetOffset(state.Direction);
			var wrapPos  = new Position(-1, -1);
			if(offset.DeltaX != 0)
			{
				wrapPos = offset.DeltaX > 0
					? new(0, position.Y)
					: new(map.GetLength(0) - 1, state.Position.Y);
			}
			else if(offset.DeltaY != 0)
			{
				wrapPos = offset.DeltaY > 0
					? new(position.X, 0)
					: new(position.X, map.GetLength(1) - 1);
			}
			var next = FindFirstCell(map, wrapPos, offset);
			return state with { Position = next };
		}
	}

	public override string Process(TextReader reader)
	{
		var map       = LoadMap(reader);
		var commands  = LoadCommands(reader, new Wrapper(map));
		var start     = FindStart(map);
		var state     = new State(start, Direction.Right);
		foreach(var command in commands)
		{
			state = command.Execute(map, state);
		}
		return state.GetPassword().ToString();
	}
}

public class Day22SolutionPart2 : Day22Solution
{
	sealed class CubeVertex
	{
		public CubeVertex(int id)
		{
			Id = id;
		}

		public int Id { get; }

		public List<CubeSide> Sides { get; } = [];
	}

	sealed class CubeSide(int id,
		Position topLeft,
		Position topRight,
		Position bottomLeft,
		Position bottomRight)
	{
		public int Id { get; } = id;


		public Position? TopLeft { get; } = topLeft;

		public Position? TopRight { get; } = topRight;

		public Position? BottomLeft { get; } = bottomLeft;

		public Position? BottomRight { get; } = bottomRight;


		public CubeVertex? TopLeftVertex { get; set; }

		public CubeVertex? TopRightVertex { get; set; }

		public CubeVertex? BottomLeftVertex { get; set; }

		public CubeVertex? BottomRightVertex { get; set; }
	}

	sealed class Wrapper : IWrapper
	{
		//readonly record struct Edge(Position P1, Position P2);

		//readonly record struct Portal(Edge Edge, Direction Direction);

		//readonly record struct EdgeMap(Portal From, Portal To);

		//readonly record struct CubeSide(Edge Left, Edge Top, Edge Right, Edge Bottom);

		static int GetCubeSideLength(char[,] map)
		{
			return map.GetLength(0) / 4;
		}

		private readonly record struct X(int Side, Direction Edge);

		void CheckEdge(int[,] cube, Position position, Offset offset)
		{
			var adjacentSide = position + offset;
			if(offset.DeltaX != 0)
			{
				if(adjacentSide.IsInside(cube))
				{
					var adjacentId = cube[adjacentSide.X, adjacentSide.Y];
					if(adjacentId != 0)
					{
						// no wrapping needed
					}
					else
					{
						var up = adjacentSide with { Y = adjacentSide.Y - 1 };
						if(up.IsInside(cube) && cube[up.X, up.Y] != 0)
						{

						}
						var down = adjacentSide with { Y = adjacentSide.Y + 1 };
						if(down.IsInside(cube) && cube[down.X, down.Y] != 0)
						{

						}
					}
				}
				else
				{

				}
			}
			else if(offset.DeltaY != 0)
			{
			}
		}

		public Wrapper(char[,] map)
		{
			var cs = GetCubeSideLength(map);
			var mw = map.GetLength(0) / cs;
			var mh = map.GetLength(1) / cs;
			var cube = new CubeSide[mw, mh];

			int id = 0;
			for(int y = 0; y < mh; ++y)
			{
				for(int x = 0; x < mw; ++x)
				{
					var isEmpty = map[x * cs, y * cs] == ' ';
					if(!isEmpty)
					{
						var x0 = x * cs;
						var x1 = x0 + cs - 1;
						var y0 = y * cs;
						var y1 = y0 + cs - 1;
						cube[x, y] = new CubeSide(++id,
							topLeft:     new(x0, y0),
							topRight:    new(x1, y0),
							bottomLeft:  new(x0, y1),
							bottomRight: new(x1, y1));
					}
				}
			}

			if(id != 6) throw new InvalidDataException();

			id = 0;

			for(int y = 0; y < mh; ++y)
			{
				for(int x = 0; x < mw; ++x)
				{
					var side = cube[x, y];
					if(side is null) continue;
					if(side.TopLeftVertex is null)
					{
						var v = new CubeVertex(++id);
						side.TopLeftVertex = v;
						v.Sides.Add(side);

						var up = new Position(x, y - 1);
						if(up.IsInside(cube))
						{
							var u = cube[up.X, up.Y];
							if(u is not null)
							{
								u.BottomLeftVertex = v;
								v.Sides.Add(u);
							}
							else
							{
							}
						}
						else
						{

						}

						var lp = new Position(x - 1, y);
						if(lp.IsInside(cube))
						{
							var l = cube[up.X, up.Y];
							if(l is not null)
							{
								l.TopRightVertex = v;
								v.Sides.Add(l);
							}
							else
							{
							}
						}
					}
				}
			}

			if(id != 8) throw new InvalidDataException();

		}

		public State Wrap(State state)
		{
			throw new NotImplementedException();
		}
	}

	public override string Process(TextReader reader)
	{
		throw new NotImplementedException();

		var map       = LoadMap(reader);
		var commands  = LoadCommands(reader, new Wrapper(map));
		var position  = FindStart(map);
		var direction = Direction.Right;

		return GetPassword(position, direction).ToString();
	}
}
