using System;
using System.Numerics;

namespace AoC.Year2023;

[Name(@"Camel Cards")]
public abstract class Day7Solution : Solution
{
	protected static class Ranks
	{
		public const int FiveOfAKind  = 7;
		public const int FourOfAKind  = 6;
		public const int FullHouse    = 5;
		public const int ThreeOfAKind = 4;
		public const int TwoPairs     = 3;
		public const int TwoOfAKind   = 2;
		public const int HighestCard  = 1;
	}

	protected record struct Hand(string Cards, int Bid);

	private static Hand ParseHand(string line)
	{
		var pos = line.IndexOf(' ');
		return new Hand(line[..pos], int.Parse(line.AsSpan(pos + 1)));
	}

	protected static List<Hand> ParseHands(TextReader reader)
		=> LoadListFromNonEmptyStrings(reader, ParseHand);

	protected abstract int GetHandRank(ReadOnlySpan<char> hand);

	protected abstract int GetCardRank(char card);

	private int Compare(Hand a, Hand b)
	{
		var r1 = GetHandRank(a.Cards);
		var r2 = GetHandRank(b.Cards);
		if(r2 > r1) return  1;
		if(r2 < r1) return -1;

		for(int i = 0; i < 5; ++i)
		{
			r1 = GetCardRank(a.Cards[i]);
			r2 = GetCardRank(b.Cards[i]);
			if(r2 > r1) return  1;
			if(r2 < r1) return -1;
		}

		return 0;
	}

	public override string Process(TextReader reader)
	{
		var hands = ParseHands(reader);
		hands.Sort(Compare);
		var sum = 0L;
		for(int i = 0; i < hands.Count; ++i)
		{
			var score = (hands.Count - i) * hands[i].Bid;
			sum += score;
		}
		return sum.ToString();
	}
}

public sealed class Day7SolutionPart1 : Day7Solution
{
	protected override int GetHandRank(ReadOnlySpan<char> hand)
	{
		static void GetCounts(ReadOnlySpan<char> hand, Span<int> counts)
		{
			Span<bool> used = stackalloc bool[5];
			for(int i = 0; i < hand.Length; ++i)
			{
				if(used[i]) continue;
				++counts[i];
				for(int j = i + 1; j < hand.Length; ++j)
				{
					if(hand[i] == hand[j])
					{
						counts[i]++;
						used[j] = true;
					}
				}
			}
		}

		static int GetRank(ReadOnlySpan<int> counts)
			=> Mathematics.Max(counts) switch
			{
				5 => Ranks.FiveOfAKind,
				4 => Ranks.FourOfAKind,
				3 => counts.Contains(2)   ? Ranks.FullHouse : Ranks.ThreeOfAKind,
				2 => counts.Count(2) == 2 ? Ranks.TwoPairs  : Ranks.TwoOfAKind,
				1 => Ranks.HighestCard,
				_ => throw new ApplicationException(),
			};

		Span<int> counts = stackalloc int[5];
		GetCounts(hand, counts);
		return GetRank(counts);
	}

	protected override int GetCardRank(char card)
		=> "23456789TJQKA".IndexOf(card);
}

public sealed class Day7SolutionPart2 : Day7Solution
{
	protected override int GetHandRank(ReadOnlySpan<char> hand)
	{
		static void GetCounts(ReadOnlySpan<char> hand, Span<int> counts)
		{
			Span<bool> used = stackalloc bool[5];
			for(int i = 0; i < hand.Length; ++i)
			{
				if(used[i] || hand[i] == 'J') continue;
				++counts[i];
				for(int j = i + 1; j < hand.Length; ++j)
				{
					if(hand[i] == hand[j])
					{
						counts[i]++;
						used[j] = true;
					}
				}
			}
		}

		static int GetRank(ReadOnlySpan<int> counts, int jokers)
			=> Mathematics.Max(counts) switch
			{
				5 => Ranks.FiveOfAKind,
				4 => jokers > 0 ? Ranks.FiveOfAKind : Ranks.FourOfAKind,
				3 => jokers switch
				{
					0 => counts.Contains(2) ? Ranks.FullHouse : Ranks.ThreeOfAKind,
					1 => Ranks.FourOfAKind,
					2 => Ranks.FiveOfAKind,
					_ => throw new ApplicationException(),
				},
				2 => counts.Count(2) == 2
					? jokers > 0 ? Ranks.FullHouse : Ranks.TwoPairs
					: jokers switch
					{
						1 => Ranks.ThreeOfAKind,
						2 => Ranks.FourOfAKind,
						3 => Ranks.FiveOfAKind,
						0 => Ranks.TwoOfAKind,
						_ => throw new ApplicationException(),
					},
				1 => jokers switch
				{
					0 => Ranks.HighestCard,
					1 => Ranks.TwoOfAKind,
					2 => Ranks.ThreeOfAKind,
					3 => Ranks.FourOfAKind,
					4 => Ranks.FiveOfAKind,
					_ => throw new ApplicationException(),
				},
				0 => jokers switch
				{
					5 => Ranks.FiveOfAKind,
					_ => throw new ApplicationException(),
				},
				_ => throw new ApplicationException(),
			};

		Span<int> counts = stackalloc int[5];
		GetCounts(hand, counts);
		return GetRank(counts, hand.Count('J'));
	}

	protected override int GetCardRank(char card)
		=> "J23456789TQKA".IndexOf(card);
}
