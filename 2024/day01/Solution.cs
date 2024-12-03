using System.Globalization;

namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/2"/></remarks>
[Name(@"Red-Nosed Reports")]
public abstract class Day02Solution : Solution
{
	protected struct SequenceState
	{
		private bool _increasing;
		private bool _decreasing;

		public bool IsValid(int curr, int next)
		{
			if(Math.Abs(curr - next) is < 1 or > 3) return false;
			if(next > curr)
			{
				if(_decreasing) return false;
				_increasing = true;
			}
			else if(next < curr)
			{
				if(_increasing) return false;
				_decreasing = true;
			}
			return true;
		}
	}

	protected virtual bool IsSafe(int[] report)
	{
		var state = new SequenceState();
		for(int i = 0; i < report.Length - 1; i++)
		{
			if(!state.IsValid(report[i], report[i + 1])) return false;
		}
		return true;
	}

	public sealed override string Process(TextReader reader)
	{
		var total = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line)) continue;
			var report = Array.ConvertAll(
				line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
				s => int.Parse(s, CultureInfo.InvariantCulture));
			if(IsSafe(report)) ++total;
		}
		return total.ToString();
	}
}

public sealed class Day02SolutionPart1 : Day02Solution
{
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	struct SkippingIterator(int[] report, int excludedIndex)
	{
		private int _i0;
		private int _i1;

		public bool MoveNext(out int a, out int b)
		{
			if(_i1 == 0)
			{
				(_i0, _i1) = excludedIndex switch
				{
					0 => (1, 2),
					1 => (0, 2),
					_ => (0, 1),
				};
			}
			else
			{
				_i0 = _i1;
				++_i1;
				if(_i1 == excludedIndex) ++_i1;
			}
			if(_i1 >= report.Length)
			{
				a = default;
				b = default;
				return false;
			}
			a = report[_i0];
			b = report[_i1];
			return true;
		}
	}

	static bool IsSafe(int[] report, int exclude)
	{
		var state = new SequenceState();
		var iter  = new SkippingIterator(report, exclude);
		while(iter.MoveNext(out var a, out var b))
		{
			if(!state.IsValid(a, b)) return false;
		}
		return true;
	}

	protected override bool IsSafe(int[] report)
	{
		if(base.IsSafe(report)) return true;

		for(int i = 0; i < report.Length; ++i)
		{
			if(IsSafe(report, i)) return true;
		}
		return false;
	}
}
