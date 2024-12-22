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

	private static int GetPrice(long number) => (int)(number % 10);

	public override string Process(TextReader reader)
	{
		string? line;
		int key;
		var prices = new Dictionary<int, int>();
		var unique = new HashSet<int>();
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var number    = long.Parse(line);
			int prevPrice = GetPrice(number);
			unique.Clear();
			key = 0;
			for(int i = 0; i < Iterations; ++i)
			{
				var price = GetPrice(number = Next(number));
				key <<= 8;
				key  |= (byte)(sbyte)(price - prevPrice);
				prevPrice = price;
				if(i >= 3 && unique.Add(key))
				{
					Add(prices, key, price);
				}
			}
		}

		return prices.Values.Max().ToString();
	}
}
