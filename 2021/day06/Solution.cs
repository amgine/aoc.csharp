namespace AoC.Year2021;

[Name(@"Lanternfish")]
public abstract class Day06Solution : Solution
{
	const int ReadyToSpawnAt    = 0;
	const int NewSpanedStartsAt = 8;
	const int SpawnersResetTo   = 6;
	const int PossibleAges      = 9;

	protected static int[] LoadInput(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException("No input.");
		return Array.ConvertAll(line.Split(',', StringSplitOptions.TrimEntries), int.Parse);
	}

	protected static long Count(ReadOnlySpan<int> initial, int days)
	{
		Span<long> counts = stackalloc long[PossibleAges];
		foreach(var n in initial) counts[n]++;
		while(days-- > 0)
		{
			var readyToSpawNewFish = counts[ReadyToSpawnAt];
			counts[1..].CopyTo(counts);
			counts[NewSpanedStartsAt] = readyToSpawNewFish;
			counts[SpawnersResetTo]  += readyToSpawNewFish;
		}
		return SpanHelper.Sum<long>(counts);
	}
}

public sealed class Day06SolutionPart1 : Day06Solution
{
	const int Days = 80;

	public override string Process(TextReader reader)
		=> Count(LoadInput(reader), Days).ToString();
}

public sealed class Day06SolutionPart2 : Day06Solution
{
	const int Days = 256;

	public override string Process(TextReader reader)
		=> Count(LoadInput(reader), Days).ToString();
}
