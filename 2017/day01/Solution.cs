namespace AoC.Year2017;

/// <remarks><a href="https://adventofcode.com/2017/day/1"/></remarks>
[Name(@"Inverse Captcha")]
public abstract class Day01Solution : Solution
{
	protected static int Solve(string line, int offset)
	{
		var sum = 0;
		for(int i = 0; i < line.Length; ++i)
		{
			var cur  = line[i];
			var next = line[(i + offset) % line.Length];
			if(cur == next) sum += line[i] - '0';
		}
		return sum;
	}
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		return Solve(line, 1).ToString();
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		return Solve(line, line.Length / 2).ToString();
	}
}
