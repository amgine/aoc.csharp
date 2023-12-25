namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/1"/></remarks>
[Name(@"Trebuchet?!")]
public abstract class Day01Solution : Solution
{
	protected const int NotDigit = -1;

	protected abstract int TryParseDigit(ReadOnlySpan<char> span);

	private int FindFirstDigit(ReadOnlySpan<char> line)
	{
		while(line.Length > 0)
		{
			var digit = TryParseDigit(line);
			if(digit != NotDigit) return digit;
			line = line[1..];
		}
		return NotDigit;
	}

	private int FindLastDigit(ReadOnlySpan<char> line)
	{
		for(int i = line.Length - 1; i >= 0; --i)
		{
			var digit = TryParseDigit(line[i..]);
			if(digit != NotDigit) return digit;
		}
		return NotDigit;
	}

	private int ParseLine(string line)
		=> FindFirstDigit(line) * 10 + FindLastDigit(line);

	public override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, ParseLine).ToString();
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	static int TryParseDigit(char c)
		=> char.IsAsciiDigit(c) ? c - '0' : NotDigit;

	protected override int TryParseDigit(ReadOnlySpan<char> span)
		=> TryParseDigit(span[0]);
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	private static readonly string[] Names =
		[
			"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
		];

	protected override int TryParseDigit(ReadOnlySpan<char> span)
	{
		if(char.IsAsciiDigit(span[0])) return span[0] - '0';
		for(int j = 0; j < Names.Length; ++j)
		{	
			if(Names[j].Length > span.Length) continue;
			if(span[..Names[j].Length].SequenceEqual(Names[j]))
			{
				return j + 1;
			}
		}
		return NotDigit;
	}
}
