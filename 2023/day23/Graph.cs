namespace AoC.Year2023;

interface IMovesProvider
{
	Span<Point2D> GetMoves(HashSet<Point2D> visited, Point2D position, Span<Point2D> buffer);
}

sealed class Node(Point2D position)
{
	public Point2D Position { get; } = position;

	public List<Edge> Edges { get; } = [];

	public override string ToString() => Position.ToString();
}

sealed class NodeWalker
{
	private readonly HashSet<Node> _visited;

	public NodeWalker(Node node)
	{
		Node = node;
		_visited = [node];
	}

	private NodeWalker(NodeWalker other)
	{
		Node = other.Node;
		Length = other.Length;
		_visited = new(other._visited);
	}

	public int Length { get; set; }

	public Node Node { get; private set; }

	public HashSet<Node> Visited => _visited;

	public NodeWalker Fork(Edge edge)
	{
		var w = new NodeWalker(this);
		w.Walk(edge);
		return w;
	}

	public void Walk(Edge edge)
	{
		Node = edge.Node;
		Length += edge.Length + 1;
		_visited.Add(edge.Node);
	}
}

readonly record struct Edge(Node Node, int Length)
{
	public override string ToString() => $"-> {Length} -> {Node}";
}

static class Graph
{
	public static int FindLongestPathLength(Node startNode, Node finishNode)
	{
		var longest = int.MinValue;

		var stack = new Stack<NodeWalker>();
		stack.Push(new NodeWalker(startNode));

		var possible = new List<Edge>();
		while(stack.TryPop(out var walker))
		{
			possible.Clear();
			foreach(var edge in walker.Node.Edges)
			{
				if(!walker.Visited.Contains(edge.Node))
				{
					possible.Add(edge);
				}
			}
			if(possible.Count == 0) continue;
			if(possible.Count == 1)
			{
				walker.Walk(possible[0]);
				if(possible[0].Node == finishNode)
				{
					if(walker.Length > longest)
					{
						longest = walker.Length;
					}
				}
				else
				{
					stack.Push(walker);
				}
			}
			else
			{
				for(int i = 1; i < possible.Count; ++i)
				{
					stack.Push(walker.Fork(possible[i]));
				}
				walker.Walk(possible[0]);
				stack.Push(walker);
			}
		}

		return longest;
	}
}

static class GraphBuilder
{
	public static Node[] Build(char[,] map, IMovesProvider moves)
	{
		var start  = FindStart(map);
		var finish = FindFinish(map);
		var splits = FindSplits(map, moves);

		splits.Insert(0, start);
		splits.Add(finish);

		var edges = FindEdges(splits, moves);
		var nodes = CreateNodes(splits, edges);

		OptimizeGraph(nodes);

		return nodes;
	}

	static Point2D FindStart(char[,] map)
	{
		for(int i = 0; i < map.GetLength(1); i++)
		{
			if(map[0, i] == '.') return new(i, 0);
		}
		throw new InvalidDataException($"Start node does not exist.");
	}

	static Point2D FindFinish(char[,] map)
	{
		var y = map.GetLength(0) - 1;
		for(int i = 0; i < map.GetLength(1); i++)
		{
			if(map[y, i] == '.') return new(i, y);
		}
		throw new InvalidDataException($"Finish node does not exist.");
	}

	static List<Point2D> FindSplits(char[,] map, IMovesProvider movesProvider)
	{
		Span<Point2D> moves = stackalloc Point2D[4];
		var emptySet = new HashSet<Point2D>();

		var splits = new List<Point2D>();
		for(int y = 0; y < map.GetLength(0); y++)
		{
			for(int x = 0; x < map.GetLength(1); x++)
			{
				var pos = new Point2D(x, y);
				if(pos.GetValue(map) == '#') continue;
				var possible = movesProvider.GetMoves(emptySet, pos, moves);
				if(possible.Length > 2) splits.Add(pos);
			}
		}
		return splits;
	}

	static Dictionary<(int, int), int> FindEdges(List<Point2D> splits, IMovesProvider movesProvider)
	{
		var splitSet = new HashSet<Point2D>(splits);
		var emptySet = new HashSet<Point2D>();

		var edges = new Dictionary<(int, int), int>();

		Span<Point2D> moves = stackalloc Point2D[4];
		Span<Point2D> moves2 = stackalloc Point2D[4];

		foreach(var split in splits)
		{
			var possible = movesProvider.GetMoves(emptySet, split, moves);
			foreach(var r in possible)
			{
				var position = r;
				var visited = new HashSet<Point2D>() { split, r };
				var len = 0;

				while(true)
				{
					if(splitSet.Contains(position))
					{
						if(position == split)
						{
							break;
						}

						var key = (splits.IndexOf(split), splits.IndexOf(position));
						edges.Add(key, len);
						break;
					}

					var next = movesProvider.GetMoves(visited, position, moves2);
					if(next.Length == 0)
					{
						// dead end
						break;
					}
					if(next.Length > 1)
					{
						throw new ApplicationException();
					}
					position = next[0];
					visited.Add(position);
					++len;
				}
			}
		}
		return edges;
	}

	static Node[] CreateNodes(List<Point2D> splits, Dictionary<(int, int), int> edges)
	{
		var nodes = new Node[splits.Count];
		for(int i = 0; i < splits.Count - 1; ++i)
		{
			var a = nodes[i] ??= new Node(splits[i]);
			for(int j = i + 1; j < splits.Count; ++j)
			{
				var b = nodes[j] ??= new Node(splits[j]);
				if(edges.TryGetValue((i, j), out var len1))
				{
					a.Edges.Add(new Edge(b, len1));
				}
				if(edges.TryGetValue((j, i), out var len2))
				{
					b.Edges.Add(new Edge(a, len2));
				}
			}
		}
		return nodes;
	}

	static void OptimizeGraph(Node[] nodes)
	{
		// no point in going back to start
		foreach(var e in nodes[0].Edges)
		{
			e.Node.Edges.RemoveAll(e => e.Node == nodes[0]);
		}

		if(nodes[0].Edges.Count == 1)
		{
			// if there is only 1 way from start, then next node can only be
			// entered from start
			var next = nodes[0].Edges[0].Node;
			foreach(var e in next.Edges)
			{
				e.Node.Edges.RemoveAll(e => e.Node == next);
			}
		}

		if(nodes[^1].Edges.Count == 1)
		{
			// if only 1 path leads to finish, then reaching pre-finish node
			// means that we have to go to finish, otherwise we'll have
			// to visit pre-finish node twice
			nodes[^1].Edges[0].Node.Edges.RemoveAll(e => e.Node != nodes[^1]);
		}
	}
}
