using System.Diagnostics.CodeAnalysis;

namespace AoC.Year2018;

[Name(@"Chronal Classification")]
public abstract class Day16Solution : Solution
{
	protected readonly record struct Instruction(byte OpCode, int A, int B, int C);

	protected sealed class Sample(int[] before, Instruction instruction, int[] after)
	{
		public int[] Before { get; } = before;

		public int[] After { get; } = after;

		public Instruction Instruction { get; } = instruction;
	}

	protected static readonly IExecutor[] Executors =
		[
			new addr(),
			new addi(),
			new mulr(),
			new muli(),
			new banr(),
			new bani(),
			new borr(),
			new bori(),
			new setr(),
			new seti(),
			new gtir(),
			new gtri(),
			new gtrr(),
			new eqri(),
			new eqir(),
			new eqrr(),
		];

	protected interface IExecutor
	{
		bool TryExecute(Instruction instruction, Span<int> registers);
	}

	static bool IsValidAddress(int address, Span<int> registers)
		=> address >= 0 && address < registers.Length;

	abstract class RegisterInstructionExecutor : IExecutor
	{
		protected abstract int Calculate(int a, int b);

		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.A, registers)) return false;
			if(!IsValidAddress(instruction.B, registers)) return false;
			if(!IsValidAddress(instruction.C, registers)) return false;

			registers[instruction.C] = Calculate(registers[instruction.A], registers[instruction.B]);
			return true;
		}
	}

	abstract class ImmediateInstructionExecutor : IExecutor
	{
		protected abstract int Calculate(int a, int b);

		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.A, registers)) return false;
			if(!IsValidAddress(instruction.C, registers)) return false;

			registers[instruction.C] = Calculate(registers[instruction.A], instruction.B);
			return true;
		}
	}

#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles

	sealed class addr : RegisterInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a + b;

		public override string ToString() => nameof(addr);
	}

	sealed class addi : ImmediateInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a + b;

		public override string ToString() => nameof(addi);
	}

	sealed class mulr : RegisterInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a * b;

		public override string ToString() => nameof(mulr);
	}

	sealed class muli : ImmediateInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a * b;

		public override string ToString() => nameof(mulr);
	}

	sealed class banr : RegisterInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a & b;

		public override string ToString() => nameof(banr);
	}

	sealed class bani : ImmediateInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a & b;

		public override string ToString() => nameof(bani);
	}

	sealed class borr : RegisterInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a | b;

		public override string ToString() => nameof(borr);
	}

	sealed class bori : ImmediateInstructionExecutor
	{
		protected override int Calculate(int a, int b) => a | b;

		public override string ToString() => nameof(bori);
	}

	sealed class setr : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;
			if(!IsValidAddress(instruction.A, registers)) return false;

			registers[instruction.C] = registers[instruction.A];
			return true;
		}

		public override string ToString() => nameof(setr);
	}

	sealed class seti : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;

			registers[instruction.C] = instruction.A;
			return true;
		}

		public override string ToString() => nameof(seti);
	}

	sealed class gtir : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;
			if(!IsValidAddress(instruction.B, registers)) return false;

			registers[instruction.C] = instruction.A > registers[instruction.B] ? 1 : 0;
			return true;
		}

		public override string ToString() => nameof(gtir);
	}

	sealed class gtri : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;
			if(!IsValidAddress(instruction.A, registers)) return false;

			registers[instruction.C] = registers[instruction.A] > instruction.B ? 1 : 0;
			return true;
		}

		public override string ToString() => nameof(gtri);
	}

	sealed class gtrr : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;
			if(!IsValidAddress(instruction.A, registers)) return false;
			if(!IsValidAddress(instruction.B, registers)) return false;

			registers[instruction.C] = registers[instruction.A] > registers[instruction.B] ? 1 : 0;
			return true;
		}

		public override string ToString() => nameof(gtrr);
	}

	sealed class eqir : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;
			if(!IsValidAddress(instruction.B, registers)) return false;

			registers[instruction.C] = instruction.A == registers[instruction.B] ? 1 : 0;
			return true;
		}

		public override string ToString() => nameof(eqir);
	}

	sealed class eqri : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;
			if(!IsValidAddress(instruction.A, registers)) return false;

			registers[instruction.C] = registers[instruction.A] == instruction.B ? 1 : 0;
			return true;
		}

		public override string ToString() => nameof(eqri);
	}

	sealed class eqrr : IExecutor
	{
		public bool TryExecute(Instruction instruction, Span<int> registers)
		{
			if(!IsValidAddress(instruction.C, registers)) return false;
			if(!IsValidAddress(instruction.A, registers)) return false;
			if(!IsValidAddress(instruction.B, registers)) return false;

			registers[instruction.C] = registers[instruction.A] == registers[instruction.B] ? 1 : 0;
			return true;
		}

		public override string ToString() => nameof(eqrr);
	}

#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.

	protected static Instruction ParseInstruction(ReadOnlySpan<char> line)
	{
		Span<Range> ranges = stackalloc Range[4];
		if(line.Split(ranges, ' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) != 4)
		{
			throw new InvalidDataException();
		}
		return new(
			OpCode: byte.Parse(line[ranges[0]]),
			A: int.Parse(line[ranges[1]]),
			B: int.Parse(line[ranges[2]]),
			C: int.Parse(line[ranges[3]]));
	}

	protected static int[] ParseSampleRegisters(ReadOnlySpan<char> line)
	{
		var p1 = line.IndexOf('[');
		var p2 = line.LastIndexOf(']');
		line = line.Slice(p1 + 1, p2 - p1 - 1);
		Span<Range> ranges = stackalloc Range[4];
		if(line.Split(ranges, ',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) != 4)
		{
			throw new InvalidDataException();
		}
		return [
			int.Parse(line[ranges[0]]),
			int.Parse(line[ranges[1]]),
			int.Parse(line[ranges[2]]),
			int.Parse(line[ranges[3]])];
	}

	protected static bool TryParseSample(TextReader reader, [MaybeNullWhen(returnValue: false)] out Sample sample)
	{
		var before = reader.ReadLine();
		if(before is not { Length: not 0 }) goto no_data;
		var instruction = reader.ReadLine();
		if(instruction is not { Length: not 0 }) goto no_data;
		var after = reader.ReadLine();
		if(after is not { Length: not 0 }) goto no_data;
		reader.ReadLine();

		sample = new(
			before:      ParseSampleRegisters(before),
			instruction: ParseInstruction(instruction),
			after:       ParseSampleRegisters(after));
		return true;

		no_data:
		sample = default;
		return false;
	}
}

public sealed class Day16SolutionPart1 : Day16Solution
{
	static int CountValidOpCodeVariants(Sample sample)
	{
		var count = 0;
		Span<int> registers = stackalloc int[4];
		foreach(var opcode in Executors)
		{
			sample.Before.CopyTo(registers);
			if(!opcode.TryExecute(sample.Instruction, registers))
			{
				continue;
			}
			if(registers.SequenceEqual(sample.After))
			{
				++count;
			}
		}
		return count;
	}

	public override string Process(TextReader reader)
	{
		var samples = new List<Sample>();
		while(TryParseSample(reader, out var sample))
		{
			samples.Add(sample);
		}

		var count = 0;
		foreach(var sample in samples)
		{
			if(CountValidOpCodeVariants(sample) >= 3)
			{
				++count;
			}
		}

		return count.ToString();
	}
}

public sealed class Day16SolutionPart2 : Day16Solution
{
	static void Execute(Span<int> registers, Dictionary<byte, IExecutor> mapping, TextReader reader)
	{
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var instruction = ParseInstruction(line);

			if(!mapping[instruction.OpCode].TryExecute(instruction, registers))
			{
				throw new InvalidDataException();
			}
		}
	}

	static List<IExecutor> GetValidOpCodeVariants(Sample sample)
	{
		var valid = new List<IExecutor>();
		Span<int> registers = stackalloc int[4];
		foreach(var opcode in Executors)
		{
			sample.Before.CopyTo(registers);
			if(!opcode.TryExecute(sample.Instruction, registers))
			{
				continue;
			}
			if(registers.SequenceEqual(sample.After))
			{
				valid.Add(opcode);
			}
		}
		return valid;
	}

	static Dictionary<byte, IExecutor> GetMapping(List<Sample> samples)
	{
		var mapping = new Dictionary<byte, IExecutor>();

		var possible = new HashSet<IExecutor>?[16];

		foreach(var sample in samples)
		{
			var valid = GetValidOpCodeVariants(sample);
			var set = possible[sample.Instruction.OpCode];
			if(set is null)
			{
				possible[sample.Instruction.OpCode] = new(valid);
			}
			else
			{
				set.IntersectWith(valid);
			}
		}

		var prev = -1;
		while(true)
		{
			var mapped = 0;
			for(int i = 0; i < 16; ++i)
			{
				if(possible[i] is null)
				{
					++mapped;
					continue;
				}
				if(possible[i]!.Count == 1)
				{
					++mapped;
					var opcode = possible[i]!.First();
					mapping.Add((byte)i, opcode);
					possible[i] = null;
					foreach(var set in possible)
					{
						set?.Remove(opcode);
					}
				}
			}
			if(mapped == 16) break;
			if(mapped == prev) throw new InvalidDataException();
			prev = mapped;
		}

		return mapping;
	}

	static List<Sample> LoadSamples(TextReader reader)
	{
		var samples = new List<Sample>();
		while(TryParseSample(reader, out var sample))
		{
			samples.Add(sample);
		}
		return samples;
	}

	public override string Process(TextReader reader)
	{
		var samples = LoadSamples(reader);
		var mapping = GetMapping(samples);
		Span<int> registers = stackalloc int[4];
		Execute(registers, mapping, reader);
		return registers[0].ToString();
	}
}
