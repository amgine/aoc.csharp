namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/1"/></remarks>
[Name(@"Report Repair")]
public abstract class Day01Solution : Solution
{
	protected static List<int> LoadValues(TextReader reader)
	{
		var list = new List<int>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			list.Add(int.Parse(line));
		}
		return list;
	}
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var values = LoadValues(reader);
		for(int i = 0; i < values.Count - 1; ++i)
		{
			for(int j = i + 1; j < values.Count; ++j)
			{
				if(values[i] + values[j] == 2020)
				{
					return (values[i] * values[j]).ToString();
				}
			}
		}
		throw new InvalidDataException();
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var values = LoadValues(reader);
		for(int i = 0; i < values.Count - 2; ++i)
		{
			for(int j = i + 1; j < values.Count - 1; ++j)
			{
				for(int k = j + 1; k < values.Count; ++k)
				{
					if(values[i] + values[j] + values[k] == 2020)
					{
						return ((long)values[i] * (long)values[j] * (long)values[k]).ToString();
					}
				}
			}
		}
		throw new InvalidDataException();
	}
}
