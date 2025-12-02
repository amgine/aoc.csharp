namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/2"/></remarks>
[Name(@"X")]
public abstract class Day02Solution : Solution
{
	protected readonly record struct Range(long Minimum, long Maximum)
	{
		public static Range Parse(string r)
		{
			var id = r.IndexOf('-');
			var min = long.Parse(r.AsSpan(0, id));
			var max = long.Parse(r.AsSpan(id + 1));
			return new(min, max);
		}
	}

	protected abstract bool IsInvalid(long n);

	public override string Process(TextReader reader)
	{
		var sum = 0L;
		var ranges = Array.ConvertAll(reader.ReadLine()!.Split(','), Range.Parse);
		foreach(var range in ranges)
		{
			for(long id = range.Minimum; id <= range.Maximum; ++id)
			{
				if(IsInvalid(id)) sum += id;
			}
		}
		return sum.ToString();
	}

	protected static int GetDigits(long value, Span<byte> digits)
	{
		if(value == 0)
		{
			digits[0] = 0;
			return 1;
		}
		var offset = 0;
		do
		{
			digits[offset++] = (byte)(value % 10);
			value /= 10;
		}
		while(value != 0);
		return offset;
	}
}

public sealed class Day02SolutionPart1 : Day02Solution
{
	protected override bool IsInvalid(long n)
	{
		Span<byte> syms = stackalloc byte[20];
		var length = GetDigits(n, syms);
		if(length <= 1) return false;
		if((length & 1) != 0) return false;
		for(int i = 0; i < length / 2; ++i)
		{
			if(syms[i] != syms[i + length / 2]) return false;
		}
		return true;
	}
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	protected override bool IsInvalid(long n)
	{
		Span<byte> syms = stackalloc byte[20];
		var length = GetDigits(n, syms);
		if(length <= 1) return false;
		for(int sequenceLength = 1; sequenceLength <= length / 2; ++sequenceLength)
		{
			if((length % sequenceLength) != 0) continue;
			var valid = false;
			for(int i = sequenceLength; i < length; ++i)
			{
				if(syms[i] != syms[i % sequenceLength])
				{
					valid = true;
					break;
				}
			}
			if(!valid) return true;
		}
		return false;
	}
}
