using System.Diagnostics.CodeAnalysis;

namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/12"/></remarks>
[Name(@"Christmas Tree Farm")]
public abstract class Day12Solution : Solution
{
}

public sealed class Day12SolutionPart1 : Day12Solution
{
	private static int ReadShape(TextReader reader)
	{
		string? line;
		int count = 0;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(line)) break;
			count += line.Count(static c => c == '#');
		}
		return count;
	}

	private static bool TryParseRequirements(string line,
		out int width, out int height, [MaybeNullWhen(false)] out int[] counts)
	{
		var s0 = line.IndexOf(':');
		var s1 = line.IndexOf('x');
		if(s0 <= 0 || s1 <= 0)
		{
			width  = default;
			height = default;
			counts = default;
			return false;
		}
		width  = int.Parse(line.AsSpan(0, s1));
		height = int.Parse(line.AsSpan(s1 + 1, s0 - s1 - 1));
		counts = Array.ConvertAll(line
			.Substring(s0 + 1)
			.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries),
			int.Parse);
		return true;
	}

	public override string Process(TextReader reader)
	{
		var shapes = new List<int>();
		var fits = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(line)) continue;

			if(line.EndsWith(':'))
			{
				shapes.Add(ReadShape(reader));
				continue;
			}

			if(TryParseRequirements(line, out var w, out var h, out var counts))
			{
				var area = w * h;
				var sum = counts.Zip(shapes, static (a, b) => a * b).Sum();
				if(sum <= area) ++fits;
			}
		}

		return fits.ToString();
	}
}
