namespace AoC.Year2017;

/// <remarks><a href="https://adventofcode.com/2017/day/2"/></remarks>
[Name(@"Corruption Checksum")]
public abstract class Day02Solution : Solution
{
	protected static readonly char[] Separators = ['\t', ' '];

	public override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, GetLineChecksum).ToString();

	private int GetLineChecksum(string line)
	{
		var nums = Array.ConvertAll(line.Split(Separators), int.Parse);
		return GetLineChecksum(nums);
	}

	protected abstract int GetLineChecksum(int[] numbers);
}

public sealed class Day02SolutionPart1 : Day02Solution
{
	protected override int GetLineChecksum(int[] numbers)
		=> numbers.Max() - numbers.Min();
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	protected override int GetLineChecksum(int[] numbers)
	{
		for(int i = 0; i < numbers.Length - 1; ++i)
		{
			var n1 = numbers[i];
			for(int j = i + 1; j < numbers.Length; ++j)
			{
				var n2 = numbers[j];
				if((n1 % n2) == 0) return n1 / n2;
				if((n2 % n1) == 0) return n2 / n1;
			}
		}
		throw new InvalidDataException();
	}
}
