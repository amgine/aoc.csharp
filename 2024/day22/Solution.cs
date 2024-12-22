namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/22"/></remarks>
[Name(@"Monkey Market")]
public abstract class Day22Solution : Solution
{
	protected const int Iterations = 2000;

	protected static long Next(long number)
	{
		number = Prune(Mix(number, number * 64));
		number = Prune(Mix(number, number / 32));
		number = Prune(Mix(number, number * 2048));
		return number;
	}

	static long Mix(long number,  long value)
		=> number ^ value;

	static long Prune(long number)
		=> number % 16777216;
}

public sealed class Day22SolutionPart1 : Day22Solution
{
	public override string Process(TextReader reader)
	{
		var sum = 0L;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var number = long.Parse(line);
			for(int i = 0; i < Iterations; ++i)
			{
				number = Next(number);
			}
			sum += number;
		}
		return sum.ToString();
	}
}

public sealed class Day22SolutionPart2 : Day22Solution
{
	static int MakeKey(ReadOnlySpan<sbyte> diffs)
		=> ((int)((byte)diffs[0]) << 24)
		 | ((int)((byte)diffs[1]) << 16)
		 | ((int)((byte)diffs[2]) <<  8)
		 | ((int)((byte)diffs[3]) <<  0);

	private static void Add<TKey, TValue>(Dictionary<TKey, TValue> prices, TKey key, TValue value)
		where TKey   : notnull
		where TValue : System.Numerics.IAdditionOperators<TValue, TValue, TValue>
	{
		if(prices.TryGetValue(key, out var p))
		{
			prices[key] = p + value;
		}
		else
		{
			prices.Add(key, value);
		}
	}

	private static void AddDiff<T>(Span<T> diffs, T diff)
	{
		for(int d = 0; d < diffs.Length - 1; ++d)
		{
			diffs[d] = diffs[d + 1];
		}
		diffs[^1] = diff;
	}

	private static int GetPrice(long number) => (int)(number % 10);

	public override string Process(TextReader reader)
	{
		Span<sbyte> diffs = stackalloc sbyte[4];
		string? line;

		var prices = new Dictionary<int, int>();
		var unique = new HashSet<int>();
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var number    = long.Parse(line);
			int prevPrice = GetPrice(number);
			unique.Clear();
			diffs.Clear();
			for(int i = 0; i < Iterations; ++i)
			{
				number    = Next(number);
				var price = GetPrice(number);
				AddDiff(diffs, (sbyte)(price - prevPrice));
				prevPrice = price;
				if(i >= 3)
				{
					var key = MakeKey(diffs);
					if(unique.Add(key))
					{
						Add(prices, key, price);
					}
				}
			}
		}

		return prices.Values.Max().ToString();
	}
}
