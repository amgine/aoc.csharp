namespace AoC.Year2023;

[Name(@"Scratchcards")]
public abstract class Day4Solution : Solution
{
	protected record struct Card(int Id, HashSet<int> Winning, List<int> Present)
	{
		public readonly int MatchingCount
		{
			get
			{
				var count = 0;
				foreach(var num in Present)
				{
					if(Winning.Contains(num)) ++count;
				}
				return count;
			}
		}
	}

	protected static Card ParseCard(string line)
	{
		var p1 = line.IndexOf(':');
		var id = int.Parse(line.AsSpan(4, p1 - 4));

		var parts = line.Substring(p1 + 1).Split('|', StringSplitOptions.RemoveEmptyEntries);
		var part0 = parts[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var part1 = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

		var winning = new HashSet<int>(part0.Select(int.Parse));
		var present = new List<int>   (part1.Select(int.Parse));

		return new Card(id, winning, present);
	}
}

public sealed class Day4SolutionPart1 : Day4Solution
{
	static int Eval(in Card card)
	{
		var mc = card.MatchingCount;
		return mc > 0 ? 1 << (mc - 1) : 0;
	}

	public override string Process(TextReader reader)
		=> SumFromNonEmptyLines(reader, static line => Eval(ParseCard(line))).ToString();
}

public sealed class Day4SolutionPart2 : Day4Solution
{
	readonly struct NextBuffer
	{
		private readonly List<int> _counts = [];

		public NextBuffer() { }

		public readonly int Dequeue()
		{
			if(_counts.Count == 0) return 1;
			var c = _counts[0];
			_counts.RemoveAt(0);
			return c + 1;
		}

		public readonly void Enqueue(int count, int value)
		{
			for(int i = 0, t = Math.Min(count, _counts.Count); i < t; ++i)
			{
				_counts[i] += value;
			}
			for(int i = _counts.Count; i < count; ++i)
			{
				_counts.Add(value);
			}
		}
	}

	public override string Process(TextReader reader)
	{
		var next = new NextBuffer();
		int sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var card  = ParseCard(line);
			var count = next.Dequeue();
			next.Enqueue(card.MatchingCount, count);
			sum += count;
		}
		return sum.ToString();
	}
}
