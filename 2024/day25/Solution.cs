namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/25"/></remarks>
[Name(@"Code Chronicle")]
public abstract class Day25Solution : Solution
{
}

public sealed class Day25SolutionPart1 : Day25Solution
{
	private static byte[] ParseSchematics(List<string> schematics, char term)
	{
		var result = new byte[schematics[0].Length];
		for(int i = 0; i < schematics[0].Length; ++i)
		{
			for(int j = 1; j < schematics.Count; ++j)
			{
				if(schematics[j][i] == term)
				{
					var h = j - 1;
					result[i] = (byte)h;
					break;
				}
			}
		}
		return result;
	}

	private static void ParseSchematics(List<string> schematics, List<byte[]> locks, List<byte[]> keys)
	{
		if(schematics[0].All(static c => c == '#'))
		{
			locks.Add(ParseSchematics(schematics, '.'));
		}
		else if(schematics[^1].All(static c => c == '#'))
		{
			keys.Add(ParseSchematics(schematics, '#'));
		}
		else throw new InvalidDataException();
	}

	private static bool Overlaps(byte[] @lock, byte[] key)
	{
		for(int i = 0; i < @lock.Length; ++i)
		{
			if(@lock[i] > key[i]) return true;
		}
		return false;
	}

	public override string Process(TextReader reader)
	{
		var locks = new List<byte[]>();
		var keys  = new List<byte[]>();

		var current = new List<string>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0)
			{
				ParseSchematics(current, locks, keys);
				current.Clear();
				continue;
			}
			current.Add(line);
		}
		if(current.Count != 0)
		{
			ParseSchematics(current, locks, keys);
		}
		var sum = 0;
		foreach(var @lock in locks)
		{
			foreach(var key in keys)
			{
				if(!Overlaps(@lock, key)) ++sum;
			}
		}
		return sum.ToString();
	}
}
