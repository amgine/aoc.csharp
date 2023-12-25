
namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/3"/></remarks>
[Name(@"Toboggan Trajectory")]
public abstract class Day03Solution : Solution
{
	protected static int CountTrees(char[,] map, Point2D position, Vector2D offset)
	{
		int count = 0;
		while(position.Y < map.GetLength(0))
		{
			if(position.GetValue(map) == '#')
			{
				++count;
			}
			position += offset;
			if(position.X >= map.GetLength(1))
			{
				position = position with { X = position.X % map.GetLength(1) };
			}
		}
		return count;
	}
}

public sealed class Day03SolutionPart1 : Day03Solution
{
	public override string Process(TextReader reader)
	{
		var map      = LoadCharMap2D(reader);
		var position = new Point2D(0, 0);
		var offset   = new Vector2D(3, 1);
		return CountTrees(map, position, offset).ToString();
	}
}

public sealed class Day03SolutionPart2 : Day03Solution
{
	public override string Process(TextReader reader)
	{
		var map      = LoadCharMap2D(reader);
		var position = new Point2D(0, 0);
		var offsets = new[]
		{
			new Vector2D(1, 1),
			new Vector2D(3, 1),
			new Vector2D(5, 1),
			new Vector2D(7, 1),
			new Vector2D(1, 2),
		};

		var result = 1L;
		foreach(var offset in offsets)
		{
			result *= CountTrees(map, position, offset);
		}
		return result.ToString();
	}
}
