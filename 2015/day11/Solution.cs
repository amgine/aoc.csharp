namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/11"/></remarks>
[Name(@"Corporate Policy")]
public abstract class Day11Solution : Solution
{
	protected static bool NextPassword(Span<char> password)
	{
		for(int i = password.Length - 1; i >= 0; --i)
		{
			var c = password[i];
			if(c != 'z')
			{
				password[i] = (char)(c + 1);
				return true;
			}
			password[i] = 'a';
		}
		return false;
	}

	static bool HasIncreasingStraightOf3(ReadOnlySpan<char> text)
	{
		for(int i = 0; i < text.Length - 3; ++i)
		{
			var c0 = text[i + 0];
			var c1 = text[i + 1];
			var c2 = text[i + 2];
			if(c0 == c1 - 1 && c1 == c2 - 1)
			{
				return true;
			}
		}
		return false;
	}

	static bool HasPair(ReadOnlySpan<char> text, out int index)
	{
		for(int i = 0; i < text.Length - 1; ++i)
		{
			if(text[i] == text[i + 1])
			{
				index = i;
				return true;
			}
		}
		index = -1;
		return false;
	}

	static bool Has2DifferentPairs(ReadOnlySpan<char> text)
	{
		while(text.Length >= 4)
		{
			if(!HasPair(text, out var index1)) break;
			var c1 = text[index1];
			text = text[(index1 + 2)..];
			var r = text;
			while(r.Length >= 2)
			{
				if(!HasPair(r, out var index2)) break;
				var c2 = r[index2];
				if(c1 != c2) return true;
				r = r[(index2 + 2)..];
			}
		}
		return false;
	}

	static bool IsValidPassword(ReadOnlySpan<char> password)
	{
		if(!HasIncreasingStraightOf3(password)) return false;
		if(password.ContainsAny(['i', 'o', 'l'])) return false;
		if(!Has2DifferentPairs(password)) return false;
		return true;
	}

	protected static bool FindNextValidPassword(Span<char> password)
	{
		while(true)
		{
			if(!NextPassword(password)) return false;
			if(IsValidPassword(password)) return true;
		}
	}
}

public sealed class Day11SolutionPart1 : Day11Solution
{
	public override string Process(TextReader reader)
	{
		var password = reader.ReadLine() ?? throw new InvalidDataException();
		var chars = password.ToCharArray();
		if(!FindNextValidPassword(chars)) throw new InvalidDataException();
		return new(chars);
	}
}

public sealed class Day11SolutionPart2 : Day11Solution
{
	public override string Process(TextReader reader)
	{
		var password = reader.ReadLine() ?? throw new InvalidDataException();
		var chars = password.ToCharArray();
		if(!FindNextValidPassword(chars)) throw new InvalidDataException();
		if(!FindNextValidPassword(chars)) throw new InvalidDataException();
		return new(chars);
	}
}
