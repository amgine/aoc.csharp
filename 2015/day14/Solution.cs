using System.Text.RegularExpressions;

namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/14"/></remarks>
[Name(@"Reindeer Olympics")]
public abstract partial class Day14Solution : Solution
{
	private static Regex ReindeerDescriptionRegex = CreateReindeerDescriptionRegex();

	[GeneratedRegex(@"^(?<name>\w+) can fly (?<speed>\d+) km\/s for (?<travel_time>\d+) seconds\, but then must rest for (?<rest_time>\d+) seconds\.$")]
	private static partial Regex CreateReindeerDescriptionRegex();

	protected readonly record struct ReindeerDescription(string Name, int Speed, int TravelTime, int RestTime);

	protected static ReindeerDescription ParseReindeerDescription(string line)
	{
		var match = ReindeerDescriptionRegex.Match(line);
		if(!match.Success) throw new InvalidDataException();

		return new(
			Name:       match.Groups["name"].Value,
			Speed:      int.Parse(match.Groups["speed"].ValueSpan),
			TravelTime: int.Parse(match.Groups["travel_time"].ValueSpan),
			RestTime:   int.Parse(match.Groups["rest_time"].ValueSpan));
	}

	protected static int GetDistance(ReindeerDescription reindeer, int time)
	{
		var segmentTime        = reindeer.TravelTime + reindeer.RestTime;
		var distancePerSegment = reindeer.Speed * reindeer.TravelTime;
		var fullSegments       = time / segmentTime;
		var remainder          = Math.Min(reindeer.TravelTime, time % segmentTime);
		return fullSegments * distancePerSegment + remainder * reindeer.Speed;
	}

	protected static List<ReindeerDescription> ParseInput(TextReader reader)
	{
		var deers = new List<ReindeerDescription>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			deers.Add(ParseReindeerDescription(line));
		}
		return deers;
	}
}

public sealed class Day14SolutionPart1 : Day14Solution
{
	const int Time = 2503;

	public static int GetMaxDistance(TextReader reader, int time)
		=> ParseInput(reader).Max(d => GetDistance(d, time));

	public override string Process(TextReader reader)
		=> GetMaxDistance(reader, Time).ToString();
}

public sealed class Day14SolutionPart2 : Day14Solution
{
	const int Time = 2503;

	public override string Process(TextReader reader)
	{
		var deers = ParseInput(reader);
		var scores = new int[deers.Count];
		for(int i = 1; i <= Time; ++i)
		{
			var maxDistance = -1;
			var lead = -1;
			for(int j = 0; j < deers.Count; ++j)
			{
				var dist = GetDistance(deers[j], i);
				if(dist > maxDistance)
				{
					lead = j;
					maxDistance = dist;
				}
			}
			++scores[lead];
		}
		return scores.Max().ToString();
	}
}
