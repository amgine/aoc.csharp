namespace AoC.Year2023;

[Name(@"Wait For It")]
public abstract class Day6Solution : Solution
{
	protected record struct Race(long Time, long Distance);

	private static int SolveQuadraticEquasion(long a, long b, long c, out double x1, out double x2)
	{
		var D = b * b - 4 * a * c;
		if(D < 0)
		{
			x1 = default;
			x2 = default;
			return 0;
		}
		var sqrt = Math.Sqrt(D);
		x1 = (-b + sqrt) / (2 * a);
		x2 = (-b - sqrt) / (2 * a);
		return D != 0 ? 2 : 1;
	}

	protected static long GetWaysToWin(in Race race)
	{
		// t * (race.Time - t) > race.Distance

		var a = -1;
		var c = -race.Distance;
		var b =  race.Time;
		var solutions = SolveQuadraticEquasion(a, b, c, out var x1, out var x2);
		if(solutions < 1) return 0;

		var min = (long)Math.Floor  (x1 + 1);
		var max = (long)Math.Ceiling(x2 - 1);

		return max - min + 1;
	}

	protected abstract Race[] ParseRaces(TextReader reader);

	public override string Process(TextReader reader)
	{
		var res = 0L;
		foreach(var race in ParseRaces(reader))
		{
			var ways = GetWaysToWin(race);
			if(ways > 0)
			{
				if(res == 0) res  = ways;
				else         res *= ways;
			}
		}
		return res.ToString();
	}
}

public sealed class Day6SolutionPart1 : Day6Solution
{
	protected override Race[] ParseRaces(TextReader reader)
	{
		var line1 = reader.ReadLine() ?? throw new InvalidDataException();
		var line2 = reader.ReadLine() ?? throw new InvalidDataException();
		var t = line1.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		var d = line2.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

		var races = new Race[d.Length - 1];
		for(int i = 0; i < races.Length; ++i)
		{
			races[i] = new(long.Parse(t[i + 1]), long.Parse(d[i + 1]));
		}
		return races;
	}
}

public sealed class Day6SolutionPart2 : Day6Solution
{
	private static long ParseNumber(string line)
	{
		static Stack<int> GetDigits(string line)
		{
			int index = line.IndexOf(':') + 1;
			var digits = new Stack<int>(capacity: line.Length - index);
			for(int i = index; i < line.Length; ++i)
			{
				if(char.IsAsciiDigit(line[i])) digits.Push(line[i] - '0');
			}
			return digits;
		}

		static long ToDecimalNumber(Stack<int> digits)
		{
			var res = 0L;
			var m = 1L;
			while(digits.Count > 0)
			{
				res += digits.Pop() * m;
				m   *= 10L;
			}
			return res;
		}

		return ToDecimalNumber(GetDigits(line));
	}

	protected override Race[] ParseRaces(TextReader reader)
	{
		var line1 = reader.ReadLine() ?? throw new InvalidDataException();
		var line2 = reader.ReadLine() ?? throw new InvalidDataException();
		return [new Race(ParseNumber(line1), ParseNumber(line2))];
	}
}
