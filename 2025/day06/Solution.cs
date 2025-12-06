using System.Globalization;

namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/6"/></remarks>
[Name(@"Trash Compactor")]
public abstract class Day06Solution : Solution
{
	protected enum Operator
	{
		Mul,
		Add,
	}

	protected sealed class Expression
	{
		public List<int> Args { get; } = [];

		public Operator Operator { get; set; }

		public long Evaluate()
		{
			var result = (long)Args[0];
			switch(Operator)
			{
				case Operator.Add:
					for(int i = 1; i < Args.Count; ++i)
					{
						result += Args[i];
					}
					break;
				case Operator.Mul:
					for(int i = 1; i < Args.Count; ++i)
					{
						result *= Args[i];
					}
					break;
			}
			return result;
		}
	}

	protected abstract List<Expression> LoadExpressions(TextReader reader);

	protected static Operator ParseOperator(char c)
		=> c switch
		{
			'*' => Operator.Mul,
			'+' => Operator.Add,
			_ => throw new InvalidDataException(),
		};

	protected static long Evaluate(List<Expression> expressions)
	{
		var sum = 0L;
		foreach(var e in expressions)
		{
			sum += e.Evaluate();
		}
		return sum;
	}

	public override string Process(TextReader reader)
		=> Evaluate(LoadExpressions(reader)).ToString();
}

public sealed class Day06SolutionPart1 : Day06Solution
{
	protected override List<Expression> LoadExpressions(TextReader reader)
	{
		var expressions = new List<Expression>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(line)) continue;
			var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			for(int i = 0; i < parts.Length; ++i)
			{
				Expression expression;
				if(i >= expressions.Count)
				{
					expressions.Add(expression = new());
				}
				else
				{
					expression = expressions[i];
				}
				if(char.IsAsciiDigit(parts[i][0]))
				{
					expression.Args.Add(int.Parse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture));
				}
				else
				{
					expression.Operator = ParseOperator(parts[i][0]);
				}
			}
		}
		return expressions;
	}
}

public sealed class Day06SolutionPart2 : Day06Solution
{
	protected override List<Expression> LoadExpressions(TextReader reader)
	{
		var start = 0;
		var expression = default(Expression);
		var expressions = new List<Expression>();
		var lines = LoadInputAsListOfNonEmptyStrings(reader);

		void FillArguments(Expression expression, int firstColumn, int lastColumn)
		{
			for(int j = lastColumn; j >= firstColumn; --j)
			{
				var n = 0;
				var hasColumn = false;
				for(int k = 0; k < lines.Count - 1; ++k)
				{
					var c = lines[k][j];
					if(!char.IsAsciiDigit(c)) continue;
					n *= 10;
					n += c - '0';
					hasColumn = true;
				}
				if(hasColumn)
				{
					expression.Args.Add(n);
				}
			}
		}

		var last = lines[^1];
		for(int i = 0; i < last.Length; ++i)
		{
			if(last[i] != ' ')
			{
				if(expression is not null)
				{
					FillArguments(expression, start, i - 2);
				}
				expressions.Add(expression = new());
				expression.Operator = ParseOperator(last[i]);
				start = i; // operator is in the first column
			}
		}
		if(expression is not null)
		{
			FillArguments(expression, start, last.Length - 1);
		}
		return expressions;
	}
}
