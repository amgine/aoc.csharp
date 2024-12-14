namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/3"/></remarks>
[Name(@"Mull It Over")]
public abstract class Day03Solution : Solution
{
	static bool TryParseInteger(ref ReadOnlySpan<char> text, out long value)
	{
		if(text.Length == 0 || !char.IsAsciiDigit(text[0]))
		{
			value = 0;
			return false;
		}
		value = 0;
		do
		{
			value *= 10;
			value += text[0] - '0';
			text = text[1..];
		}
		while(text.Length != 0 && char.IsAsciiDigit(text[0]));
		return true;
	}

	static bool Expect(ref ReadOnlySpan<char> text, char value)
	{
		if(text.Length == 0 || text[0] != value) return false;
		text = text[1..];
		return true;
	}

	protected static long Process(ReadOnlySpan<char> text)
	{
		var sum = 0L;
		while(text.Length >= 8)
		{
			const string prefix = "mul(";

			var i0 = text.IndexOf(prefix);
			if(i0 < 0) break;
			text = text[(i0 + prefix.Length)..];
			if(text.Length < 4) break;

			long a = 0;
			long b = 0;

			var isValid
				 = TryParseInteger(ref text, out a)
				&& Expect         (ref text, ',')
				&& TryParseInteger(ref text, out b)
				&& Expect         (ref text, ')');

			if(!isValid) continue;

			sum += a * b;
		}
		return sum;
	}
}

public sealed class Day03SolutionPart1 : Day03Solution
{
	public override string Process(TextReader reader)
		=> Process(reader.ReadToEnd()).ToString();
}

public sealed class Day03SolutionPart2 : Day03Solution
{
	public override string Process(TextReader reader)
	{
		var line    = reader.ReadToEnd().AsSpan();
		var sum     = 0L;
		var enabled = true;
		while(line.Length != 0)
		{
			const string doCommand   = "do()";
			const string dontCommand = "don't()";

			if(enabled)
			{
				var e = line.IndexOf(dontCommand.AsSpan());
				if(e < 0)
				{
					sum += Process(line);
					break;
				}
				else
				{
					sum += Process(line[..e]);
					line = line[(e + dontCommand.Length)..];
					enabled = false;
				}
			}
			else
			{
				var e = line.IndexOf(doCommand.AsSpan());
				if(e < 0) break;
				enabled = true;
				line = line[(e + doCommand.Length)..];
			}
		}
		return sum.ToString();
	}
}
