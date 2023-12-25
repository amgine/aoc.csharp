using System.Diagnostics.CodeAnalysis;

namespace AoC.Year2021;

[Name(@"Seven Segment Search")]
public abstract class Day08Solution : Solution
{
	protected readonly record struct Input(string[] Digits, string[] Display);

	protected static Input ParseInput(string line)
	{
		var pos = line.IndexOf('|');
		if(pos < 0) throw new InvalidDataException();

		var digits  = line[..pos].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		var display = line[(pos + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		return new(digits, display);
	}
}

public sealed class Day08SolutionPart1 : Day08Solution
{
	public override string Process(TextReader reader)
	{
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var input = ParseInput(line);
			foreach(var d in input.Display)
			{
				if(d.Length is 2 or 3 or 4 or 7)
				{
					++sum;
				}
			}
		}
		return sum.ToString();
	}
}

public sealed class Day08SolutionPart2 : Day08Solution
{
	/*
	  0:      1:      2:      3:      4:
	 aaaa    ....    aaaa    aaaa    ....
	b    c  .    c  .    c  .    c  b    c
	b    c  .    c  .    c  .    c  b    c
	 ....    ....    dddd    dddd    dddd
	e    f  .    f  e    .  .    f  .    f
	e    f  .    f  e    .  .    f  .    f
	 gggg    ....    gggg    gggg    ....

	  5:      6:      7:      8:      9:
	 aaaa    aaaa    aaaa    aaaa    aaaa
	b    .  b    .  .    c  b    c  b    c
	b    .  b    .  .    c  b    c  b    c
	 dddd    dddd    ....    dddd    dddd
	.    f  e    f  .    f  e    f  .    f
	.    f  e    f  .    f  e    f  .    f
	 gggg    gggg    ....    gggg    gggg
	 */

	static readonly string[] Digits =
		[
			/* 0 */ @"abcefg",
			/* 1 */ @"cf",
			/* 2 */ @"acdeg",
			/* 3 */ @"acdfg",
			/* 4 */ @"bcdf",
			/* 5 */ @"abdfg",
			/* 6 */ @"abdefg",
			/* 7 */ @"acf",
			/* 8 */ @"abcdefg",
			/* 9 */ @"abcdfg",
		];

	static int DecodeDisplay(string[] digits, Dictionary<string, int> map)
	{
		var num = 0;
		foreach(var d in digits)
		{
			num *= 10;
			num += map[d];
		}
		return num;
	}

	static Dictionary<string, int> MapDigits(Input input)
	{
		var unknown = new List<string>(input.Digits);

		if(unknown.Count != 10) throw new InvalidDataException();

		var md1 = Array.Find(input.Digits, s => s.Length == 2) ?? throw new InvalidDataException();
		var md7 = Array.Find(input.Digits, s => s.Length == 3) ?? throw new InvalidDataException();
		var md4 = Array.Find(input.Digits, s => s.Length == 4) ?? throw new InvalidDataException();
		var md8 = Array.Find(input.Digits, s => s.Length == 7) ?? throw new InvalidDataException();

		unknown.Remove(md1);
		unknown.Remove(md7);
		unknown.Remove(md4);
		unknown.Remove(md8);

		string? md6 = default;
		char c = '\0', f = '\0';
		foreach(var d in unknown)
		{
			if(d.Length != 6) continue;
			// 0, 9 contain both segments of 1,
			// but 6 is missing 'c' segment
			if(!d.Contains(md1[0]))
			{
				c = md1[0];
				f = md1[1];
				md6 = d;
				break;
			}
			if(!d.Contains(md1[1]))
			{
				c = md1[1];
				f = md1[0];
				md6 = d;
				break;
			}
		}

		unknown.Remove(md6 ?? throw new InvalidDataException());

		string? md2 = default;
		string? md3 = default;
		string? md5 = default;
		foreach(var d in unknown)
		{
			if(d.Length != 5) continue;
			if(!d.Contains(f))
			{
				md2 = d;
				continue;
			}
			if(!d.Contains(c))
			{
				md5 = d;
				continue;
			}
			md3 = d;
		}

		unknown.Remove(md2 ?? throw new InvalidDataException());
		unknown.Remove(md3 ?? throw new InvalidDataException());
		unknown.Remove(md5 ?? throw new InvalidDataException());

		string md0;
		string md9;
		if(md4.All(c => unknown[0].Contains(c))) // 9 contains 4
		{
			md9 = unknown[0];
			md0 = unknown[1];
		}
		else
		{
			md9 = unknown[1];
			md0 = unknown[0];
		}

		return new Dictionary<string, int>(capacity: 10, DigitComparer.Instance)
		{
			[md0] = 0,
			[md1] = 1,
			[md2] = 2,
			[md3] = 3,
			[md4] = 4,
			[md5] = 5,
			[md6] = 6,
			[md7] = 7,
			[md8] = 8,
			[md9] = 9,
		};
	}

	sealed class DigitComparer : IEqualityComparer<string>
	{
		public static readonly DigitComparer Instance = new();

		public bool Equals(string? x, string? y)
		{
			if(x is null) return y is null;
			if(y is null) return false;
			if(x.Length != y.Length) return false;

			for(int i = 0; i < x.Length; ++i)
			{
				if(!y.Contains(x[i])) return false;
			}
			return true;
		}

		public int GetHashCode([DisallowNull] string obj)
		{
			var sum = 0;
			for(int i = 0; i < obj.Length; ++i)
			{
				sum += obj[i];
			}
			return sum;
		}
	}

	static int DecodeDisplay(Input input)
	{
		var map = MapDigits(input);
		return DecodeDisplay(input.Display, map);
	}

	public override string Process(TextReader reader)
	{
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			sum += DecodeDisplay(ParseInput(line));
		}
		return sum.ToString();
	}
}
