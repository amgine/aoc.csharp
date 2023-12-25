using System.Runtime.InteropServices;

namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/11"/></remarks>
[Name(@"Cosmic Expansion")]
public abstract class Day11Solution : Solution
{
	static List<Point2D> GetGalaxies(List<string> map)
	{
		var positions = new List<Point2D>();
		for(int y = 0; y < map.Count; ++y)
		{
			var line = map[y];
			for(int x = 0; x < line.Length; ++x)
			{
				if(line[x] == '#') positions.Add(new(x, y));
			}
		}
		return positions;
	}

	static List<int> GetEmptyRows(List<string> map)
	{
		var indices = new List<int>();
		for(int i = 0; i < map.Count; ++i)
		{
			if(map[i].All(static c => c == '.')) indices.Add(i);
		}
		return indices;
	}

	static List<int> GetEmptyColumns(List<string> map)
	{
		var indices = new List<int>();
		for(int x = 0; x < map[0].Length; ++x)
		{
			var allSpaces = true;
			for(int y = 0; y < map.Count; ++y)
			{
				if(map[y][x] != '.')
				{
					allSpaces = false;
					break;
				}
			}
			if(allSpaces) indices.Add(x);
		}
		return indices;
	}

	static long GetDistance(Point2D p1, Point2D p2,
		List<int> emptyColumns, List<int> emptyRows, long expansion)
	{
		static int FindLeftEdge(ReadOnlySpan<int> expanded, int edge)
		{
			for(int i = 0; i < expanded.Length; ++i)
			{
				if(expanded[i] >= edge) return i;
			}
			return expanded.Length;
		}

		static int FindRightEdge(ReadOnlySpan<int> expanded, int edge)
		{
			for(int i = expanded.Length - 1; i >= 0 ; --i)
			{
				if(expanded[i] <= edge) return i;
			}
			return -1;
		}

		static long GetExpandedSpace(int a, int b, List<int> expanded, long expansion)
		{
			var min = Math.Min(a, b) + 1;
			var max = Math.Max(a, b) - 1;
			if(min > max) return 0;

			var span = CollectionsMarshal.AsSpan(expanded);
			var e1 = FindLeftEdge (span, min);
			var e2 = FindRightEdge(span, max);

			return e2 >= e1 ? (e2 - e1 + 1) * (expansion - 1) : 0;
		}

		var expandedSpace = expansion > 1
			? GetExpandedSpace(p1.X, p2.X, emptyColumns, expansion) +
			  GetExpandedSpace(p1.Y, p2.Y, emptyRows,    expansion)
			: 0L;
		return expandedSpace + Geometry.ManhattanDistance(p1, p2);
	}

	protected static long Solve(List<string> map, long expansion)
	{
		var galaxies     = GetGalaxies(map);
		var emptyRows    = GetEmptyRows(map);
		var emptyColumns = GetEmptyColumns(map);

		var sum = 0L;
		for(int i = 0; i < galaxies.Count - 1; ++i)
		{
			var g1 = galaxies[i];
			for(int j = i + 1; j < galaxies.Count; ++j)
			{
				var g2 = galaxies[j];
				sum += GetDistance(g1, g2, emptyColumns, emptyRows, expansion);
			}
		}

		return sum;
	}
}

public sealed class Day11SolutionPart1 : Day11Solution
{
	public override string Process(TextReader reader)
		=> Solve(LoadInputAsListOfNonEmptyStrings(reader), expansion: 2).ToString();
}

public sealed class Day11SolutionPart2 : Day11Solution
{
	public override string Process(TextReader reader)
		=> Solve(LoadInputAsListOfNonEmptyStrings(reader), expansion: 1_000_000).ToString();
}
