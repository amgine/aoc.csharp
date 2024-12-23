namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/23"/></remarks>
[Name(@"LAN Party")]
public abstract class Day23Solution : Solution
{
}

public sealed class Day23SolutionPart1 : Day23Solution
{
	public override string Process(TextReader reader)
	{
		var connections = new Dictionary<string, List<string>>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;

			var i = line.IndexOf('-');
			var a = line.Substring(0, i);
			var b = line.Substring(i + 1);
			if(!connections.TryGetValue(a, out var ac))
			{
				connections.Add(a, ac = []);
			}
			if(!connections.TryGetValue(b, out var bc))
			{
				connections.Add(b, bc = []);
			}
			ac.Add(b);
			bc.Add(a);
		}
		var sum = 0L;
		var visited = new HashSet<string>();
		foreach(var node in connections)
		{
			if(node.Key.StartsWith('t') && node.Value.Count >= 2)
			{
				visited.Add(node.Key);
				for(int i0 = 0; i0 < node.Value.Count - 1; ++i0)
				{
					if(visited.Contains(node.Value[i0])) continue;
					var n0 = node.Value[i0];

					for(int i1 = i0 + 1; i1 < node.Value.Count; ++i1)
					{
						if(visited.Contains(node.Value[i1])) continue;
						var n1 = node.Value[i1];
						if(!connections[n0].Contains(n1)) continue;
						++sum;
					}
				}
			}
		}
		return sum.ToString();
	}
}

public sealed class Day23SolutionPart2 : Day23Solution
{
	public override string Process(TextReader reader)
	{
		var connections = new Dictionary<string, HashSet<string>>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;

			var i = line.IndexOf('-');
			var a = line.Substring(0, i);
			var b = line.Substring(i + 1);
			if(!connections.TryGetValue(a, out var ac))
			{
				connections.Add(a, ac = []);
			}
			if(!connections.TryGetValue(b, out var bc))
			{
				connections.Add(b, bc = []);
			}
			ac.Add(b);
			bc.Add(a);
		}
		var visited = new HashSet<string>();
		var set     = new HashSet<string>();
		var max     = new List<string>();
		foreach(var n in connections)
		{
			visited.Clear();
			var hasUntested = true;
			while(hasUntested)
			{
				hasUntested = false;
				set.Clear();
				foreach(var n0 in n.Value)
				{
					if(visited.Contains(n0)) continue;

					var otherSide = connections[n0];
					var connected = true;
					foreach(var x in set)
					{
						if(!otherSide.Contains(x))
						{
							connected = false;
							break;
						}
					}
					if(connected)
					{
						visited.Add(n0);
						set.Add(n0);
					}
					else
					{
						hasUntested = true;
					}
				}
				set.Add(n.Key);
				if(set.Count > max.Count)
				{
					max.Clear();
					max.AddRange(set);
				}
			}
		}
		max.Sort();
		return string.Join(",", max);
	}
}
