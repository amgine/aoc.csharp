namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/3"/></remarks>
[Name(@"Lobby")]
public abstract class Day03Solution : Solution
{
	protected abstract long GetMaxJoltage(ReadOnlySpan<char> bank);

	public override string Process(TextReader reader)
	{
		var sum = 0L;
		var banks = LoadInputAsListOfNonEmptyStrings(reader);
		foreach(var bank in banks)
		{
			sum += GetMaxJoltage(bank);
		}
		return sum.ToString();
	}
}

public sealed class Day03SolutionPart1 : Day03Solution
{
	protected override long GetMaxJoltage(ReadOnlySpan<char> bank)
	{
		var num = 0;
		for(int i = 0; i < bank.Length - 1; ++i)
		{
			for(int j = i + 1; j < bank.Length; ++j)
			{
				var n = (bank[i] - '0') * 10 + (bank[j] - '0');
				if(n > num) num = n;
			}
		}
		return num;
	}
}

public sealed class Day03SolutionPart2 : Day03Solution
{
	readonly record struct CacheKey(int Offset, int Count);

	static long Eval(ReadOnlySpan<char> span, int offset, int count, Dictionary<CacheKey, long> cache)
	{
		if(count <= 0) return 0;

		var key = new CacheKey(offset, count);
		if(cache.TryGetValue(key, out var value)) return value;

		var max = 0L;
		for(int i = offset; i <= span.Length - count; ++i)
		{
			var v = (long)(span[i] - '0');
			if(count > 1)
			{
				var c = count - 1;
				while(c-- > 0) v *= 10;
				v += Eval(span, i + 1, count - 1, cache);
			}
			if(v > max) max = v;
		}
		cache.Add(key, max);
		return max;
	}

	protected override long GetMaxJoltage(ReadOnlySpan<char> bank)
		=> Eval(bank, 0, 12, []);
}
