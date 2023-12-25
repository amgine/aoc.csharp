namespace AoC.Year2018;

[Name(@"Chronal Calibration")]
public abstract class Day01Solution : Solution
{
	protected static List<int> LoadOffsets(TextReader reader)
	{
		var offsets = new List<int>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			offsets.Add(int.Parse(line));
		}
		return offsets;
	}
}

public sealed class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
		=> LoadOffsets(reader).Sum().ToString();
}

public sealed class Day01SolutionPart2 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var offsets = LoadOffsets(reader);
		var visited = new HashSet<int>() { 0 };
		var freq = 0;
		while(true)
		{
			foreach(var offset in offsets)
			{
				freq += offset;
				if(!visited.Add(freq))
				{
					return freq.ToString();
				}
			}
		}
	}
}
