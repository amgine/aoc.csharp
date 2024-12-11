namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/11"/></remarks>
[Name(@"Plutonian Pebbles")]
public abstract class Day11Solution(int iterations) : Solution
{
	static bool TrySplit(long v, out long v0, out long v1)
	{
		var x = v;
		var d = 0;
		do
		{
			x /= 10;
			++d;
		}
		while(x != 0);

		if((d & 1) == 0)
		{
			v0 = v;
			v1 = 0;
			d >>= 1;
			var k = 1;
			do
			{
				v1 += k * (v0 % 10);
				v0 /= 10;
				k  *= 10;
				--d;
			}
			while(d != 0);
			return true;
		}
		v0 = default;
		v1 = default;
		return false;
	}

	static long CountSplits(Dictionary<(long, int), long> cache, long v, int iteration, int total)
	{
		if(cache.TryGetValue((v, iteration), out var c)) return c;

		long s;
		if(iteration == total)
		{
			s = 0;
		}
		else if(v == 0)
		{
			s = CountSplits(cache, 1, iteration + 1, total);
		}
		else if(TrySplit(v, out var v0, out var v1))
		{
			s = CountSplits(cache, v0, iteration + 1, total)
			  + CountSplits(cache, v1, iteration + 1, total)
			  + 1;
		}
		else
		{
			s = CountSplits(cache, v * 2024, iteration + 1, total);
		}
		cache.TryAdd((v, iteration), s);
		return s;
	}

	public sealed override string Process(TextReader reader)
	{
		var stones = Array.ConvertAll((reader.ReadLine() ?? throw new InvalidDataException()).Split(' '), long.Parse);
		var sum    = stones.LongLength;
		var cache  = new Dictionary<(long, int), long>();
		foreach(var value in stones)
		{
			sum += CountSplits(cache, value, 0, iterations);
		}
		return sum.ToString();
	}
}

public sealed class Day11SolutionPart1 : Day11Solution
{
	public Day11SolutionPart1() : base(iterations: 25) { }
}

public sealed class Day11SolutionPart2 : Day11Solution
{
	public Day11SolutionPart2() : base(iterations: 75) { }
}
