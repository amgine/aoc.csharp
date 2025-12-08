namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/8"/></remarks>
[Name(@"Playground")]
public abstract class Day08Solution : Solution
{
	protected static long DistanceSquared(Point3D p0, Point3D p1)
	{
		var dx = (long)(p1.X - p0.X);
		var dy = (long)(p1.Y - p0.Y);
		var dz = (long)(p1.Z - p0.Z);
		return dx * dx + dy * dy + dz * dz;
	}

	protected class Circuit
	{
		public List<Node> Nodes { get; } = [];
	}

	protected class Node(Point3D coordinates)
	{
		public Point3D Coordinates { get; } = coordinates;

		public Circuit? Circuit { get; set; }

		public override string ToString() => Coordinates.ToString();
	}

	protected static List<(Node A, Node B, long Distance)> LoadInput(TextReader reader, out List<Node> nodes)
	{
		    nodes     = LoadListFromNonEmptyStrings(reader, static line => new Node(Parsers.ParsePoint3D(line)));
		var circuits  = new List<Circuit>();
		var distances = new List<(Node A, Node B, long Distance)>();

		for(int i = 0; i < nodes.Count - 1; ++i)
		{
			for(int j = i + 1; j < nodes.Count; ++j)
			{
				var a = nodes[i];
				var b = nodes[j];
				var distance = DistanceSquared(a.Coordinates, b.Coordinates);
				distances.Add((a, b, distance));
			}
		}
		distances.Sort(static (a, b) => a.Distance.CompareTo(b.Distance));
		return distances;
	}

	protected static void Connect(List<Circuit> circuits, Node a, Node b)
	{
		if(a.Circuit is null && b.Circuit is null)
		{
			var c = new Circuit();
			c.Nodes.Add(a);
			c.Nodes.Add(b);
			a.Circuit = c;
			b.Circuit = c;
			circuits.Add(c);
		}
		else if(a.Circuit is not null && b.Circuit is null)
		{
			b.Circuit = a.Circuit;
			a.Circuit.Nodes.Add(b);
		}
		else if(b.Circuit is not null && a.Circuit is null)
		{
			a.Circuit = b.Circuit;
			b.Circuit.Nodes.Add(a);
		}
		else if(a.Circuit is not null && b.Circuit is not null)
		{
			if(a.Circuit == b.Circuit) return;
			circuits.Remove(b.Circuit);
			a.Circuit.Nodes.AddRange(b.Circuit.Nodes);
			foreach(var node in b.Circuit.Nodes)
			{
				node.Circuit = a.Circuit;
			}
		}
	}
}

public sealed class Day08SolutionPart1 : Day08Solution
{
	public static int Solve(TextReader reader, int connections)
	{
		var circuits  = new List<Circuit>();
		var distances = LoadInput(reader, out _);

		foreach(var (a, b, _) in distances)
		{
			if(connections-- <= 0) break;
			Connect(circuits, a, b);
		}

		return circuits
			.OrderByDescending(c => c.Nodes.Count)
			.Take(3)
			.Aggregate(1, static (m, c) => m * c.Nodes.Count);
	}

	public override string Process(TextReader reader)
		=> Solve(reader, connections: 1000).ToString();
}

public sealed class Day08SolutionPart2 : Day08Solution
{
	static long GetAnswer(Node a, Node b)
		=> (long)a.Coordinates.X * (long)b.Coordinates.X;

	public override string Process(TextReader reader)
	{
		var circuits  = new List<Circuit>();
		var distances = LoadInput(reader, out var nodes);

		foreach(var (a, b, _) in distances)
		{
			Connect(circuits, a, b);
			if(circuits.Count == 1 && circuits[0].Nodes.Count == nodes.Count)
			{
				return GetAnswer(a, b).ToString();
			}
		}
		throw new InvalidDataException();
	}
}
