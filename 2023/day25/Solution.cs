namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/25"/></remarks>
[Name(@"Snowverload")]
public abstract class Day25Solution : Solution
{
}

public sealed class Day25SolutionPart1 : Day25Solution
{
	sealed class Node(string name)
	{
		public string Name { get; } = name;

		public int Value { get; set; } = 1;

		public List<Edge> Edges { get; } = [];

		public override string ToString() => Name;
	}

	sealed class Edge(Node a, Node b)
	{
		public Node A { get; } = a;

		public Node B { get; } = b;

		public Node Other(Node node) => A == node ? B : A;

		public override string ToString()
			=> $"{A.Name} <-> {B.Name}";
	}

	static List<Node> ParseGraph(TextReader reader)
	{
		static Node GetOrCreateNode(string name, Dictionary<string, Node> lookup)
		{
			if(!lookup.TryGetValue(name, out var node))
			{
				lookup.Add(name, node = new(name));
			}
			return node;
		}

		var lookup = new Dictionary<string, Node>();

		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			var sep   = line.IndexOf(':');
			var name  = line[..sep].Trim();
			var names = line[(sep + 1)..].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			var a = GetOrCreateNode(name, lookup);
			foreach(var c in names)
			{
				var b = GetOrCreateNode(c, lookup);
				var edge = new Edge(a, b);
				a.Edges.Add(edge);
				b.Edges.Add(edge);
			}
		}

		return [.. lookup.Values];
	}

	public override string Process(TextReader reader)
	{
		var nodes = ParseGraph(reader);
		ReduceGraph(nodes, 3);
		return (nodes[0].Value * nodes[1].Value).ToString();
	}

	static Node MergeNodes(Node a, Node b)
	{
		static void ConvertEdges(Node merged, Node a, Node b)
		{
			foreach(var edge in a.Edges)
			{
				var other = edge.Other(a);
				if(other == b) continue;
				var newEdge = new Edge(merged, other);
				merged.Edges.Add(newEdge);
				other.Edges.Remove(edge);
				other.Edges.Add(newEdge);
			}
		}
		var merged = new Node("merged")
		{
			Value = a.Value + b.Value,
		};
		ConvertEdges(merged, a, b);
		ConvertEdges(merged, b, a);
		return merged;
	}

	static void ReduceGraph(List<Node> nodes, int edgesCount)
	{
		while(nodes.Count > 2)
		{
			var wasMerged = false;
			foreach(var a in nodes)
			{
				if(a.Edges.Count <= edgesCount) continue;

				foreach(var conn in a.Edges)
				{
					var b = conn.A == a ? conn.B : conn.A;
					if(b.Edges.Count <= edgesCount) continue;

					var without = new HashSet<Edge>() { conn };
					var found3Paths = true;
					for(int k = 1; k < edgesCount; ++k)
					{
						var path = FindPath(a, b, without);
						if(path is not null)
						{
							foreach(var e in path) without.Add(e);
						}
						else
						{
							found3Paths = false;
							break;
						}
					}
					if(found3Paths && PathExists(a, b, without))
					{
						wasMerged = true;
						nodes.Remove(a);
						nodes.Remove(b);
						nodes.Add(MergeNodes(a, b));
						break;
					}
				}
				if(wasMerged) break;
			}
			if(!wasMerged && nodes.Count != 2)
			{
				throw new Exception("Failed to reduce input graph.");
			}
		}
	}

	static bool PathExists(Node a, Node b, HashSet<Edge> without)
	{
		var queue = new Queue<Node>();
		var visited = new HashSet<Node>();

		queue.Enqueue(a);
		visited.Add(a);

		while(queue.TryDequeue(out var next))
		{
			foreach(var c in next.Edges)
			{
				if(without.Contains(c)) continue;
				var other = c.A == next ? c.B : c.A;
				if(other == b)
				{
					return true;
				}
				if(!visited.Add(other)) continue;
				queue.Enqueue(other);
			}
		}
		return false;
	}

	sealed class Walker(
		Node    node,
		Edge?   edge     = default,
		Walker? previous = default,
		int     score    = 0)
	{
		public Node Node { get; } = node;

		public Edge? Edge { get; } = edge;

		public Walker? Previous { get; } = previous;

		public int Score { get; } = score;

		public Walker Fork(Node node, Edge edge)
			=> new(node, edge, this, Score + 1);

		public List<Edge> ExtractPath()
		{
			var path = new List<Edge>();
			if(Edge is not null)
			{
				path.Add(Edge);
				var prev = Previous;
				while(prev is not null)
				{
					if(prev.Edge is null) break;
					path.Add(prev.Edge);
					prev = prev.Previous;
				}
				path.Reverse();
			}
			return path;
		}
	}

	static List<Edge>? FindPath(Node a, Node b, HashSet<Edge> without)
	{
		var queue  = new Queue<Walker>();
		queue.Enqueue(new Walker(a));

		var bestScore = int.MaxValue;
		var best      = default(Walker);

		var scores = new Dictionary<Node, int>();
		while(queue.TryDequeue(out var walker))
		{
			if(walker.Score >= bestScore) continue;
			foreach(var edge in walker.Node.Edges)
			{
				if(without.Contains(edge)) continue;

				var other = edge.Other(walker.Node);
				if(scores.TryGetValue(other, out var score) && score <= walker.Score + 1)
				{
					continue;
				}

				var fork = walker.Fork(other, edge);
				scores[other] = fork.Score;

				if(other == b)
				{
					if(fork.Score < bestScore)
					{
						bestScore = fork.Score;
						best = fork;
					}
					continue;
				}

				queue.Enqueue(fork);
			}
		}
		return best?.ExtractPath();
	}
}
