namespace AoC.Year2021;

/// <remarks><a href="https://adventofcode.com/2021/day/1"/></remarks>
[Name(@"Sonar Sweep")]
public abstract class Day01Solution : Solution
{
	protected static int GetRequiredFuel(int mass) => mass / 3 - 2;
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var prev = 0;
		var sum = 0;
		var first = true;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var measurement = int.Parse(line);
			if(!first && measurement > prev) ++sum;
			first = false;
			prev = measurement;
		}
		return sum.ToString();
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	static bool IsIncrease(Span<int> span)
		=> span[^1] > span[0];

	static void Shift(Span<int> span)
	{
		for(int i = 0; i < span.Length - 1; ++i)
		{
			span[i] = span[i + 1];
		}
	}

	public override string Process(TextReader reader)
	{
		const int WindowSize = 3;

		var window = new int[WindowSize + 1];
		for(int i = 0; i < WindowSize; ++i)
		{
			window[i + 1] = int.Parse(reader.ReadLine() ?? throw new InvalidDataException());
		}
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			Shift(window);
			window[3] = int.Parse(line);
			if(IsIncrease(window)) ++sum;
		}
		return sum.ToString();
	}
}
