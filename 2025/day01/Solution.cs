using System.Globalization;

namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/1"/></remarks>
[Name(@"Secret Entrance")]
public abstract class Day01Solution : Solution
{
	protected const int InitialDial = 50;
	protected const int Steps = 100;

	static int ParseSign(char c)
		=> c switch
		{
			'L' => -1,
			'R' =>  1,
			_ => throw new InvalidDataException(),
		};

	static int ParseOffset(string line)
		=> ParseSign(line[0]) * int.Parse(line.AsSpan(1), NumberStyles.Integer, CultureInfo.InvariantCulture);

	protected static List<int> ReadInput(TextReader reader)
		=> LoadListFromNonEmptyStrings(reader, ParseOffset);

	public override string Process(TextReader reader)
		=> Solve(ReadInput(reader)).ToString(CultureInfo.InvariantCulture);

	private int Solve(List<int> offsets)
	{
		var dial = InitialDial;
		var password = 0;
		foreach(var offset in offsets)
		{
			password += Turn(ref dial, offset);
		}
		return password;
	}

	protected abstract int Turn(ref int dial, int offset);
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	protected override int Turn(ref int dial, int offset)
	{
		dial += offset;
		dial %= Steps;
		if(dial < 0) dial += Steps;
		return dial == 0 ? 1 : 0;
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	protected override int Turn(ref int dial, int offset)
	{
		var wasZero = dial == 0;
		dial += offset;
		if(dial == 0) return 1;

		if(dial > 0)
		{
			var count = dial / Steps;
			dial %= Steps;
			return count;
		}
		else
		{
			var count = wasZero ? 0 : 1;
			count -= dial / Steps;
			dial %= Steps;
			if(dial < 0) dial += Steps;
			return count;
		}
	}
}
