namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/17"/></remarks>
[Name(@"Chronospatial Computer")]
public abstract class Day17Solution : Solution
{
	protected abstract class CpuBase
	{
		public ulong A;
		public ulong B;
		public ulong C;

		private ulong Combo(byte operand)
			=> operand switch
			{
				>= 0 and <= 3 => operand,
				4 => A,
				5 => B,
				6 => C,
				_ => throw new InvalidDataException(),
			};

		private ulong Adv(byte operand)
			=> A / (ulong)(1 << (int)Combo(operand));

		public void Execute(Input input)
		{
			A = input.RegisterA;
			B = input.RegisterB;
			C = input.RegisterC;
			Execute(input.Program);
		}

		public void Execute(ReadOnlySpan<byte> program)
		{
			for(int i = 0; i < program.Length; i += 2)
			{
				var operand = program[i + 1];
				switch(program[i])
				{
					case 0: A = Adv(operand);                   break;
					case 1: B ^= operand;                       break;
					case 2: B = Combo(operand) % 8;             break;
					case 3 when A != 0: i = operand - 2;        break;
					case 3 when A == 0:                         continue;
					case 4: B ^= C;                             break;
					case 5: Output((byte)(Combo(operand) % 8)); break;
					case 6: B = Adv(operand);                   break;
					case 7: C = Adv(operand);                   break;
					default: throw new InvalidDataException();
				}
			}
		}

		protected virtual void Output(byte value) { }
	}

	protected sealed record class Input(ulong RegisterA, ulong RegisterB, ulong RegisterC, byte[] Program)
	{
		public static Input Parse(TextReader reader)
		{
			var a = reader.ReadLine() ?? throw new InvalidDataException();
			var b = reader.ReadLine() ?? throw new InvalidDataException();
			var c = reader.ReadLine() ?? throw new InvalidDataException();
			reader.ReadLine();
			var p = reader.ReadLine() ?? throw new InvalidDataException();

			static ulong GetReg(string line)
				=> ulong.Parse(line.AsSpan(line.IndexOf(':') + 1));

			var aReg = GetReg(a);
			var bReg = GetReg(b);
			var cReg = GetReg(c);
			var program = Array.ConvertAll(p[(p.IndexOf(':') + 1)..].Split(','), byte.Parse);

			return new(aReg, bReg, cReg, program);
		}
	}
}

public sealed class Day17SolutionPart1 : Day17Solution
{
	sealed class CpuWithOutput : CpuBase
	{
		public List<byte> Out { get; } = [];

		protected override void Output(byte value)
			=> Out.Add(value);
	}

	public override string Process(TextReader reader)
	{
		var input = Input.Parse(reader);
		var cpu   = new CpuWithOutput();
		cpu.Execute(input);
		return string.Join(',', cpu.Out);
	}
}

public sealed class Day17SolutionPart2 : Day17Solution
{
	sealed class CpuWithOutput : CpuBase
	{
		public byte Out { get; private set; }

		protected override void Output(byte value)
			=> Out = value;

		public byte Run(ulong a, ReadOnlySpan<byte> program)
		{
			A = a;
			Execute(program);
			return Out;
		}
	}

	public override string Process(TextReader reader)
	{
		var input = Input.Parse(reader);

		if(input.Program is not [.., 3, 0])
		{
			throw new NotSupportedException();
		}

		var body     = input.Program.AsSpan(0, input.Program.Length - 2);
		var variants = new List<ulong>();
		var temp     = new List<ulong>();
		var cpu      = new CpuWithOutput();

		var expected = input.Program[^1];
		for(ulong j = 0; j < 8; ++j)
		{
			if(cpu.Run(j, body) == expected)
			{
				variants.Add(j);
			}
		}
		for(int i = input.Program.Length - 2; i >= 0; --i)
		{
			foreach(var variant in variants)
			{
				var vv = variant << 3;
				expected = input.Program[i];
				for(int j = 0; j < 8; ++j)
				{
					var part = (ulong)j;
					part |= vv;
					if(cpu.Run(part, body) == expected)
					{
						temp.Add(part);
					}
				}
			}
			(variants, temp) = (temp, variants);
			temp.Clear();
		}

		return variants[0].ToString();
	}
}
