namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/2"/></remarks>
[Name(@"I Was Told There Would Be No Math")]
public abstract class Day02Solution : Solution
{
	protected readonly record struct Box(int Length, int Widgth, int Height)
	{
		public static Box Parse(string line)
		{
			Span<Range> ranges = stackalloc Range[3];
			if(line.AsSpan().Split(ranges, 'x') != 3)
				throw new InvalidDataException();
			var l = int.Parse(line.AsSpan(ranges[0]));
			var w = int.Parse(line.AsSpan(ranges[1]));
			var h = int.Parse(line.AsSpan(ranges[2]));
			return new Box(l, w, h);
		}
	}

	protected abstract int GetBoxTotal(Box box);

	public override string Process(TextReader reader)
	{
		var sum = 0L;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line)) continue;
			sum += GetBoxTotal(Box.Parse(line));
		}
		return sum.ToString();
	}
}

public sealed class Day02SolutionPart1 : Day02Solution
{
	protected override int GetBoxTotal(Box box)
	{
		var a1 = box.Length * box.Widgth;
		var a2 = box.Length * box.Height;
		var a3 = box.Widgth * box.Height;
		var area = 2 * a1 + 2 * a2 + 2 * a3;
		return area + Math.Min(Math.Min(a1, a2), Math.Min(a2, a3));
	}
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	protected override int GetBoxTotal(Box box)
	{
		var p1 = (box.Length + box.Widgth) * 2;
		var p2 = (box.Length + box.Height) * 2;
		var p3 = (box.Widgth + box.Height) * 2;
		var volume = box.Length * box.Widgth * box.Height;
		return volume + Math.Min(Math.Min(p1, p2), Math.Min(p2, p3));
	}
}
