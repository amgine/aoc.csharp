namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/6"/></remarks>
[Name(@"Custom Customs")]
public abstract class Day06Solution : Solution
{
}

public sealed class Day06SolutionPart1 : Day06Solution
{
	public override string Process(TextReader reader)
	{
		var count = 0;
		var set = new HashSet<char>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0)
			{
				count += set.Count;
				set.Clear();
				continue;
			}
			foreach(var c in line) set.Add(c);
		}
		count += set.Count;
		return count.ToString();
	}
}

public sealed class Day06SolutionPart2 : Day06Solution
{
	public override string Process(TextReader reader)
	{
		var count = 0;
		var set = new HashSet<char>();
		var start = true;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0)
			{
				count += set.Count;
				set.Clear();
				start = true;
				continue;
			}
			if(start)
			{
				start = false;
				foreach(var c in line) set.Add(c);
			}
			else
			{
				set.IntersectWith(line);
			}
		}
		count += set.Count;
		return count.ToString();
	}
}
