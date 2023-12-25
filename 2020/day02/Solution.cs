namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/2"/></remarks>
[Name(@"Password Philosophy")]
public abstract class Day02Solution : Solution
{
	protected readonly record struct Policy(char Character, int Min, int Max);

	protected readonly record struct Input(Policy Policy, string Password);

	protected static Input ParseInput(string line)
	{
		var sep1 = line.IndexOf('-');
		var sep2 = line.IndexOf(' ', sep1);
		var sep3 = line.IndexOf(':', sep2 + 1);

		int min = int.Parse(line.AsSpan(0, sep1));
		int max = int.Parse(line.AsSpan(sep1 + 1, sep2 - sep1 - 1));
		var c = line[sep2 + 1];
		var password = line[(sep3 + 2)..];

		return new(new(c, min, max), password);
	}

	protected abstract bool Validate(Policy policy, ReadOnlySpan<char> password);

	public override string Process(TextReader reader)
	{
		var count = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var input = ParseInput(line);
			if(Validate(input.Policy, input.Password))
			{
				++count;
			}
		}
		return count.ToString();
	}
}

public sealed class Day02SolutionPart1 : Day02Solution
{
	protected override bool Validate(Policy policy, ReadOnlySpan<char> password)
	{
		var count = password.Count(policy.Character);
		return count >= policy.Min && count <= policy.Max;
	}
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	protected override bool Validate(Policy policy, ReadOnlySpan<char> password)
	{
		var c0 = password[policy.Min - 1];
		var c1 = password[policy.Max - 1];
		return (c0 != c1)
			&& (c0 == policy.Character || c1 == policy.Character);
	}
}
