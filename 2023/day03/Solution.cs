namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/3"/></remarks>
[Name("Gear Ratios")]
public abstract class Day3Solution : Solution
{
	protected static bool IsPartNumber(List<string> lines, int i, int s, int e)
	{
		static bool IsSymbol(char c)
			=> c is not '.' && !char.IsAsciiDigit(c);

		static bool Check(ReadOnlySpan<char> line)
		{
			foreach(var c in line)
			{
				if(IsSymbol(c)) return true;
			}
			return false;
		}

		static ReadOnlySpan<char> GetCheckRange(ReadOnlySpan<char> line, int s, int e)
		{
			if(s > 0) --s;
			if(e < line.Length - 1) ++e;
			return line.Slice(s, e - s + 1);
		}

		if(i > 0)
		{
			var prev = lines[i - 1];
			if(Check(GetCheckRange(prev, s, e))) return true;
		}

		if(i < lines.Count - 1)
		{
			var next = lines[i + 1];
			if(Check(GetCheckRange(next, s, e))) return true;
		}

		var cur = lines[i];
		if(s > 0 && IsSymbol(cur[s - 1])) return true;
		if(e < cur.Length - 1 && IsSymbol(cur[e + 1])) return true;

		return false;
	}
}

public sealed class Day3SolutionPart1 : Day3Solution
{
	static IEnumerable<int> GetPartNumbers(List<string> lines, int lineIndex)
	{
		var line = lines[lineIndex];
		var s = -1;
		for(int i = 0; i < line.Length; ++i)
		{
			if(char.IsAsciiDigit(line[i]))
			{
				if(s == -1) s = i;
				continue;
			}
			if(s == -1) continue;
			var e = i - 1;
			if(IsPartNumber(lines, lineIndex, s, e))
			{
				yield return int.Parse(line.AsSpan(s, e - s + 1));
			}
			s = -1;
		}
		if(s != -1)
		{
			var e = line.Length - 1;
			if(IsPartNumber(lines, lineIndex, s, e))
			{
				yield return int.Parse(line.AsSpan(s, e - s + 1));
			}
		}
	}

	public override string Process(TextReader reader)
	{
		var lines = LoadInputAsListOfNonEmptyStrings(reader);
		int sum = 0;
		for(int i = 0; i < lines.Count; ++i)
		{
			sum += GetPartNumbers(lines, i).Sum();
		}
		return sum.ToString();
	}
}

public sealed class Day3SolutionPart2 : Day3Solution
{
	static bool IsGear(char c) => c is '*';

	readonly struct GearPower(int value, int count = 1)
	{
		public readonly int Count = count;

		public readonly int Value = value;

		public GearPower Add(int value) => new(value * Value, Count + 1);
	}

	static void MarkGearNumbers(List<string> lines, Dictionary<Point2D, GearPower> powers, int value, int i, int s, int e)
	{
		static void Add(int lineIndex, int charIndex, Dictionary<Point2D, GearPower> powers, int value)
		{
			var pos = new Point2D(charIndex, lineIndex);
			if(powers.TryGetValue(pos, out var power))
			{
				powers[pos] = power.Add(value);
			}
			else
			{
				powers.Add(pos, new(value));
			}
		}

		static void Mark(int lineIndex, Dictionary<Point2D, GearPower> powers, int value, string line, int s, int e)
		{
			if(s > 0) --s;
			if(e < line.Length - 1) ++e;
			for(int i = s; i <= e; ++i)
			{
				if(IsGear(line[i])) Add(lineIndex, i, powers, value);
			}
		}

		if(i > 0)
		{
			Mark(i - 1, powers, value, lines[i - 1], s, e);
		}

		if(i < lines.Count - 1)
		{
			Mark(i + 1, powers, value, lines[i + 1], s, e);
		}

		var cur = lines[i];
		if(s > 0 && IsGear(cur[s - 1]))
		{
			Add(i, s - 1, powers, value);
		}
		if(e < cur.Length - 1 && IsGear(cur[e + 1]))
		{
			Add(i, e + 1, powers, value);
		}
	}

	public override string Process(TextReader reader)
	{
		var lines  = LoadInputAsListOfNonEmptyStrings(reader);
		var powers = new Dictionary<Point2D, GearPower>();

		for(int lineIndex = 0; lineIndex < lines.Count; ++lineIndex)
		{
			var line = lines[lineIndex];
			var s = -1;
			var value = 0;
			for(int i = 0; i < line.Length; ++i)
			{
				if(char.IsAsciiDigit(line[i]))
				{
					if(s == -1) s = i;
					value *= 10;
					value += line[i] - '0';
					continue;
				}
				if(s == -1) continue;
				var e = i - 1;
				MarkGearNumbers(lines, powers, value, lineIndex, s, e);
				value = 0;
				s = -1;
			}
			if(s != -1)
			{
				var e = line.Length - 1;
				MarkGearNumbers(lines, powers, value, lineIndex, s, e);
			}
		}

		long sum = 0;
		foreach(var power in powers.Values)
		{
			if(power.Count == 2)
			{
				sum += power.Value;
			}
		}

		return sum.ToString();
	}
}
