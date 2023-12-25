namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/7"/></remarks>
[Name(@"Handy Haversacks")]
public abstract class Day07Solution : Solution
{
	protected readonly record struct BagCount(int Count, Bag Bag);

	protected record class Bag(string Name, List<BagCount> Contains);

	static readonly char[] Separators = [',', '.'];

	static Bag GetOrCreateBag(string name, Dictionary<string, Bag> lookup)
	{
		if(!lookup.TryGetValue(name, out var bag))
		{
			lookup.Add(name, bag = new(name, new()));
		}
		return bag;
	}

	static BagCount ParseBagCount(ReadOnlySpan<char> text, Dictionary<string, Bag> lookup)
	{
		var s1 = text.IndexOf(' ');
		var count = int.Parse(text[..s1]);
		text = text.Slice(s1 + 1);
		var s2 = text.IndexOf(" bag");
		var name = new string(text.Slice(0, s2));
		return new(count, GetOrCreateBag(name, lookup));
	}

	private static Bag ParseBag(string line, Dictionary<string, Bag> lookup)
	{
		var s1   = line.IndexOf(" bags contain ");
		var name = line[..s1];
		var bag  = GetOrCreateBag(name, lookup);
		s1 += " bags contain ".Length;

		while(s1 < line.Length - 1)
		{
			var s2 = line.IndexOfAny(Separators, s1);
			var span = line.AsSpan(s1, s2 - s1);
			if(span.SequenceEqual("no other bags"))
			{
				break;
			}
			bag.Contains.Add(ParseBagCount(span, lookup));
			if(line[s2] == '.') break;
			s1 = s2 + 2;
		}

		return bag;
	}

	protected static List<Bag> ParseBags(TextReader reader)
	{
		var bags = new List<Bag>();
		var lookup = new Dictionary<string, Bag>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;

			bags.Add(ParseBag(line, lookup));
		}
		return bags;
	}
}

public sealed class Day07SolutionPart1 : Day07Solution
{
	static bool CanContain(Bag bag, Dictionary<Bag, bool> cache, Bag contained)
	{
		if(cache.TryGetValue(bag, out var cached))
		{
			return cached;
		}
		foreach(var c in bag.Contains)
		{
			if(c.Bag == contained || CanContain(c.Bag, cache, contained))
			{
				cache[bag] = true;
				return true;
			}
		}
		cache[bag] = false;
		return false;
	}

	public override string Process(TextReader reader)
	{
		var bags = ParseBags(reader);
		var goldShiny = bags.Find(static b => b.Name == "shiny gold")
			?? throw new InvalidDataException($"No shiny gold bag definition");

		var cache = new Dictionary<Bag, bool>();
		var count = 0;
		foreach(var bag in bags)
		{
			if(bag == goldShiny) continue;
			if(CanContain(bag, cache, goldShiny))
			{
				++count;
			}
		}

		return count.ToString();
	}
}

public sealed class Day07SolutionPart2 : Day07Solution
{
	static int CountContained(Bag bag)
	{
		var count = 0;
		foreach(var c in bag.Contains)
		{
			count += c.Count + c.Count * CountContained(c.Bag);
		}
		return count;
	}

	public override string Process(TextReader reader)
	{
		var bags = ParseBags(reader);
		var goldShiny = bags.Find(static b => b.Name == "shiny gold")
			?? throw new InvalidDataException($"No shiny gold bag definition");

		return CountContained(goldShiny).ToString();
	}
}
