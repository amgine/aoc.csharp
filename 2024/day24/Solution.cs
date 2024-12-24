using System.Collections.Immutable;

namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/24"/></remarks>
[Name(@"Crossed Wires")]
public abstract class Day24Solution : Solution
{
	protected abstract class Gate(Wire op0, Wire op1, Wire dst)
	{
		public Wire Op0 { get; } = op0;

		public Wire Op1 { get; } = op1;

		public Wire Dst { get; set; } = dst;

		public abstract bool Exec(bool a, bool b);

		public bool HasInputs(Wire a, Wire b)
			=> HasInput(a) && HasInput(b);

		public bool HasInput(Wire a)
			=> Op0 == a || Op1 == a;

		public void Exec()
		{
			Dst.Value    = Exec(Op0.Value, Op1.Value);
			Dst.HasValue = true;
		}
	}

	protected sealed class Xor(Wire a, Wire b, Wire dst) : Gate(a, b, dst)
	{
		public override bool Exec(bool a, bool b) => a ^ b;

		public override string ToString() => $"{Op0} XOR {Op1} -> {Dst}";
	}

	protected sealed class And(Wire a, Wire b, Wire dst) : Gate(a, b, dst)
	{
		public override bool Exec(bool a, bool b) => a && b;

		public override string ToString() => $"{Op0} AND {Op1} -> {Dst}";
	}

	protected sealed class Or(Wire a, Wire b, Wire dst) : Gate(a, b, dst)
	{
		public override bool Exec(bool a, bool b) => a || b;

		public override string ToString() => $"{Op0} OR {Op1} -> {Dst}";
	}

	protected sealed class Wire(string name, bool hasValue = false, bool value = false)
	{
		private readonly bool _initialHasValue = hasValue;
		private readonly bool _initialValue = value;

		public string Name { get; } = name;

		public bool HasValue { get; set; } = hasValue;

		public bool Value { get; set; } = value;

		public void Reset()
		{
			HasValue = _initialHasValue;
			Value = _initialValue;
		}

		public override string ToString() => Name;
	}

	protected sealed class Input(Dictionary<string, Wire> wireLookup, ImmutableArray<Gate> gates)
	{
		public Dictionary<string, Wire> WireLookup { get; } = wireLookup;

		public ImmutableArray<Gate> Gates { get; } = gates;
	}

	private static Dictionary<string, Wire> ParseWires(TextReader reader)
	{
		var wires = new Dictionary<string, Wire>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) break;
			var p = line.IndexOf(':');
			var name = line[..p];
			var v = int.Parse(line.AsSpan(p + 1).Trim()) != 0;
			wires.Add(name, new(name, true, v));
		}
		return wires;
	}

	private static Gate ParseGate(Dictionary<string, Wire> wires, string line)
	{
		static Wire GetOrAddWire(Dictionary<string, Wire> wires, string name)
		{
			if(!wires.TryGetValue(name, out var wire))
			{
				wires.Add(name, wire = new Wire(name));
			}
			return wire;
		}

		var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		var op0 = GetOrAddWire(wires, parts[0]);
		var op1 = GetOrAddWire(wires, parts[2]);
		var dst = GetOrAddWire(wires, parts[4]);

		return parts[1] switch
		{
			"AND" => new And(op0, op1, dst),
			"OR"  => new Or (op0, op1, dst),
			"XOR" => new Xor(op0, op1, dst),
			_ => throw new InvalidDataException(),
		};
	}

	protected static Input ParseInput(TextReader reader)
	{
		var wires = ParseWires(reader);
		var gates = ImmutableArray.CreateBuilder<Gate>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			gates.Add(ParseGate(wires, line));
		}
		return new(wires, gates.ToImmutable());
	}

	protected static long GetValue(Wire[] wires)
	{
		var bit   = 1L;
		var value = 0L;
		foreach(var w in wires)
		{
			if(w.Value) value |= bit;
			bit <<= 1;
		}
		return value;
	}

	protected static void Run(ImmutableArray<Gate> gates)
	{
		var toRun = new List<Gate>();
		while(true)
		{
			toRun.Clear();
			foreach(var gate in gates)
			{
				if(gate.Op0.HasValue && gate.Op1.HasValue && !gate.Dst.HasValue)
				{
					toRun.Add(gate);
				}
			}
			if(toRun.Count == 0) break;
			foreach(var gate in toRun)
			{
				gate.Exec();
			}
		}
	}

	protected static Wire[] GetWires(IEnumerable<Wire> wires, char prefix)
	{
		var prefixed = wires.Where(w => w.Name.StartsWith(prefix)).ToArray();
		Array.Sort(prefixed, static (a, b) => a.Name.CompareTo(b.Name));
		return prefixed;
	}
}

public sealed class Day24SolutionPart1 : Day24Solution
{
	public override string Process(TextReader reader)
	{
		var input = ParseInput(reader);
		Run(input.Gates);
		var z = GetWires(input.WireLookup.Values, 'z');
		return GetValue(z).ToString();
	}
}

public sealed class Day24SolutionPart2 : Day24Solution
{
	public override string Process(TextReader reader)
	{
		var input = ParseInput(reader);

		/*
		 * TODO : implement better pattern matching
		     XOR -> z{i}
		        x{i} XOR y{i} -> xor{i}
		        OR            ->  or{i}
		            x{i-1}   AND y{i-1}
		            xor{i-1} AND or{i-1}

		*/


		var gates = input.Gates.ToArray();

		var x = GetWires(input.WireLookup.Values, 'x');
		var y = GetWires(input.WireLookup.Values, 'y');
		var z = GetWires(input.WireLookup.Values, 'z');

		var invalid = new HashSet<Gate>();
		var valid   = new HashSet<Gate>();

		Wire? prevXor, prevOr;
		Wire? curXor = default, curOr = default;

		for(int i = 0; i < z.Length; ++i)
		{
			prevXor = curXor;
			prevOr  = curOr;

			curOr   = default;
			curXor  = default;

			var zw = z[i];
			var root = input.Gates.Single(g => g.Dst == zw);
			if(i == 0)
			{
				if(root is not Xor || !root.HasInputs(x[0], y[0]))
				{
					invalid.Add(root);
				}
				else
				{
					valid.Add(root);
				}
			}
			else if(i == 1)
			{

			}
			else if(i == 2)
			{

			}
			else if(i == z.Length - 1)
			{
				
			}
			else
			{
				if(root is not Xor || root.Op0.Name.StartsWith('x') || root.Op1.Name.StartsWith('x') || root.Op0.Name.StartsWith('y') || root.Op1.Name.StartsWith('y'))
				{
					invalid.Add(root);
				}
				else
				{
					var g0 = gates.Single(g => g.Dst == root.Op0);
					var g1 = gates.Single(g => g.Dst == root.Op1);
					var xor0 = g0 as Xor ?? g1 as Xor;
					if(xor0 is null)
					{
						var and = g0 as And ?? g1 as And;
						if(and is not null)
						{
							if(and.HasInputs(x[i], y[i]))
							{
								invalid.Add(and);
							}
						}
						// !
					}
					else if(!xor0.HasInputs(x[i], y[i]))
					{
						invalid.Add(xor0);
					}
					else
					{
						curXor = xor0.Dst;
						valid.Add(xor0);
					}
					var or0 = g0 as Or ?? g1 as Or;
					if(or0 is null)
					{
						// !
					}
					else
					{
						curOr = or0.Dst;
						var and0 = gates.Single(g => g.Dst == or0.Op0);
						var and1 = gates.Single(g => g.Dst == or0.Op1);
						if(and0 is And && and1 is And)
						{
							if(and1.HasInputs(x[i - 1], y[i - 1]))
							{
								(and0, and1) = (and1, and0);
							}
							if(!and0.HasInputs(x[i - 1], y[i - 1]))
							{
								invalid.Add(and0);
							}
							if(prevOr is not null && prevXor is not null && and1.HasInputs(prevOr, prevXor))
							{
								valid.Add(and1);
							}
							else
							{
								if(prevOr is not null)
								{
									if(!and1.HasInput(prevOr))
									{
										invalid.Add(and1);
									}
								}
								if(prevXor is not null)
								{
									if(!and1.HasInput(prevXor))
									{
										invalid.Add(and1);
									}
								}
							}
						}
						else
						{
							if(and0 is And)
							{
								invalid.Add(and1);
							}
							else
							{
								invalid.Add(and0);
							}
						}
					}
				}
			}
		}

		return string.Join(',', invalid.Select(w => w.Dst.Name).Order());
	}
}
