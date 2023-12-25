namespace AoC.Year2016;

/// <remarks><a href="https://adventofcode.com/2016/day/1"/></remarks>
[Name(@"No Time for a Taxicab")]
public abstract class Day01Solution : Solution
{
	protected record struct Position(int X, int Y)
	{
		public static readonly Position Zero = new(0, 0);
	}

	protected enum Direction { N, E, S, W, }

	protected static int GetDistance(Position p1, Position p2)
		=> Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

	protected static Direction RotateLeft(Direction direction)
	{
		--direction;
		if(direction < 0) direction = Direction.W;
		return direction;
	}

	protected static Direction RotateRight(Direction direction)
	{
		++direction;
		if(direction > Direction.W) direction = Direction.N;
		return direction;
	}

	protected static Position Move(Position position, Direction direction, int distance)
		=> direction switch
		{
			Direction.N => position with { Y = position.Y - distance },
			Direction.S => position with { Y = position.Y + distance },
			Direction.W => position with { X = position.X - distance },
			Direction.E => position with { X = position.X + distance },
			_ => throw new ArgumentException($"Unknown direction: {direction}", nameof(direction)),
		};
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var direction = Direction.N;
		var position  = Position.Zero;
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		var d = 0;
		for(int i = 0; i < line.Length; ++i)
		{
			var c = line[i];
			switch(c)
			{
				case ' ': continue;
				case ',':
					position = Move(position, direction, d);
					d = 0;
					continue;
				case 'L': direction = RotateLeft (direction); continue;
				case 'R': direction = RotateRight(direction); continue;
			}
			if(char.IsAsciiDigit(c))
			{
				d *= 10;
				d += c - '0';
				continue;
			}
			throw new InvalidDataException();
		}
		position = Move(position, direction, d);
		return GetDistance(position, Position.Zero).ToString();
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var direction = Direction.N;
		var position  = Position.Zero;
		var visited   = new HashSet<Position> { position };
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		var d = 0;
		for(int i = 0; i < line.Length; ++i)
		{
			var c = line[i];
			switch(c)
			{
				case ' ': continue;
				case ',':
					while(d != 0)
					{
						position = Move(position, direction, 1);
						if(!visited.Add(position))
						{
							return GetDistance(position, Position.Zero).ToString();
						}
						--d;
					}
					continue;
				case 'L': direction = RotateLeft (direction); continue;
				case 'R': direction = RotateRight(direction); continue;
			}
			if(char.IsAsciiDigit(c))
			{
				d *= 10;
				d += c - '0';
				continue;
			}
			throw new InvalidDataException();
		}
		while(d != 0)
		{
			position = Move(position, direction, 1);
			if(!visited.Add(position))
			{
				return GetDistance(position, Position.Zero).ToString();
			}
			--d;
		}
		throw new InvalidDataException();
	}
}
