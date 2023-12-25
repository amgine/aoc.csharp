namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/8"/></remarks>
[Name(@"Matchsticks")]
public abstract class Day08Solution : Solution
{
}

public sealed class Day08SolutionPart1 : Day08Solution
{
	static int GetLength(string line)
	{
		var len = line.Length - 2;
		for(int i = 1; i < line.Length - 2; ++i)
		{
			if(line[i] == '\\')
			{
				switch(line[i + 1])
				{
					case '\\' or '"': --len; ++i; break;
					case 'x': len -= 3; i += 3; break;
				}
			}
		}
		return len;
	}

	public override string Process(TextReader reader)
	{
		int sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			sum += line.Length - GetLength(line);
		}
		return sum.ToString();
	}
}

public sealed class Day08SolutionPart2 : Day08Solution
{
	static int GetLength(string line)
	{
		var len = 2;
		foreach(var c in line)
		{
			switch(c)
			{
				case '\"' or '\\': len += 2; break;
				default: ++len; break;
			}
		}
		return len;
	}

	public override string Process(TextReader reader)
	{
		int sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			sum += GetLength(line) - line.Length;
		}
		return sum.ToString();
	}
}
