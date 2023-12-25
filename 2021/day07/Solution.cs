namespace AoC.Year2021;

[Name(@"The Treachery of Whales")]
public abstract class Day07Solution : Solution
{
	protected static int[] LoadInput(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException("No input.");
		return Array.ConvertAll(line.Split(',', StringSplitOptions.TrimEntries), int.Parse);
	}

	protected abstract long GetFuelConsumption(long distance);

	public override string Process(TextReader reader)
	{
		var input = LoadInput(reader);
		var minFuel = long.MaxValue;
		for(int i = input.Min(), max = input.Max(); i <= max; ++i)
		{
			var fuel = 0L;
			for(int j = 0; j < input.Length; ++j)
			{
				var dist = Math.Abs(input[j] - i);
				fuel += GetFuelConsumption(dist);
				if(fuel >= minFuel) break;
			}
			if(fuel < minFuel) minFuel = fuel;
		}
		return minFuel.ToString();
	}
}

public sealed class Day07SolutionPart1 : Day07Solution
{
	protected override long GetFuelConsumption(long distance)
		=> distance;
}

public sealed class Day07SolutionPart2 : Day07Solution
{
	protected override long GetFuelConsumption(long distance)
		=> distance * (distance + 1) / 2;
}
