using System.Diagnostics.CodeAnalysis;

namespace AoC.Year2018;

[Name(@"Inventory Management System")]
public abstract class Day02Solution : Solution
{
}

public sealed class Day02SolutionPart1 : Day02Solution
{
	public override string Process(TextReader reader)
	{
		Span<int> counts = stackalloc int[26];
		var countWith2 = 0;
		var countWith3 = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			for(int i = 0; i < line.Length; ++i)
			{
				++counts[line[i] - 'a'];
			}
			if(counts.Contains(2)) ++countWith2;
			if(counts.Contains(3)) ++countWith3;
			counts.Clear();
		}
		return (countWith2 * countWith3).ToString();
	}
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	static bool TryGetID(string box1, string box2,
		[MaybeNullWhen(returnValue: false)] out string id)
	{
		if(box1.Length != box2.Length) goto unmatched;

		var diff  = 0;
		var index = -1;
		for(int i = 0; i < box1.Length; ++i)
		{
			if(box1[i] == box2[i]) continue;
			if(++diff > 1) goto unmatched;
			index = i;
		}

		if(diff != 1) goto unmatched;

		if(index == 0)
		{
			id = box1[1..];
		}
		else if(index == box1.Length - 1)
		{
			id = box1[..^1];
		}
		else
		{
			id = string.Concat(box1.AsSpan(0, index), box1.AsSpan(index + 1));
		}
		return true;

		unmatched:
		id = default;
		return false;
	}

	public override string Process(TextReader reader)
	{
		var boxes = LoadInputAsListOfNonEmptyStrings(reader);
		for(int i = 0; i < boxes.Count - 2; ++i)
		{
			for(int j = i + 1; j < boxes.Count; ++j)
			{
				if(TryGetID(boxes[i], boxes[j], out var id))
				{
					return id;
				}
			}
		}
		throw new InvalidDataException();
	}
}
