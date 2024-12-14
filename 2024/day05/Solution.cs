namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/5"/></remarks>
[Name(@"Print Queue")]
public abstract class Day05Solution : Solution
{
	protected readonly record struct Dependency(int Page, int Before);

	protected sealed class Input
	{
		public HashSet<Dependency> Rules { get; } = [];

		public List<int[]> PrintQueues { get; } = [];
	}

	protected static bool IsValid(HashSet<Dependency> rules, int[] queue)
	{
		for(int i = 0; i < queue.Length - 1; ++i)
		{
			for(int j = i + 1; j < queue.Length; ++j)
			{
				if(rules.Contains(new(queue[j], queue[i])))
				{
					return false;
				}
			}
		}
		return true;
	}

	public sealed override string Process(TextReader reader)
	{
		var input = new Input();
		var deps = true;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line))
			{
				if(deps) deps = false;
				continue;
			}

			if(deps)
			{
				var i0 = line.IndexOf('|');
				if(i0 < 0) throw new InvalidDataException();
				input.Rules.Add(new(
					int.Parse(line.AsSpan(0, i0)),
					int.Parse(line.AsSpan(i0 + 1))));
			}
			else
			{
				input.PrintQueues.Add(Array.ConvertAll(line.Split(','), int.Parse));
			}
		}

		return Solve(input).ToString();
	}

	protected abstract int Solve(Input input);
}

public sealed class Day05SolutionPart1 : Day05Solution
{
	protected override int Solve(Input input)
	{
		var sum = 0;
		foreach(var queue in input.PrintQueues)
		{
			if(!IsValid(input.Rules, queue)) continue;
			sum += queue[queue.Length / 2];
		}
		return sum;
	}
}

public sealed class Day05SolutionPart2 : Day05Solution
{
	private static int[] MakeValid(HashSet<Dependency> rules, int[] queue)
	{
		var valid = (int[])queue.Clone();
		do
		{
			for(int i = 0; i < valid.Length - 1; ++i)
			{
				for(int j = i + 1; j < valid.Length; ++j)
				{
					if(rules.Contains(new(valid[j], valid[i])))
					{
						(valid[j], valid[i]) = (valid[i], valid[j]);
						break;
					}
				}
			}
		}
		while(!IsValid(rules, valid));

		return valid;
	}

	protected override int Solve(Input input)
	{
		var sum = 0;
		foreach(var order in input.PrintQueues)
		{
			if(IsValid(input.Rules, order)) continue;
			var valid = MakeValid(input.Rules, order);
			sum += valid[valid.Length / 2];
		}
		return sum;
	}
}
