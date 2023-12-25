using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/13"/></remarks>
[Name(@"Knights of the Dinner Table")]
public abstract partial class Day13Solution : Solution
{
	protected readonly record struct Rule(int Person, int Neighbour, int Diff);

	private static readonly Regex RuleRegex = CreateRuleRegex();

	[GeneratedRegex(@"^(?<person>\w+)\swould\s(?<change>lose|gain)\s(?<value>\d+)\shappiness\sunits\sby\ssitting\snext\sto\s(?<neighbour>\w+)\.$")]
	private static partial Regex CreateRuleRegex();

	protected static Rule ParseRule(string line, Dictionary<string, int> lookup)
	{
		static int GetPersonId(string name, Dictionary<string, int> lookup)
		{
			if(!lookup.TryGetValue(name, out var personId))
			{
				lookup.Add(name, personId = lookup.Count);
			}
			return personId;
		}

		var match = RuleRegex.Match(line);
		if(!match.Success) throw new InvalidDataException();

		var p = GetPersonId(match.Groups[@"person"].Value, lookup);
		var n = GetPersonId(match.Groups[@"neighbour"].Value, lookup);
		var sign = match.Groups[@"change"].ValueSpan switch
		{
			"gain" =>  1,
			"lose" => -1,
			_ => throw new InvalidDataException(),
		};
		var diff = sign * int.Parse(match.Groups[@"value"].ValueSpan);

		return new(p, n, diff);
	}

	protected static List<Rule> ParseRules(TextReader reader)
	{
		var lookup = new Dictionary<string, int>();
		var rules  = new List<Rule>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var rule = ParseRule(line, lookup);
			rules.Add(rule);
		}
		return rules;
	}

	protected static bool AllUnique(int[] setup)
	{
		for(int i = 0; i < setup.Length - 1; ++i)
		{
			for(int j = i + 1; j < setup.Length; ++j)
			{
				if(setup[i] == setup[j])
				{
					return false;
				}
			}
		}
		return true;
	}

	protected static int GetDiff(Dictionary<InvariantPair, int> diffs, ReadOnlySpan<int> setup)
	{
		var diff = diffs.TryGetValue(new(setup[0], setup[^1]), out var d0) ? d0 : 0;
		for(int i = 1; i < setup.Length; ++i)
		{
			if(diffs.TryGetValue(new(setup[i - 1], setup[i]), out var d1))
			{
				diff += d1;
			}
		}
		return diff;
	}

	protected static int GetDiffIgnoringLowest(Dictionary<InvariantPair, int> diffs, ReadOnlySpan<int> setup)
	{
		var diff = diffs.TryGetValue(new(setup[0], setup[^1]), out var d0) ? d0 : 0;
		var min  = diff;
		for(int i = 1; i < setup.Length; ++i)
		{
			if(diffs.TryGetValue(new(setup[i - 1], setup[i]), out var d1))
			{
				if(d1 < min) min = d1;
				diff += d1;
			}
		}
		return diff - min;
	}

	protected static bool GetPermutation(int permutation, int count, Span<int> setup)
	{
		for(int n = 0; n < setup.Length; ++n)
		{
			setup[n] = permutation % count;
			permutation /= count;
		}
		return permutation == 0;
	}

	protected static Dictionary<InvariantPair, int> GetDiffs(List<Rule> rules, out int personsCount)
	{
		var diffs = new Dictionary<InvariantPair, int>();
		var persons = new HashSet<int>();
		foreach(var rule in rules)
		{
			persons.Add(rule.Person);
			persons.Add(rule.Neighbour);
			var p = new InvariantPair(rule.Person, rule.Neighbour);
			if(diffs.TryGetValue(p, out var diff))
			{
				diff += rule.Diff;
				diffs[p] = diff;
			}
			else
			{
				diffs.Add(p, rule.Diff);
			}
		}
		personsCount = persons.Count;
		return diffs;
	}

	protected static int GetBestScore(Dictionary<InvariantPair, int> diffs, int personsCount)
	{
		var setup = new int[personsCount];
		var bestDiff = int.MinValue;
		for(int i = 0; i < int.MaxValue; ++i)
		{
			if(!GetPermutation(i, personsCount, setup)) break;
			if(!AllUnique(setup)) continue;
			var diff = GetDiff(diffs, setup);
			if(diff > bestDiff) bestDiff = diff;
		}
		return bestDiff;
	}

	protected static int GetBestScoreIgnoringLowest(Dictionary<InvariantPair, int> diffs, int personsCount)
	{
		var setup = new int[personsCount];
		var bestDiff = int.MinValue;
		for(int i = 0; i < int.MaxValue; ++i)
		{
			if(!GetPermutation(i, personsCount, setup)) break;
			if(!AllUnique(setup)) continue;
			var diff = GetDiffIgnoringLowest(diffs, setup);
			if(diff > bestDiff) bestDiff = diff;
		}
		return bestDiff;
	}
}

public sealed class Day13SolutionPart1 : Day13Solution
{
	public override string Process(TextReader reader)
	{
		var rules = ParseRules(reader);
		var diffs = GetDiffs(rules, out var personsCount);
		return GetBestScore(diffs, personsCount).ToString();
	}
}

public sealed class Day13SolutionPart2 : Day13Solution
{
	public override string Process(TextReader reader)
	{
		var rules = ParseRules(reader);
		var diffs = GetDiffs(rules, out var personsCount);
		return GetBestScoreIgnoringLowest(diffs, personsCount).ToString();
	}
}
