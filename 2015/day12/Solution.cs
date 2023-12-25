using System.Text;
using System.Text.Json;

namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/12"/></remarks>
[Name(@"JSAbacusFramework.io")]
public abstract class Day12Solution : Solution
{
}

public sealed class Day12SolutionPart1 : Day12Solution
{
	private static bool TryParseNumber(ref ReadOnlySpan<char> sequence, out int number)
	{
		var negative = false;
		for(int i = 0; i < sequence.Length; ++i)
		{
			if(sequence[i] == '-')
			{
				negative = true;
				continue;
			}
			if(char.IsAsciiDigit(sequence[i]))
			{
				var end = i;
				for(int j = i + 1; j < sequence.Length; ++j)
				{
					if(!char.IsAsciiDigit(sequence[j]))
					{
						end = j - 1;
						break;
					}
				}
				number = int.Parse(sequence.Slice(i, end - i + 1));
				if(negative) number = -number;
				sequence = sequence[(end + 1)..];
				return true;
			}
			negative = false;
		}
		number = 0;
		return false;
	}

	public override string Process(TextReader reader)
	{
		var json = reader.ReadLine() ?? throw new InvalidDataException();
		var sequence = json.AsSpan();
		var sum = 0;
		while(TryParseNumber(ref sequence, out var number))
		{
			sum += number;
		}
		return sum.ToString();
	}
}

public sealed class Day12SolutionPart2 : Day12Solution
{
	static int ReadObject(ref Utf8JsonReader json)
	{
		bool hadRed = false;
		var sum = 0;
		while(json.Read())
		{
			switch(json.TokenType)
			{
				case JsonTokenType.EndObject: return sum;
				case JsonTokenType.PropertyName:
					if(!json.Read())
					{
						throw new InvalidDataException();
					}
					if(json.TokenType == JsonTokenType.String && json.ValueTextEquals("red"u8))
					{
						hadRed = true;
						sum = 0;
					}
					else
					{
						if(hadRed) json.Skip();
						else sum += ReadValue(ref json);
					}
					break;
				default: throw new InvalidDataException();
			}
		}
		throw new InvalidDataException();
	}

	static int ReadArray(ref Utf8JsonReader json)
	{
		var sum = 0;
		while(json.Read())
		{
			if(json.TokenType == JsonTokenType.EndArray)
			{
				return sum;
			}
			sum += ReadValue(ref json);
		}
		throw new InvalidDataException();
	}

	static int ReadValue(ref Utf8JsonReader json)
	{
		return json.TokenType switch
		{
			JsonTokenType.String      => 0,
			JsonTokenType.Number      => json.GetInt32(),
			JsonTokenType.StartArray  => ReadArray (ref json),
			JsonTokenType.StartObject => ReadObject(ref json),
			_ => throw new InvalidDataException(),
		};
	}

	public override string Process(TextReader reader)
	{
		var text = reader.ReadLine() ?? throw new InvalidDataException();
		var json = new Utf8JsonReader(Encoding.UTF8.GetBytes(text));
		if(!json.Read()) throw new InvalidDataException();
		int sum = ReadValue(ref json);
		return sum.ToString();
	}
}
