namespace AoC.Year2022;

[Name(@"Rucksack Reorganization")]
public abstract class Day03Solution : Solution
{
	protected static int GetPriority(char item)
		=> char.IsAsciiLetterUpper(item)
			? item - 'A' + 27
			: item - 'a' +  1;
}

public class Day03SolutionPart1 : Day03Solution
{
	static int GetPriority(ReadOnlySpan<char> content)
	{
		if((content.Length & 1) == 1)
		{
			throw new ArgumentException("Must have even count of characters.", nameof(content));
		}

		var split = content.Length / 2;
		var part1 = content[..split];
		var part2 = content[split..];

		foreach(var c in part1)
		{
			if(part2.Contains(c)) return GetPriority(c);
		}
		throw new InvalidDataException("Must have exactly 1 misplaced item.");
	}

	public override string Process(TextReader reader)
	{
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			sum += GetPriority(line);
		}
		return sum.ToString();
	}
}

public class Day03SolutionPart2 : Day03Solution
{
	private static char FindMatch(ReadOnlySpan<char> content1, ReadOnlySpan<char> content2, ReadOnlySpan<char> content3)
	{
		foreach(var c in content1)
		{
			if(content2.Contains(c) && content3.Contains(c)) return c;
		}
		foreach(var c in content2)
		{
			if(content1.Contains(c) && content3.Contains(c)) return c;
		}
		foreach(var c in content3)
		{
			if(content1.Contains(c) && content2.Contains(c)) return c;
		}
		throw new InvalidDataException();
	}

	public override string Process(TextReader reader)
	{
		var sum = 0;
		string? line0;
		while((line0 = reader.ReadLine()) is not null)
		{
			var line1 = reader.ReadLine() ?? throw new InvalidDataException();
			var line2 = reader.ReadLine() ?? throw new InvalidDataException();
			sum += GetPriority(FindMatch(line0, line1, line2));
		}
		return sum.ToString();
	}
}
