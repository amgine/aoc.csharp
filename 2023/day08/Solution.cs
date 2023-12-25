namespace AoC.Year2023;

[Name(@"Haunted Wasteland")]
public abstract class Day8Solution : Solution
{
	protected sealed class Node(string name)
	{
		public string Name { get; } = name;

		public Node Left { get; set; } = default!;

		public Node Right { get; set; } = default!;

		public override string ToString() => Name;
	}

	protected struct InstructionsEnumerator(string instructions)
	{
		private int _index = 0;

		public char Next()
		{
			var value = instructions[_index];
			_index = (_index + 1) % instructions.Length;
			return value;
		}
	}

	protected static Node Next(Node node, char instruction)
		=> instruction switch
		{
			'L' => node.Left,
			'R' => node.Right,
			_   => throw new ArgumentException($"Invalid instruction: {instruction}", nameof(instruction)),
		};

	private static Node ParseNode(Dictionary<string, Node> lookup, string line)
	{
		static Node GetOrCreateNode(Dictionary<string, Node> lookup, string name)
		{
			if(!lookup.TryGetValue(name, out var node))
			{
				lookup.Add(name, node = new(name));
			}
			return node;
		}

		var node   = GetOrCreateNode(lookup, line.Substring( 0, 3));
		node.Left  = GetOrCreateNode(lookup, line.Substring( 7, 3));
		node.Right = GetOrCreateNode(lookup, line.Substring(12, 3));
		return node;
	}

	protected static List<Node> ParseNodes(TextReader reader)
	{
		var lookup = new Dictionary<string, Node>();
		var nodes  = new List<Node>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			nodes.Add(ParseNode(lookup, line));
		}
		return nodes;
	}
}

public sealed class Day8SolutionPart1 : Day8Solution
{
	static bool IsInitial(Node node) => node.Name == "AAA";

	static bool IsTerminal(Node node) => node.Name == "ZZZ";

	public override string Process(TextReader reader)
	{
		var instructions = reader.ReadLine() ?? throw new InvalidDataException();
		var instruction  = new InstructionsEnumerator(instructions);
		var nodes   = ParseNodes(reader);
		var node    = nodes.First(IsInitial);
		var counter = 0L;
		while(!IsTerminal(node))
		{
			node = Next(node, instruction.Next());
			++counter;
		}
		return counter.ToString();
	}
}

public sealed class Day8SolutionPart2 : Day8Solution
{
	static bool IsInitial(Node node) => node.Name.EndsWith('A');

	static bool IsTerminal(Node node) => node.Name.EndsWith('Z');

	static int FindCycleLength(string instructions, Node node)
	{
		int counter     = 0;
		var instruction = new InstructionsEnumerator(instructions);
		while(!IsTerminal(node))
		{
			node = Next(node, instruction.Next());
			++counter;
		}
		return counter;
	}

	public override string Process(TextReader reader)
	{
		var instructions = reader.ReadLine() ?? throw new InvalidDataException();
		var nodes    = ParseNodes(reader);
		var starters = nodes.Where(IsInitial).ToArray();
		var cycles   = Array.ConvertAll(starters, c => (long)FindCycleLength(instructions, c));
		return Mathematics.LCM(cycles).ToString();
	}
}
