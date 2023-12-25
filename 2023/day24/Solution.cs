using System.Numerics;

namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/24"/></remarks>
[Name(@"Never Tell Me The Odds")]
public abstract class Day24Solution : Solution
{
	public static Hailstone ParseHailstone(string line)
	{
		var sep = line.IndexOf('@');
		if(sep < 0) throw new InvalidDataException($"Invalid hailstone definition: {line}");
		return new(
			Parsers.ParsePoint3D<long>(line.AsSpan(0, sep)),
			Parsers.ParseVector3D  <long>(line.AsSpan(sep + 1)));
	}

	public static bool ProjectionsIntersect2D(in Hailstone s1, in Hailstone s2, out Point2D<double> intersection)
	{
		static Ray2D<BigInteger> ToRay2D(in Hailstone stone) => new(
			new(stone.Position.X,      stone.Position.Y),
			new(stone.Velocity.DeltaX, stone.Velocity.DeltaY));

		return Geometry.TryGetIntersection(ToRay2D(s1), ToRay2D(s2), out intersection);
	}

	static bool SatisfiesMinMax(in Point2D<double> p, long min, long max)
		=> p.X >= min && p.X <= max && p.Y >= min && p.Y <= max;

	public static int CountIntersectionsInArea(TextReader reader, long min, long max)
	{
		var stones = LoadListFromNonEmptyStrings(reader, ParseHailstone);

		var count = 0;
		for(int i = 0; i < stones.Count - 1; ++i)
		{
			var a = stones[i];
			for(int j = i + 1; j < stones.Count; ++j)
			{
				var b = stones[j];
				if(ProjectionsIntersect2D(a, b, out var p) && SatisfiesMinMax(p, min, max))
				{
					++count;
				}
			}
		}
		return count;
	}
}

public sealed class Day24SolutionPart1 : Day24Solution
{
	const long Min = 200000000000000;
	const long Max = 400000000000000;

	public override string Process(TextReader reader)
		=> CountIntersectionsInArea(reader, Min, Max).ToString();
}

public sealed class Day24SolutionPart2 : Day24Solution
{
	public override string Process(TextReader reader)
	{
		var stones = LoadListFromNonEmptyStrings(reader, ParseHailstone);
		return Z3Solver.SolvePart2(stones).ToString();
	}
}
