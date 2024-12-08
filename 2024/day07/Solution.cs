namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/7"/></remarks>
[Name(@"Bridge Repair")]
public abstract class Day07Solution(int operatorsCount) : Solution
{
	enum Operator : byte
	{
		Add,
		Multiply,
		Concat,
	}

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

		public bool IsValidWith(ReadOnlySpan<Operator> ops)
			=> Calc(ops) == Result;

		public long Calc(ReadOnlySpan<Operator> ops)
		{
			long aggregated = Args[0];
			for(int i = 0; i < ops.Length; ++i)
			{
				aggregated = Exec(ops[i], aggregated, Args[i + 1]);
			}
			return aggregated;
		}

		static long Exec(Operator op, long a, long b)
			=> op switch
			{
				Operator.Add      => a + b,
				Operator.Multiply => a * b,
				Operator.Concat   => Concat(a, b),
				_ => throw new ArgumentException($"Unknown operator: {op}", nameof(op)),
			};
	}

	public static long Concat(long a, long b)
	{
		var x = b;
		x /= 10;
		a *= 10;
		while(x != 0)
		{
			x /= 10;
			a *= 10;
		}
		return a + b;
	}

	static bool Next(Span<Operator> expression, int count)
	{
		for(int i = 0; i < expression.Length; ++i)
		{
			++expression[i];
			if(expression[i] < (Operator)count) return true;
			expression[i] = default;
		}
		return false;
	}

	private bool CanSolve(Equation eq)
	{
		Span<Operator> ops = stackalloc Operator[eq.Args.Length - 1];
		do
		{
			if(eq.IsValidWith(ops)) return true;
		}
		while(Next(ops, operatorsCount));
		return false;
	}

	public sealed override string Process(TextReader reader)
	{
		var sum = 0L;
		Parallel.ForEach(
			LoadListFromNonEmptyStrings(reader, Equation.Parse),
			eq => { if(CanSolve(eq)) Interlocked.Add(ref sum, eq.Result); });
		return sum.ToString();
	}
}

public sealed class Day07SolutionPart1 : Day07Solution
{
	public Day07SolutionPart1() : base(operatorsCount: 2) { }
}

public sealed class Day07SolutionPart2 : Day07Solution
{
	public Day07SolutionPart2() : base(operatorsCount: 3) { }
}
