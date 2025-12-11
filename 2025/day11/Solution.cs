namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/11"/></remarks>
[Name(@"Reactor")]
public abstract class Day11Solution : Solution
{
	protected sealed class Node(string name)
	{
		public string Name { get; } = name;

		public List<Node> Outputs { get; } = [];

		public List<Node> Inputs { get; } = [];

		public override string ToString() => Name;
	}

	protected static Node GetNode(Dictionary<string, Node> nodes, string name)
	{
		if(!nodes.TryGetValue(name, out var node))
		{
			nodes.Add(name, node = new(name));
		}
		return node;
	}

	protected class Graph
	{
		private readonly List<Node> _nodes = [];
		private readonly Dictionary<string, Node> _lookup = [];

		public Node this[string name] => _lookup[name];

		public Node GetOrCreateNode(string name)
		{
			if(!_lookup.TryGetValue(name, out var node))
			{
				_lookup.Add(name, node = new(name));
				_nodes.Add(node);
			}
			return node;
		}

		public void Simplify(Predicate<Node>? preserve = default)
		{
			bool changed;
			do
			{
				changed = false;
				for(int i = _nodes.Count - 1; i >= 0; --i)
				{
					var n = _nodes[i];
					if(preserve is not null && preserve(n))
					{
						continue;
					}
					if(n.Inputs.Count == 1 && n.Outputs.Count == 1)
					{
						var input = n.Inputs[0];
						var output = n.Outputs[0];
						input.Outputs.Remove(n);
						input.Outputs.Add(output);
						output.Inputs.Remove(n);
						output.Inputs.Add(input);
						_nodes.RemoveAt(i);
						changed = true;
					}
				}
			}
			while(changed);
		}
	}

	protected static Graph LoadGraph(TextReader reader)
	{
		var graph = new Graph();

		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrWhiteSpace(line)) continue;

			var s = line.IndexOf(':');
			var name = line.Substring(0, s);
			var node = graph.GetOrCreateNode(name);
			var outs = line.Substring(s + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			foreach(var o in outs)
			{
				var n = graph.GetOrCreateNode(o);
				node.Outputs.Add(n);
				n.Inputs.Add(node);
			}
		}

		return graph;
	}
}

public sealed class Day11SolutionPart1 : Day11Solution
{
	static long FindPaths(Node pos, Node dst)
	{
		if(pos == dst) return 1;

		var count = 0L;
		foreach(var node in pos.Outputs)
		{
			count += FindPaths(node, dst);
		}
		return count;
	}

	public override string Process(TextReader reader)
	{
		var graph = LoadGraph(reader);
		graph.Simplify(static n => n.Name is @"you" or @"out");
		return FindPaths(graph[@"you"], graph[@"out"]).ToString();
	}
}

public sealed class Day11SolutionPart2 : Day11Solution
{
	static long FindPaths(Node pos, Node dst, Predicate<Node>? filter = default)
	{
		if(pos == dst) return 1;

		var count = 0L;
		foreach(var next in pos.Outputs)
		{
			if(filter is not null && !filter(next)) continue;
			count += FindPaths(next, dst, filter);
		}
		return count;
	}

	private static HashSet<Node> ReachableFrom(Node node)
	{
		var reachableFrom = new HashSet<Node>() { node };
		var queue = new Queue<Node>();
		queue.Enqueue(node);
		while(queue.TryDequeue(out var n))
		{
			foreach(var input in n.Inputs)
			{
				if(reachableFrom.Add(input)) queue.Enqueue(input);
			}
		}
		return reachableFrom;
	}

	public override string Process(TextReader reader)
	{
		var graph = LoadGraph(reader);

		graph.Simplify(static n => n.Name is @"svr" or @"fft" or @"dac" or @"out");

		var start  = graph[@"svr"];
		var finish = graph[@"out"];

		var x = FindPaths(graph[@"svr"], graph[@"fft"], ReachableFrom(graph[@"fft"]).Contains);
		var y = FindPaths(graph[@"fft"], graph[@"dac"], ReachableFrom(graph[@"dac"]).Contains);
		var z = FindPaths(graph[@"dac"], graph[@"out"]);

		return (x * y * z).ToString();
	}
}
