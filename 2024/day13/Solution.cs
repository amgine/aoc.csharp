namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/13"/></remarks>
[Name(@"Claw Contraption")]
public abstract class Day13Solution : Solution
{
	protected readonly record struct Input(Vector2D A, Vector2D B, Point2D Target);

	private static bool TryParseInput(TextReader reader, out Input input)
	{
		var a = reader.ReadLine();
		var b = reader.ReadLine();
		var t = reader.ReadLine();
		reader.ReadLine();

		if(a is null || b is null || t is null)
		{
			input = default;
			return false;
		}

		static Vector2D ParseVector(string line)
		{
			var i0 = line.IndexOf("X+");
			var i1 = line.IndexOf(",",  i0 + 2);
			var i2 = line.IndexOf("Y+", i1 + 1);
			return new(
				int.Parse(line.AsSpan(i0 + 2, i1 - (i0 + 2))),
				int.Parse(line.AsSpan(i2 + 2)));
		}

		static Point2D ParsePoint(string line)
		{
			var i0 = line.IndexOf("X=");
			var i1 = line.IndexOf(",",  i0 + 2);
			var i2 = line.IndexOf("Y=", i1 + 1);
			return new(
				int.Parse(line.AsSpan(i0 + 2, i1 - (i0 + 2))),
				int.Parse(line.AsSpan(i2 + 2)));
		}

		input = new Input(ParseVector(a), ParseVector(b), ParsePoint(t));
		return true;
	}

	protected static long GetScore(long targetX, long targetY, Vector2D a, Vector2D b)
	{
		const int CostA = 3;
		const int CostB = 1;

		var num = targetX  * a.DeltaY - a.DeltaX * targetY;
		var den = b.DeltaX * a.DeltaY - a.DeltaX * b.DeltaY;

		if(num % den != 0) return 0;

		var countB = num / den;

		num = targetX - b.DeltaX * countB;
		den = a.DeltaX;
		if(num % den != 0) return 0;

		var countA = num / den;
		return countA * CostA + countB * CostB;
	}

	protected abstract long GetScore(Input input);

	public sealed override string Process(TextReader reader)
	{
		var sum = 0L;
		while(TryParseInput(reader, out var input))
		{
			sum += GetScore(input);
		}
		return sum.ToString();
	}
}

public sealed class Day13SolutionPart1 : Day13Solution
{
	protected override long GetScore(Input input)
		=> GetScore(input.Target.X, input.Target.Y, input.A, input.B);
}

public sealed class Day13SolutionPart2 : Day13Solution
{
	const long Offset = 10000000000000L;

	protected override long GetScore(Input input)
		=> GetScore(input.Target.X + Offset, input.Target.Y + Offset, input.A, input.B);
}
