using System;
using System.Collections.Immutable;

namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/2"/></remarks>
[Name("Cube Conundrum")]
public abstract class Day02Solution : Solution
{
	protected readonly record struct Game(int Id, ImmutableArray<CubeSet> Sets);

	protected readonly record struct CubeSet(int R, int G, int B);

	private static CubeSet ParseSet(ReadOnlySpan<char> span)
	{
		var rc = 0;
		var gc = 0;
		var bc = 0;
		while(true)
		{
			span = span.TrimStart();
			var next = span.IndexOf(',');
			if(next < 0) next = span.Length;

			var current = span[..next].Trim();
			var value = int.Parse(current[..current.IndexOf(' ')]);

			     if(current.EndsWith("red"))   rc = value;
			else if(current.EndsWith("green")) gc = value;
			else if(current.EndsWith("blue"))  bc = value;
			else throw new InvalidDataException($"Unexpected color entry: '{current.ToString()}'");

			if(next >= span.Length) break;
			span = span[(next + 1)..];
		}
		return new(rc, gc, bc);
	}

	protected static Game ParseGame(string line)
	{
		var p1 = line.IndexOf(':');
		var id = int.Parse(line.AsSpan(5, p1 - 5));
		int x  = line.IndexOf(';', p1 + 1);
		var sets = ImmutableArray.CreateBuilder<CubeSet>();
		while(true)
		{
			sets.Add(ParseSet(line.AsSpan(p1 + 1, x - p1 - 1)));
			p1 = x;
			if(x < line.Length)
			{
				x = line.IndexOf(';', x + 1);
				if(x < 0) x = line.Length;
			}
			else break;
		}
		return new Game(id, sets.ToImmutable());
	}
}

public sealed class Day02SolutionPart1 : Day02Solution
{
	private static readonly CubeSet AvailableCubes = new(R: 12, G: 13, B: 14);

	static bool IsPossible(CubeSet set)
		=> set.R <= AvailableCubes.R
		&& set.G <= AvailableCubes.G
		&& set.B <= AvailableCubes.B;

	static bool IsPossible(in Game game)
	{
		foreach(var set in game.Sets)
		{
			if(!IsPossible(set)) return false;
		}
		return true;
	}

	static int GetScore(in Game game)
		=> IsPossible(game) ? game.Id : 0;

	public override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, static line => GetScore(ParseGame(line))).ToString();
}

public sealed class Day02SolutionPart2 : Day02Solution
{
	private static int GetPower(in Game game)
	{
		var minR = 0;
		var minG = 0;
		var minB = 0;
		foreach(var set in game.Sets)
		{
			if(set.R > minR) minR = set.R;
			if(set.G > minG) minG = set.G;
			if(set.B > minB) minB = set.B;
		}
		return minR * minG * minB;
	}

	public override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, static line => GetPower(ParseGame(line))).ToString();
}
