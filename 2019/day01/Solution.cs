namespace AoC.Year2019;

[Name(@"The Tyranny of the Rocket Equation")]
public abstract class Day01Solution : Solution
{
	protected static int GetRequiredFuel(int mass) => mass / 3 - 2;
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var sum = 0L;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var mass = int.Parse(line);
			sum += GetRequiredFuel(mass);
		}
		return sum.ToString();
	}
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var sum = 0L;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var mass = int.Parse(line);
			while(true)
			{
				mass = GetRequiredFuel(mass);
				if(mass <= 0) break;
				sum += mass;
			}
		}
		return sum.ToString();
	}
}
