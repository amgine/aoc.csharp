namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/9"/></remarks>
[Name(@"All in a Single Night")]
public abstract class Day09Solution : Solution
{
	protected sealed class Input(int locationsCount, Dictionary<InvariantPair, int> distances)
	{
		private readonly Dictionary<InvariantPair, int> _distances = distances;

		public int LocationsCount { get; } = locationsCount;

		public int GetDistance(int from, int to)
			=> _distances[new(from, to)];
	}

	protected static Input ParseInput(TextReader reader)
	{
		static int GetOrCreateLocation(string name, Dictionary<string, int> lookup)
		{
			if(!lookup.TryGetValue(name, out var id))
			{
				lookup.Add(name, id = lookup.Count);
			}
			return id;
		}

		var distances = new Dictionary<InvariantPair, int>();
		var locations = new Dictionary<string, int>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var s0 = line.IndexOf(" to ");
			if(s0 == -1) throw new InvalidDataException();
			var s1 = line.IndexOf(" = ");
			if(s1 == -1) throw new InvalidDataException();
			var fromName = line.Substring(0, s0);
			var toName   = line.Substring(s0 + " to ".Length, s1 - s0 - " to ".Length);
			var from     = GetOrCreateLocation(fromName, locations);
			var to       = GetOrCreateLocation(toName,   locations);
			var dist     = int.Parse(line.AsSpan(s1 + " = ".Length));
			distances[new(from, to)] = dist;
		}
		return new(locations.Count, distances); 
	}

	protected static bool AllUnique(int[] route)
	{
		for(int i = 0; i < route.Length - 1; ++i)
		{
			for(int j = i + 1; j < route.Length; ++j)
			{
				if(route[i] == route[j])
				{
					return false;
				}
			}
		}
		return true;
	}

	protected static int GetDistance(Input input, ReadOnlySpan<int> route)
	{
		var dist = 0;
		for(int i = 1; i < route.Length; ++i)
		{
			dist += input.GetDistance(route[i - 1], route[i]);
		}
		return dist;
	}

	protected static bool GetPermutation(int permutation, int count, Span<int> route)
	{
		for(int n = 0; n < route.Length; ++n)
		{
			route[n] = permutation % count;
			permutation /= count;
		}
		return permutation == 0;
	}
}

public sealed class Day09SolutionPart1 : Day09Solution
{
	public override string Process(TextReader reader)
	{
		var input = ParseInput(reader);
		var route = new int[input.LocationsCount];
		var bestDistance = int.MaxValue;
		for(int i = 0; i < int.MaxValue; ++i)
		{
			if(!GetPermutation(i, input.LocationsCount, route)) break;
			if(!AllUnique(route)) continue;
			var dist = GetDistance(input, route);
			if(dist < bestDistance) bestDistance = dist;
		}
		return bestDistance.ToString();
	}
}

public sealed class Day09SolutionPart2 : Day09Solution
{
	public override string Process(TextReader reader)
	{
		var input = ParseInput(reader);
		var route = new int[input.LocationsCount];
		var bestDistance = -1;
		for(int i = 0; i < int.MaxValue; ++i)
		{
			if(!GetPermutation(i, input.LocationsCount, route)) break;
			if(!AllUnique(route)) continue;
			var dist = GetDistance(input, route);
			if(dist > bestDistance) bestDistance = dist;
		}
		return bestDistance.ToString();
	}
}
