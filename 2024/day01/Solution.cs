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
			var i0 = line.IndexOf(' ');
			var s0 = line.AsSpan(0, i0);
			var s1 = line.AsSpan(i0 + 1).Trim();
			left.Add (int.Parse(s0, CultureInfo.InvariantCulture));
			right.Add(int.Parse(s1, CultureInfo.InvariantCulture));
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

		return left.Zip(right, static (a, b) => Math.Abs(a - b)).Sum();
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	private static Dictionary<T, int> GetCounts<T>(List<T> values)
		where T : notnull
	{
		var counts = new Dictionary<T, int>(capacity: values.Count);
		foreach(var value in values)
		{
			if(counts.TryGetValue(value, out var count))
			{
				counts[value] = count + 1;
			}
			else
			{
				counts.Add(value, 1);
			}
		}
		return counts;
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
