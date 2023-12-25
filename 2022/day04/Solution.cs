namespace AoC.Year2022;

[Name(@"Camp Cleanup")]
public abstract class Day04Solution : Solution
{
	protected readonly record struct Range(int Start, int End)
	{
		public bool Contains(Range other)
			=> other.Start >= Start && other.End <= End;

		public bool IntersectsWith(Range other)
			=> End >= other.Start && other.End >= Start;
	}

	protected static bool Intersects(Range a, Range b)
		=> a.End >= b.Start && b.End >= a.Start;

	protected static (Range, Range) ParsePair(string line)
	{
		var groups = line.Split(',');
		var r1 = groups[0].Split('-');
		var r2 = groups[1].Split('-');
		return (
			new(int.Parse(r1[0]), int.Parse(r1[1])),
			new(int.Parse(r2[0]), int.Parse(r2[1])));
	}

	protected abstract bool ShouldCount((Range, Range) pair);

	public override string Process(TextReader reader)
	{
		var count = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			if(ShouldCount(ParsePair(line))) ++count;
		}
		return count.ToString();
	}
}

public sealed class Day04SolutionPart1 : Day04Solution
{
	protected override bool ShouldCount((Range, Range) pair)
		=> pair.Item1.Contains(pair.Item2)
		|| pair.Item2.Contains(pair.Item1);
}

public sealed class Day04SolutionPart2 : Day04Solution
{
	protected override bool ShouldCount((Range, Range) pair)
		=> pair.Item1.IntersectsWith(pair.Item2);
}
