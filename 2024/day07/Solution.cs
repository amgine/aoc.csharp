namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/7"/></remarks>
[Name(@"Bridge Repair")]
public abstract class Day07Solution(int operatorsCount) : Solution
{
	readonly record struct Equation(long Result, long[] Args)
	{
		public static Equation Parse(string line)
		{
			var i = line.IndexOf(':');
			var result = long.Parse(line.AsSpan(0, i));
			var args = Array.ConvertAll(
				line[(i + 1)..].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries),
				long.Parse);
			return new Equation(result, args);
		}
	}

	enum Operator
	{
		Add,
		Multiply,
		Concat,
	}

	static long Concat(long a, long b)
	{
		var d = 1L;
		var x = b;
		x /= 10;
		while(x != 0)
		{
			d *= 10;
			x /= 10;
		}
		while(d != 0)
		{
			var digit = b / d;
			b -= digit * d;
			d /= 10;
			a = a * 10 + digit;
		}
		return a;
	}

	static long Calc(long a, long b, Operator op)
		=> op switch
		{
			Operator.Add      => a + b,
			Operator.Multiply => a * b,
			Operator.Concat   => Concat(a, b),
			_ => throw new ArgumentException($"Unknown operator: {op}", nameof(op)),
		};

	static long Calc(long[] args, ReadOnlySpan<Operator> ops)
	{
		long aggregated = args[0];
		for(int i = 0; i < ops.Length; ++i)
		{
			aggregated = Calc(aggregated, args[i + 1], ops[i]);
		}
		return aggregated;
	}

	static bool Next(Span<Operator> ops, int count)
	{
		for(int i = 0; i < ops.Length; ++i)
		{
			++ops[i];
			if(ops[i] < (Operator)count) return true;
			ops[i] = default;
		}
		return false;
	}

	private bool CanSolve(Equation eq)
	{
		Span<Operator> ops = stackalloc Operator[eq.Args.Length - 1];
		do
		{
			if(Calc(eq.Args, ops) == eq.Result) return true;
		}
		while(Next(ops, operatorsCount));
		return false;
	}

	public sealed override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, line =>
		{
			var eq = Equation.Parse(line);
			return CanSolve(eq) ? eq.Result : 0;
		}).ToString();
}

public sealed class Day07SolutionPart1 : Day07Solution
{
	public Day07SolutionPart1() : base(operatorsCount: 2) { }
}

public sealed class Day07SolutionPart2 : Day07Solution
{
	public Day07SolutionPart2() : base(operatorsCount: 3) { }
}
