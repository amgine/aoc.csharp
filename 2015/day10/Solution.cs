namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/10"/></remarks>
[Name(@"Elves Look, Elves Say")]
public abstract class Day10Solution : Solution
{
	protected static List<byte> Expand(List<byte> digits)
	{
		static void AppendCount(List<byte> expanded, int count)
		{
			if(count < 10)
			{
				expanded.Add((byte)count);
				return;
			}
			var stack = new Stack<byte>();
			while(count > 0)
			{
				stack.Push((byte)(count % 10));
				count /= 10;
			}
			while(stack.Count > 0)
			{
				expanded.Add(stack.Pop());
			}
		}

		var expanded = new List<byte>();
		var current = -1;
		var count   = 0;
		for(int i = 0; i < digits.Count; ++i)
		{
			if(current == -1)
			{
				current = digits[i];
				count = 1;
			}
			else if(digits[i] == current)
			{
				++count;
			}
			else
			{
				AppendCount(expanded, count);
				expanded.Add((byte)current);
				current = digits[i];
				count   = 1;
			}
		}
		if(current != -1)
		{
			AppendCount(expanded, count);
			expanded.Add((byte)current);
		}
		return expanded;
	}

	protected static int Solve(string line, int iterations)
	{
		var num = new List<byte>(capacity: line.Length);
		for(int i = 0; i < line.Length; ++i)
		{
			num.Add((byte)(line[i] - '0'));
		}
		var expanded = num;
		for(int i = 0; i < iterations; ++i)
		{
			expanded = Expand(expanded);
		}
		return expanded.Count;
	}
}

public sealed class Day10SolutionPart1 : Day10Solution
{
	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		return Solve(line, 40).ToString();
	}
}

public sealed class Day10SolutionPart2 : Day10Solution
{
	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		return Solve(line, 50).ToString();
	}
}
