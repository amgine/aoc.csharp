namespace AoC.Year2023;

[Name(@"Point of Incidence")]
public abstract class Day13Solution : Solution
{
	protected static bool TryLoadPattern(TextReader reader, out List<string> pattern)
	{
		var lines = new List<string>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) break;
			lines.Add(line);
		}
		pattern = lines;
		return lines.Count != 0;
	}

	protected abstract bool IsVerticalReflection(List<string> pattern, int index);

	protected abstract bool IsHorizontalReflection(List<string> pattern, int index);

	private int GetScore(List<string> pattern)
	{
		for(int i = 0; i < pattern.Count - 1; ++i)
		{
			if(IsHorizontalReflection(pattern, i)) return (i + 1) * 100;
		}
		for(int i = 0; i < pattern[0].Length - 1; ++i)
		{
			if(IsVerticalReflection(pattern, i)) return i + 1;
		}
		throw new InvalidDataException("No reflection was found.");
	}

	public override string Process(TextReader reader)
	{
		var sum = 0L;
		while(TryLoadPattern(reader, out var pattern))
		{
			sum += GetScore(pattern);
		}
		return sum.ToString();
	}
}

public sealed class Day13SolutionPart1 : Day13Solution
{
	protected override bool IsVerticalReflection(List<string> pattern, int index)
	{
		for(int a = index, b = index + 1; a >= 0 && b < pattern[0].Length; --a, ++b)
		{
			for(int i = 0; i < pattern.Count; ++i)
			{
				if(pattern[i][a] != pattern[i][b]) return false;
			}
		}
		return true;
	}

	protected override bool IsHorizontalReflection(List<string> pattern, int index)
	{
		for(int a = index, b = index + 1; a >= 0 && b < pattern.Count; --a, ++b)
		{
			if(pattern[a] != pattern[b]) return false;
		}
		return true;
	}
}

public sealed class Day13SolutionPart2 : Day13Solution
{
	const int RequiredDiff = 1;

	protected override bool IsVerticalReflection(List<string> pattern, int index)
	{
		int diff = 0;
		for(int a = index, b = index + 1; a >= 0 && b < pattern[0].Length; --a, ++b)
		{
			for(int i = 0; i < pattern.Count; ++i)
			{
				if(pattern[i][a] != pattern[i][b])
				{
					++diff;
					if(diff > RequiredDiff) return false;
				}
			}
		}
		return diff == RequiredDiff;
	}

	protected override bool IsHorizontalReflection(List<string> pattern, int index)
	{
		int diff = 0;
		for(int a = index, b = index + 1; a >= 0 && b < pattern.Count; --a, ++b)
		{
			for(int i = 0; i < pattern[0].Length; ++i)
			{
				if(pattern[a][i] != pattern[b][i])
				{
					++diff;
					if(diff > RequiredDiff) return false;
				}
			}
		}
		return diff == RequiredDiff;
	}
}
