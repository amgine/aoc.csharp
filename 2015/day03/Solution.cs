namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/3"/></remarks>
[Name(@"Perfectly Spherical Houses in a Vacuum")]
public abstract class Day03Solution : Solution
{
	protected static Vector2D GetOffset(char value)
		=> value switch
		{
			'^' => Vector2D.Up,
			'v' => Vector2D.Down,
			'<' => Vector2D.Left,
			'>' => Vector2D.Right,
			_   => throw new InvalidDataException($"Invalid direction: {value}"),
		};
}

public sealed class Day03SolutionPart1 : Day03Solution
{
	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		var position = Point2D.Zero;
		var visits = new HashSet<Point2D>() { position };
		foreach(var value in line)
		{
			visits.Add(position += GetOffset(value));
		}
		return visits.Count.ToString();
	}
}

public sealed class Day03SolutionPart2 : Day03Solution
{
	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		var visits = new HashSet<Point2D>() { Point2D.Zero };

		Span<Point2D> positions =
		[
			Point2D.Zero,
			Point2D.Zero,
		];

		var current = 1;
		foreach(var value in line)
		{
			visits.Add(positions[current ^= 1] += GetOffset(value));
		}
		return visits.Count.ToString();
	}
}
