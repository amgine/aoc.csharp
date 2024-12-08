namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/8"/></remarks>
[Name(@"Resonant Collinearity")]
public abstract class Day08Solution : Solution
{
	public sealed override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		return FindAntinodes(map, FindAntennaLocations(map)).Count.ToString();
	}

	private static Dictionary<char, List<Point2D>> FindAntennaLocations(char[,] map)
	{
		var antennaLocations = new Dictionary<char, List<Point2D>>();
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				var c = map[y, x];
				if(c == '.') continue;
				if(antennaLocations.TryGetValue(c, out var points))
				{
					points.Add(new(x, y));
				}
				else
				{
					antennaLocations.Add(c, [new(x, y)]);
				}
			}
		}
		return antennaLocations;
	}

	private HashSet<Point2D> FindAntinodes(char[,] map, Dictionary<char, List<Point2D>> antennaLocations)
	{
		var nn = new HashSet<Point2D>();
		foreach(var list in antennaLocations.Values)
		{
			for(int i = 0; i < list.Count - 1; ++i)
			{
				for(int j = i + 1; j < list.Count; ++j)
				{
					EmitAntinodes(nn, map, list[i], list[j]);
				}
			}
		}
		return nn;
	}

	protected abstract void EmitAntinodes(HashSet<Point2D> antinodes, char[,] map, Point2D p0, Point2D p1);
}

public sealed class Day08SolutionPart1 : Day08Solution
{
	protected override void EmitAntinodes(HashSet<Point2D> antinodes, char[,] map, Point2D p0, Point2D p1)
	{
		var v  = p1 - p0;
		var n0 = p0 - v;;
		var n1 = p1 + v;;
		if(n0.IsInside(map)) antinodes.Add(n0);
		if(n1.IsInside(map)) antinodes.Add(n1);
	}
}

public sealed class Day08SolutionPart2 : Day08Solution
{
	protected override void EmitAntinodes(HashSet<Point2D> antinodes, char[,] map, Point2D p0, Point2D p1)
	{
		antinodes.Add(p0);
		antinodes.Add(p1);
		var v  = p1 - p0;
		var n0 = p0 - v;
		while(n0.IsInside(map))
		{
			antinodes.Add(n0);
			n0 -= v;
		}
		var n1 = p1 + v;
		while(n1.IsInside(map))
		{
			antinodes.Add(n1);
			n1 += v;
		}
	}
}
