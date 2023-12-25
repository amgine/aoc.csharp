namespace AoC.Year2017;

/// <remarks><a href="https://adventofcode.com/2017/day/4"/></remarks>
[Name(@"High-Entropy Passphrases")]
public abstract class Day04Solution : Solution
{
	protected abstract bool IsValid(string passPhrase);

	public override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, line => IsValid(line) ? 1 : 0).ToString();
}

public sealed class Day04SolutionPart1 : Day04Solution
{
	protected override bool IsValid(string passPhrase)
	{
		var words = passPhrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		return words.Length < 2 || Array.TrueForAll(words, new HashSet<string>().Add);
	}
}

public sealed class Day04SolutionPart2 : Day04Solution
{
	static Dictionary<char, int> GetHistogram(string word)
	{
		var hist = new Dictionary<char, int>();
		foreach(var c in word)
		{
			if(hist.TryGetValue(c, out int value))
			{
				hist[c] = value + 1;
			}
			else
			{
				hist.Add(c, 1);
			}
		}
		return hist;
	}

	static bool IsAnagram(Dictionary<char, int> histA, Dictionary<char, int> histB)
	{
		if(histA.Count != histB.Count) return false;
		foreach(var kvp in histA)
		{
			if(!histB.TryGetValue(kvp.Key, out var count) || kvp.Value != count)
			{
				return false;
			}
		}
		foreach(var kvp in histB)
		{
			if(!histA.TryGetValue(kvp.Key, out var count) || kvp.Value != count)
			{
				return false;
			}
		}
		return true;
	}

	protected override bool IsValid(string passPhrase)
	{
		var words = passPhrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if(words.Length < 2) return true;
		var hists = new Dictionary<char, int>?[words.Length];
		for(int i = 0; i < words.Length - 1; ++i)
		{
			for(int j = i + 1; j < words.Length; ++j)
			{
				if(words[i].Length != words[j].Length) continue;
				var histA = hists[i] ??= GetHistogram(words[i]);
				var histB = hists[j] ??= GetHistogram(words[j]);
				if(IsAnagram(histA, histB)) return false;
			}
		}
		return true;
	}
}
