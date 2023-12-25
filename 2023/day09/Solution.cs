using System;
using System.Globalization;
using System.Numerics;

namespace AoC.Year2023;

[Name(@"Mirage Maintenance")]
public abstract class Day9Solution : Solution
{
	protected static T[] ParseValues<T>(string line) where T : IParsable<T>
		=> Array.ConvertAll(
			line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
			static s => T.Parse(s, CultureInfo.InvariantCulture));

	protected abstract T Extrapolate<T>(Span<T> values) where T : INumber<T>;

	public override string Process(TextReader reader)
	{
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			sum += Extrapolate(ParseValues<int>(line).AsSpan());
		}
		return sum.ToString();
	}
}

public sealed class Day9SolutionPart1 : Day9Solution
{
	static T Sum<T>(Span<T> values) where T : INumberBase<T>
	{
		var sum = T.Zero;
		foreach(var value in values) sum += value;
		return sum;
	}

	static bool FillDelta<T>(ReadOnlySpan<T> values, Span<T> delta)
		where T : INumber<T>
	{
		bool allEqual = true;
		for(int i = 0; i < values.Length - 1; ++i)
		{
			delta[i] = values[i + 1] - values[i];
			if(i > 0 && delta[i] != delta[i - 1]) allEqual = false;
		}
		return allEqual;
	}

	protected override T Extrapolate<T>(Span<T> values)
	{
		var part = values;
		var tail = 2;
		while(!FillDelta(part, part[..^1]))
		{
			part = part[..^1];
			++tail;
		}
		return Sum(values[^tail..]);
	}
}

public sealed class Day9SolutionPart2 : Day9Solution
{
	static T Sub<T>(Span<T> values) where T : INumberBase<T>
	{
		var result = values[^1];
		for(int i = values.Length - 2; i >= 0; --i)
		{
			result = values[i] - result;
		}
		return result;
	}

	static bool FillDelta<T>(ReadOnlySpan<T> values, Span<T> delta)
		where T : INumberBase<T>
	{
		bool allEqual = true;
		for(int i = values.Length - 1; i > 0; --i)
		{
			delta[i - 1] = values[i] - values[i - 1];
			if(i < values.Length - 1 && delta[i] != delta[i - 1]) allEqual = false;
		}
		return allEqual;
	}

	protected override T Extrapolate<T>(Span<T> values)
	{
		var part = values;
		var head = 2;
		while(!FillDelta(part, part[1..]))
		{
			part = part[1..];
			++head;
		}
		return Sub(values[..head]);
	}
}
