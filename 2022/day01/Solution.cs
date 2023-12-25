namespace AoC.Year2022;

[Name(@"Calorie Counting")]
public abstract class Day01Solution : Solution
{
}

public class Day01SolutionPart1 : Day01Solution
{
	public override string Process(TextReader reader)
	{
		var max = 0;
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0)
			{
				if(sum > max) max = sum;
				sum = 0;
				continue;
			}
			sum += int.Parse(line);
		}
		if(sum > max) max = sum;
		return max.ToString();
	}
}

public class Day01SolutionPart2 : Day01Solution
{
	const int TopCount = 3;

	static void Update(List<int> max, int sum)
	{
		int index = 0;
		for(int i = max.Count - 1; i >= 0; --i)
		{
			if(max[i] > sum)
			{
				index = i + 1;
				break;
			}
		}

		if(index < TopCount)
		{
			max.Insert(index, sum);
			if(max.Count > TopCount) max.RemoveAt(max.Count - 1);
		}
	}

	public override string Process(TextReader reader)
	{
		var max = new List<int>();
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0)
			{
				Update(max, sum);
				sum = 0;
				continue;
			}
			sum += int.Parse(line);
		}
		Update(max, sum);
		return max.Sum().ToString();
	}
}
