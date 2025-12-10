using System.Collections.Immutable;
using System.Collections.Specialized;

using Microsoft.Z3;

namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/10"/></remarks>
[Name(@"Factory")]
public abstract class Day10Solution : Solution
{
	protected sealed class Machine(ImmutableArray<bool> requiredIndicators, ImmutableArray<ImmutableArray<int>> buttons, ImmutableArray<int> joltage)
	{
		private static BitVector32 ToVector(ImmutableArray<bool> value)
		{
			var vec = new BitVector32();
			for(int i = 0; i < value.Length; i++)
			{
				if(value[i]) vec[1 << i] = true;
			}
			return vec;
		}

		public BitVector32 RequiredState { get; } = ToVector(requiredIndicators);

		public ImmutableArray<bool> RequiredIndicators { get; } = requiredIndicators;

		public ImmutableArray<ImmutableArray<int>> Buttons { get; } = buttons;

		public ImmutableArray<int> JoltageRequirements { get; } = joltage;

		private static ImmutableArray<bool> ParseRequired(ReadOnlySpan<char> slice)
		{
			var b = ImmutableArray.CreateBuilder<bool>();
			for(int i = 0; i < slice.Length; ++i)
			{
				b.Add(slice[i] switch
				{
					'.' => false,
					'#' => true,
					_   => throw new InvalidDataException(),
				});
			}
			return b.ToImmutable();
		}

		private static ImmutableArray<int> ParseIntList(ReadOnlySpan<char> slice)
		{
			var b = ImmutableArray.CreateBuilder<int>();
			while(slice.Length != 0)
			{
				var sep = slice.IndexOf(',');
				if(sep < 0)
				{
					b.Add(int.Parse(slice));
					break;
				}
				b.Add(int.Parse(slice.Slice(0, sep)));
				slice = slice.Slice(sep + 1);
			}
			return b.ToImmutable();
		}

		public static Machine Parse(ReadOnlySpan<char> line)
		{
			var required = ImmutableArray<bool>.Empty;
			var buttons = ImmutableArray.CreateBuilder<ImmutableArray<int>>();
			var joltage = ImmutableArray<int>.Empty;
			while(line.Length != 0)
			{
				if(char.IsWhiteSpace(line[0]))
				{
					line = line[1..];
					continue;
				}
				switch(line[0])
				{
					case '[':
						{
							var end = line.IndexOf(']');
							required = ParseRequired(line.Slice(1, end - 1));
							line = line.Slice(end + 1);
						}
						break;
					case '(':
						{
							var end = line.IndexOf(')');
							var b = ParseIntList(line.Slice(1, end - 1));
							buttons.Add(b);
							line = line.Slice(end + 1);
						}
						break;
					case '{':
						{
							var end = line.IndexOf('}');
							joltage = ParseIntList(line.Slice(1, end - 1));
							line = line.Slice(end + 1);
						}
						break;
				}
			}
			return new(required, buttons.ToImmutable(), joltage);
		}
	}
}

public sealed class Day10SolutionPart1 : Day10Solution
{
	static BitVector32 Simulate(BitVector32 state, ImmutableArray<int> button)
	{
		for(int i = 0; i < button.Length; i++)
		{
			var b = 1 << button[i];
			state[b] = !state[b];
		}
		return state;
	}

	static int Iterate(Machine machine, BitVector32 state, Dictionary<BitVector32, int> cache, int count)
	{
		var nc = count + 1;
		var best = -1;
		foreach(var button in machine.Buttons)
		{
			var next = Simulate(state, button);
			if(cache.TryGetValue(next, out var c))
			{
				if(c <= nc) continue;
				cache[next] = nc;
			}
			else
			{
				cache.Add(next, nc);
			}
			if(next.Data == machine.RequiredState.Data)
			{
				return nc;
			}
			var p = Iterate(machine, next, cache, nc);
			if(best == -1 || (p != -1 && p < best)) best = p;
		}
		return best;
	}

	private static int Solve(Machine machine)
	{
		var cache = new Dictionary<BitVector32, int>();
		cache.Add(default, 0);
		return Iterate(machine, default, cache, 0);
	}

	public override string Process(TextReader reader)
	{
		var machines = LoadListFromNonEmptyStrings(reader, static line => Machine.Parse(line));
		var count = 0L;
		foreach(var machine in machines)
		{
			var c = Solve(machine);
			count += c;
		}
		return count.ToString();
	}
}

public sealed class Day10SolutionPart2 : Day10Solution
{
	private static int Solve(Machine machine)
	{
		using var context = new Context();

		var result = new IntNum[machine.JoltageRequirements.Length];
		for(int i = 0; i < result.Length; ++i)
		{
			result[i] = context.MkInt(machine.JoltageRequirements[i]);
		}

		var vars = new List<IntExpr>[machine.JoltageRequirements.Length];
		var btns = new List<IntExpr>();
		for(int i = 0; i < machine.Buttons.Length; ++i)
		{
			var b = context.MkIntConst($"b{i}");
			btns.Add(b);
			foreach(var index in machine.Buttons[i])
			{
				(vars[index] ??= []).Add(b);
			}
		}

		using var solver = context.MkOptimize();
		using var sum = context.MkAdd([.. btns]);
		solver.MkMinimize(sum);
		for(int i = 0; i < vars.Length; ++i)
		{
			var bsum = context.MkAdd([.. vars[i]]);
			var eq = context.MkEq(bsum, result[i]);
			solver.Add(eq);
		}
		var zero = context.MkInt(0);
		foreach(var b in btns)
		{
			BoolExpr e = b >= 0;
			solver.Add(e);
		}
		if(solver.Check() != Status.SATISFIABLE)
		{
			throw new InvalidDataException("Cannot solve for the specified data.");
		}

		using var m = solver.Model;
		using var res = m.Eval(sum);

		return (res is IntNum x) ? x.Int : 0;
	}

	public override string Process(TextReader reader)
	{
		var machines = LoadListFromNonEmptyStrings(reader, static line => Machine.Parse(line));
		var count = 0L;
		foreach(var machine in machines)
		{
			var c = Solve(machine);
			count += c;
		}
		return count.ToString();
	}
}
