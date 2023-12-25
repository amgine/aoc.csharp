using System.Collections;

namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/8"/></remarks>
[Name(@"Handheld Halting")]
public abstract class Day08Solution : Solution
{
	protected enum OpCode
	{
		nop,
		acc,
		jmp,
	}

	protected readonly record struct Instruction(OpCode OpCode, int Argument);

	private static OpCode ParseOpCode(ReadOnlySpan<char> text)
		=> text switch
		{
			@"nop" => OpCode.nop,
			@"acc" => OpCode.acc,
			@"jmp" => OpCode.jmp,
			_ => throw new InvalidDataException($"Unknown instruction: {new string(text)}")
		};

	static Instruction ParseInstruction(string line)
	{
		var opCode   = ParseOpCode(line.AsSpan(0, 3));
		var argument = int.Parse(line.AsSpan(4));
		return new(opCode, argument);
	}

	protected static List<Instruction> ParseInstructions(TextReader reader)
	{
		var instructions = new List<Instruction>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			instructions.Add(ParseInstruction(line));
		}
		return instructions;
	}

	protected enum ExecutionResult { RanToCopmpletion, Loop, Error, }

	protected class Cpu
	{
		private BitArray? _executed;

		public ExecutionResult Execute(
			List<Instruction> instructions, out int accumulator)
		{
			accumulator = 0;

			var curr = 0;
			if(_executed is null || _executed.Length < instructions.Count)
			{
				_executed = new BitArray(instructions.Count);
			}
			else
			{
				_executed.SetAll(false);
			}
			while(curr < instructions.Count)
			{
				if(curr < 0) return ExecutionResult.Error;

				if(_executed[curr]) return ExecutionResult.Loop;
				_executed[curr] = true;

				var i = instructions[curr];
				switch(i.OpCode)
				{
					case OpCode.jmp:
						curr += i.Argument;
						break;
					case OpCode.acc:
						accumulator += i.Argument;
						++curr;
						break;
					case OpCode.nop:
						++curr;
						break;
					default: throw new ApplicationException(
						$"Unknown opcode: {i.OpCode}");
				}
			}

			return curr == instructions.Count
				? ExecutionResult.RanToCopmpletion
				: ExecutionResult.Error;
		}
	}
}

public sealed class Day08SolutionPart1 : Day08Solution
{
	public override string Process(TextReader reader)
	{
		var instructions = ParseInstructions(reader);
		var cpu = new Cpu();
		if(cpu.Execute(instructions, out var acc) != ExecutionResult.Loop)
		{
			throw new InvalidDataException($"Expected infinite loop.");
		}
		return acc.ToString();
	}
}

public sealed class Day08SolutionPart2 : Day08Solution
{
	private static Instruction Swap(Instruction instruction)
		=> instruction.OpCode switch
		{
			OpCode.jmp => new(OpCode.nop, instruction.Argument),
			OpCode.nop => new(OpCode.jmp, instruction.Argument),
			_ => throw new ArgumentException(@"Expected jmp or nop.", nameof(instruction)),
		};

	public override string Process(TextReader reader)
	{
		var instructions = ParseInstructions(reader);
		var cpu = new Cpu();
		for(int i = 0; i < instructions.Count; ++i)
		{
			var instruction = instructions[i];

			switch(instruction.OpCode)
			{
				case OpCode.jmp or OpCode.nop:
					instructions[i] = Swap(instruction);
					break;
				default: continue;
			}

			if(cpu.Execute(instructions, out var acc) == ExecutionResult.RanToCopmpletion)
			{
				return acc.ToString();
			}

			instructions[i] = instruction;
		}
		throw new InvalidDataException();
	}
}
