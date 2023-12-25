namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/1"/></remarks>
[Name(@"Not Quite Lisp")]
public abstract class Day01Solution : Solution
{
	protected static int Value(char symbol)
		=> symbol switch
		{
			'(' =>  1,
			')' => -1,
			_ => throw new InvalidDataException(),
		};
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, static line => line.Sum(Value)).ToString();
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		var floor = 0;
		for(int i = 0; i < line.Length; ++i)
		{
			floor += Value(line[i]);
			if(floor == -1) return (i + 1).ToString();
		}
		throw new InvalidDataException();
	}
}
