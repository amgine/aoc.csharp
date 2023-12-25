using System;
using System.Numerics;

namespace AoC.Year2022;

[Name(@"Monkey in the Middle")]
public abstract class Day11Solution : Solution
{
	protected class Monkey(int id)
	{
		public int Id { get; } = id;

		public Queue<BigInteger> Items { get; } = [];

		public IOperation Operation { get; set; } = default!;

		public ITest Test { get; set; } = default!;

		public int InspectedItemsCount { get; set; }
	}

	protected interface IOperation
	{
		BigInteger Execute(BigInteger old);
	}

	protected interface ITest
	{
		Monkey Execute(BigInteger value);
	}

	sealed class DivisibleByTest(int op, Monkey monkeyIfTrue, Monkey monkeyIfFalse) : ITest
	{
		public Monkey Execute(BigInteger value)
			=> (value % op) == 0 ? monkeyIfTrue : monkeyIfFalse;
	}

	sealed class Add(IOperand a, IOperand b) : IOperation
	{
		public BigInteger Execute(BigInteger old) => checked(a.Get(old) + b.Get(old));
	}

	sealed class Subtract(IOperand a, IOperand b) : IOperation
	{
		public BigInteger Execute(BigInteger old) => checked(a.Get(old) - b.Get(old));
	}

	sealed class Multiply(IOperand a, IOperand b) : IOperation
	{
		public BigInteger Execute(BigInteger old) => checked(a.Get(old) * b.Get(old));
	}

	sealed class Divide(IOperand a, IOperand b) : IOperation
	{
		public BigInteger Execute(BigInteger old) => checked(a.Get(old) / b.Get(old));
	}

	interface IOperand { BigInteger Get(BigInteger old); }

	sealed class ConstantValue(int value) : IOperand
	{
		public BigInteger Get(BigInteger old) => value;
	}

	sealed class OldValue : IOperand
	{
		public static readonly OldValue Instance = new();

		public BigInteger Get(BigInteger old) => old;
	}

	private static IOperand ParseOperand(ReadOnlySpan<char> operand)
	{
		if(operand.SequenceEqual("old")) return OldValue.Instance;
		return new ConstantValue(int.Parse(operand));
	}

	private static IOperation ParseOperation(string operation)
	{
		var parts = operation.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		if(parts.Length != 3) throw new InvalidDataException();

		var a = ParseOperand(parts[0]);
		var b = ParseOperand(parts[2]);

		return parts[1] switch
		{
			"+" => new Add     (a, b),
			"-" => new Subtract(a, b),
			"*" => new Multiply(a, b),
			"/" => new Divide  (a, b),
			_ => throw new InvalidDataException(),
		};
	}

	private static Monkey ParseMonkey(int id, TextReader reader, Dictionary<int, Monkey> lookup)
	{
		static Monkey GetMonkey(int id, Dictionary<int, Monkey> lookup)
		{
			if(!lookup.TryGetValue(id, out var monkey))
			{
				lookup.Add(id, monkey = new(id));
			}
			return monkey;
		}

		var monkey = GetMonkey(id, lookup);

		var itemsLine = reader.ReadLine() ?? throw new InvalidDataException();
		if(!itemsLine.StartsWith("  Starting items: ")) throw new InvalidDataException();
		var items = Array.ConvertAll(itemsLine.Substring("  Starting items: ".Length).Split(',', StringSplitOptions.TrimEntries), int.Parse);
		foreach(var item in items)
		{
			monkey.Items.Enqueue(item);
		}

		var operationLine = reader.ReadLine() ?? throw new InvalidDataException();
		if(!operationLine.StartsWith("  Operation: new = ")) throw new InvalidDataException();
		monkey.Operation = ParseOperation(operationLine.Substring("  Operation: new = ".Length));

		var testLine = reader.ReadLine() ?? throw new InvalidDataException();
		if(!testLine.StartsWith("  Test: divisible by ")) throw new InvalidDataException();
		int testValue = int.Parse(testLine.AsSpan("  Test: divisible by ".Length));

		var testTrueLine = reader.ReadLine() ?? throw new InvalidDataException();
		if(!testTrueLine.StartsWith("    If true: throw to monkey ")) throw new InvalidDataException();
		var trueId = int.Parse(testTrueLine.AsSpan("    If true: throw to monkey ".Length));

		var testFalseLine = reader.ReadLine() ?? throw new InvalidDataException();
		if(!testFalseLine.StartsWith("    If false: throw to monkey ")) throw new InvalidDataException();
		var falseId = int.Parse(testFalseLine.AsSpan("    If false: throw to monkey ".Length));

		monkey.Test = new DivisibleByTest(testValue, GetMonkey(trueId, lookup), GetMonkey(falseId, lookup));

		return monkey;
	}

	protected static Dictionary<int, Monkey> ParseMonkeys(TextReader reader)
	{
		var lookup = new Dictionary<int, Monkey>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			const string MonkeyPrefix = "Monkey ";

			if(line.Length == 0) continue;
			if(line.StartsWith(MonkeyPrefix))
			{
				var id = int.Parse(line.AsSpan(MonkeyPrefix.Length, line.Length - MonkeyPrefix.Length - 1));
				ParseMonkey(id, reader, lookup);
				continue;
			}
			throw new InvalidDataException();
		}
		return lookup;
	}

	protected static long GetMonkeyBusiness(IEnumerable<Monkey> monkeys)
		=> monkeys.Select(m => m.InspectedItemsCount).OrderByDescending(c => c).Take(2).Aggregate(1L, (a, b) => a * b);
}

public class Day11SolutionPart1 : Day11Solution
{
	private static void InspectItems(Monkey monkey)
	{
		while(monkey.Items.Count > 0)
		{
			var item = monkey.Items.Dequeue();
			var lvl = monkey.Operation.Execute(item);
			lvl /= 3;
			var other = monkey.Test.Execute(lvl);
			other.Items.Enqueue(lvl);
			monkey.InspectedItemsCount++;
		}
	}

	public override string Process(TextReader reader)
	{
		var monkeys = ParseMonkeys(reader);
		for(int round = 0; round < 20; ++round)
		{
			for(int i = 0; i < monkeys.Count; ++i)
			{
				var monkey = monkeys[i];
				InspectItems(monkey);
			}
		}
		return GetMonkeyBusiness(monkeys.Values).ToString();
	}
}

public class Day11SolutionPart2 : Day11Solution
{
	private static void InspectItems(Monkey monkey)
	{
		while(monkey.Items.Count > 0)
		{
			var item = monkey.Items.Dequeue();
			var lvl = monkey.Operation.Execute(item);
			var other = monkey.Test.Execute(lvl);
			other.Items.Enqueue(lvl);
			monkey.InspectedItemsCount++;
		}
	}

	public override string Process(TextReader reader)
	{
		throw new NotImplementedException();

		var monkeys = ParseMonkeys(reader);
		for(int round = 0; round < 10000; ++round)
		{
			for(int i = 0; i < monkeys.Count; ++i)
			{
				var monkey = monkeys[i];
				InspectItems(monkey);
			}
		}
		return GetMonkeyBusiness(monkeys.Values).ToString();
	}
}
