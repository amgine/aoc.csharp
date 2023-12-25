namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/5"/></remarks>
[Name(@"Doesn't He Have Intern-Elves For This?")]
public abstract class Day05Solution : Solution
{
	protected abstract bool IsNice(ReadOnlySpan<char> value);

	public override string Process(TextReader reader)
		=> CountFromNonEmptyLines(reader, line => IsNice(line)).ToString();
}

public sealed class Day05SolutionPart1 : Day05Solution
{
	static int GetVowelsCount(ReadOnlySpan<char> value)
	{
		static bool IsVowel(char value)
			=> value is 'a' or 'e' or 'i' or 'o' or 'u';

		var count = 0;
		foreach(var c in value)
		{
			if(IsVowel(c)) ++count;
		}
		return count;
	}

	static bool ContainsSequence(ReadOnlySpan<char> value)
	{
		for(int i = 0; i < value.Length - 1; ++i)
		{
			if(value[i + 1] == value[i]) return true;
		}
		return false;
	}

	protected override bool IsNice(ReadOnlySpan<char> value)
	{
		if(GetVowelsCount(value) < 3) return false;
		if(!ContainsSequence(value)) return false;
		if(value.Contains(@"ab", StringComparison.Ordinal)) return false;
		if(value.Contains(@"cd", StringComparison.Ordinal)) return false;
		if(value.Contains(@"pq", StringComparison.Ordinal)) return false;
		if(value.Contains(@"xy", StringComparison.Ordinal)) return false;
		return true;
	}
}

public sealed class Day05SolutionPart2 : Day05Solution
{
	static bool HasRepeatingChar(ReadOnlySpan<char> value)
	{
		for(int i = 0; i < value.Length - 2; ++i)
		{
			if(value[i] == value[i + 2]) return true;
		}
		return false;
	}

	static bool HasRepeatingPair(ReadOnlySpan<char> value)
	{
		for(int i = 0; i < value.Length - 3; ++i)
		{
			for(int j = i + 2; j < value.Length - 1; ++j)
			{
				if(value[i] == value[j] && value[i + 1] == value[j + 1])
				{
					return true;
				}
			}
		}
		return false;
	}

	protected override bool IsNice(ReadOnlySpan<char> value)
		=> HasRepeatingChar(value)
		&& HasRepeatingPair(value);
}
