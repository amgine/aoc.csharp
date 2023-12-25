namespace AoC.Year2021;

[Name(@"Binary Diagnostic")]
public abstract class Day03Solution : Solution
{
}

public sealed class Day03SolutionPart1 : Day03Solution
{
	static (int GammaRate, int EpsilonRate) GetRates(int[] freq, int count)
	{
		count /= 2;
		int gammaRate = 0;
		for(int i = 0; i < freq.Length; ++i)
		{
			if(freq[i] > count) gammaRate |= 1 << (freq.Length - i - 1);
		}
		var epsilonRate = ~gammaRate & ((1 << freq.Length) - 1);
		return (gammaRate, epsilonRate);
	}

	static int GetScore(int gammaRate, int epsilonRate)
		=> gammaRate * epsilonRate;

	public override string Process(TextReader reader)
	{
		var freq  = default(int[]);
		var count = 0;

		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			freq ??= new int[line.Length];
			for(int i = 0; i < line.Length; ++i)
			{
				if(line[i] == '1') ++freq[i];
			}
			++count;
		}

		if(freq is null) throw new InvalidDataException();

		var (gammaRate, epsilonRate) = GetRates(freq, count);
		return GetScore(gammaRate, epsilonRate).ToString();
	}
}

public sealed class Day03SolutionPart2 : Day03Solution
{
	static int ParseBinary(string line)
	{
		int value = 0;
		for(int i = 0; i < line.Length; ++i)
		{
			if(line[i] == '1') value |= 1 << (line.Length - i - 1);
		}
		return value;
	}

	static (int Ones, int Zeros) GetFreq(List<int> numbers, int mask)
	{
		var ones = 0;
		foreach(var num in numbers)
		{
			if((num & mask) != 0) ++ones;
		}
		return (ones, numbers.Count - ones);
	}

	static int GetO2(List<int> numbers, int digits)
	{
		var set = numbers.ToList();
		for(int d = digits - 1; d >= 0 && set.Count > 1; --d)
		{
			var mask = 1 << d;
			var (ones, zeros) = GetFreq(set, mask);
			var value = ones >= zeros ? mask : 0;
			set.RemoveAll(n => (n & mask) != value);
		}
		if(set.Count == 0) throw new InvalidDataException();
		return set[0];
	}

	static int GetCO2(List<int> numbers, int digits)
	{
		var set = numbers.ToList();
		for(int d = digits - 1; d >= 0 && set.Count > 1; --d)
		{
			var mask = 1 << d;
			var (ones, zeros) = GetFreq(set, mask);
			var value = zeros <= ones ? 0 : mask;
			set.RemoveAll(n => (n & mask) != value);
		}
		if(set.Count == 0) throw new InvalidDataException();
		return set[0];
	}

	public override string Process(TextReader reader)
	{
		var numbers = new List<int>();
		string? line;
		var digits = 0;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			numbers.Add(ParseBinary(line));
			digits = line.Length;
		}

		if(digits == 0) throw new InvalidDataException();

		return (GetO2(numbers, digits) * GetCO2(numbers, digits)).ToString();
	}
}
