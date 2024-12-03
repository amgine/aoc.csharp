using System.Globalization;

namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/1"/></remarks>
[Name(@"Historian Hysteria")]
public abstract class Day01Solution : Solution
{
	public sealed override string Process(TextReader reader)
	{
		var left  = new List<int>();
		var right = new List<int>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line)) continue;
			var p = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			left.Add(int.Parse(p[0], CultureInfo.InvariantCulture));
			right.Add(int.Parse(p[1], CultureInfo.InvariantCulture));
		}

		return Solve(left, right).ToString(CultureInfo.InvariantCulture);
	}

	protected abstract long Solve(List<int> left, List<int> right);
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	protected override long Solve(List<int> left, List<int> right)
	{
		left.Sort();
		right.Sort();

		var sum = 0L;
		for(int i = 0; i < left.Count; ++i)
		{
			sum += Math.Abs(left[i] - right[i]);
		}
		return sum;
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	private static Dictionary<int, int> GetCounts(List<int> right)
	{
		var d = new Dictionary<int, int>(capacity: right.Count);
		foreach(var n in right)
		{
			if(d.TryGetValue(n, out var count))
			{
				d[n] = count + 1;
			}
			else
			{
				d.Add(n, 1);
			}
		}
		return d;
	}

	protected override long Solve(List<int> left, List<int> right)
	{
		var counts = GetCounts(right);
		var sum = 0L;
		foreach(var n in left)
		{
			if(counts.TryGetValue(n, out var count))
			{
				sum += count * n;
			}
		}
		return sum;
	}
}
