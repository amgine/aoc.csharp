using System.Globalization;

namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/1"/></remarks>
[Name(@"Secret Entrance")]
public abstract class Day01Solution : Solution
{
	protected const int InitialDial = 50;

	protected static List<int> ReadInput(TextReader reader)
	{
		var offsets = new List<int>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(line)) continue;
			var sign = line[0] switch
			{
				'L' => -1,
				'R' =>  1,
				_ => throw new InvalidDataException(),
			};
			var value = int.Parse(line.AsSpan(1), NumberStyles.Integer, CultureInfo.InvariantCulture);
			offsets.Add(sign * value);
		}
		return offsets;
	}

	public override string Process(TextReader reader)
		=> Solve(ReadInput(reader)).ToString(CultureInfo.InvariantCulture);

	protected abstract int Solve(List<int> offsets);
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	protected override int Solve(List<int> offsets)
	{
		var password = 0;
		var dial = InitialDial;
		foreach(var offset in offsets)
		{
			dial += offset;
			while(dial < 0) dial += 100;
			dial %= 100;
			if(dial == 0) ++password;
		}
		return password;
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	protected override int Solve(List<int> offsets)
	{
		var password = 0;
		var dial = InitialDial;
		foreach(var offset in offsets)
		{
			var wasZero = dial == 0;
			dial += offset;
			if(dial == 0)
			{
				++password;
			}
			else if(dial > 0)
			{
				password += dial / 100;
				dial %= 100;
			}
			else
			{
				if(!wasZero) ++password;
				password -= dial / 100;
				while(dial < 0) dial += 100;
			}
		}
		return password;
	}
}
