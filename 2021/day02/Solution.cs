namespace AoC.Year2021;

[Name(@"Dive!")]
public abstract class Day02Solution : Solution
{
	protected static Direction2D ParseDirection(string line)
	{
		if(line.StartsWith("forward")) return Direction2D.Right;
		if(line.StartsWith("up"))      return Direction2D.Up;
		if(line.StartsWith("down"))    return Direction2D.Down;
		throw new InvalidDataException();
	}

	protected readonly record struct Command(Direction2D Direction, int Count);

	protected static Command ParseCommand(string line)
	{
		var s = line.IndexOf(' ');
		var count = int.Parse(line.AsSpan(s + 1));
		return new(ParseDirection(line), count);
	}

	protected static int GetScore(Point2D position)
		=> position.X * position.Y;
}

public sealed class Day02SolutionPart1 : Day02Solution
{
	static Vector2D ParseOffset(string line)
	{
		var command = ParseCommand(line);
		return Vector2D.FromDirection(command.Direction) * command.Count;
	}

	public override string Process(TextReader reader)
	{
		var position = Point2D.Zero;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			position += ParseOffset(line);
		}
		return GetScore(position).ToString();
	}
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	public override string Process(TextReader reader)
	{
		var position = Point2D.Zero;
		var aim = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var command = ParseCommand(line);
			switch(command.Direction)
			{
				case Direction2D.Up:   aim -= command.Count; break;
				case Direction2D.Down: aim += command.Count; break;
				case Direction2D.Right:
					position += new Vector2D(command.Count, aim * command.Count);
					break;
				default: throw new ApplicationException($"Invalid command: {command}");
			}
		}
		return GetScore(position).ToString();
	}
}
