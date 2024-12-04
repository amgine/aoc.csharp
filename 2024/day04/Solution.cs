namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/4"/></remarks>
[Name(@"Ceres Search")]
public abstract class Day04Solution : Solution
{
	protected static bool IsInside<T>(T[,] map, int y, int x)
		=> x >= 0 && y >= 0 && x < map.GetLength(1) && y < map.GetLength(0);

	protected abstract int Solve(char[,] map);

	public sealed override string Process(TextReader reader)
		=> Solve(LoadCharMap2D(reader)).ToString();
}

public sealed class Day04SolutionPart1 : Day04Solution
{
	const string Phrase = "XMAS";
	static readonly Vector2D[] Offsets =
		[
			new( 0,  1),
			new( 0, -1),
			new( 1,  0),
			new( 1,  1),
			new( 1, -1),
			new(-1,  0),
			new(-1,  1),
			new(-1, -1),
		];

	private static bool IsXmas(char[,] map, Point2D p, Vector2D offset)
	{
		for(int i = 0; i < Phrase.Length; ++i)
		{
			if(!IsInside(map, p.Y, p.X))   return false;
			if(map[p.Y, p.X] != Phrase[i]) return false;
			p += offset;
		}
		return true;
	}

	private static int Count(char[,] map, Point2D p)
	{
		var count = 0;
		foreach(var offset in Offsets)
		{
			if(IsXmas(map, p, offset)) ++count;
		}
		return count;
	}

	protected override int Solve(char[,] map)
	{
		var count = 0;
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				count += Count(map, new(x, y));
			}
		}
		return count;
	}
}

public sealed class Day04SolutionPart2 : Day04Solution
{
	private static bool IsXmas(char[,] map, int y, int x)
	{
		if(map[y, x] != 'A') return false;

		var d1
			 = map[y - 1, x - 1] == 'M' && map[y + 1, x + 1] == 'S'
			|| map[y - 1, x - 1] == 'S' && map[y + 1, x + 1] == 'M';

		var d2
			 = map[y - 1, x + 1] == 'M' && map[y + 1, x - 1] == 'S'
			|| map[y - 1, x + 1] == 'S' && map[y + 1, x - 1] == 'M';

		return d1 && d2;
	}

	protected override int Solve(char[,] map)
	{
		var count = 0;
		for(int y = 1; y < map.GetLength(0) - 1; ++y)
		{
			for(int x = 1; x < map.GetLength(1) - 1; ++x)
			{
				if(IsXmas(map, y, x)) ++count;
			}
		}
		return count;
	}
}
