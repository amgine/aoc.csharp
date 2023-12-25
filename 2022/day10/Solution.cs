using System;
using System.Text;

namespace AoC.Year2022;

[Name(@"Cathode-Ray Tube")]
public abstract class Day10Solution : Solution
{
	protected interface ICommand
	{
		void Execute(ref CpuState cpu);
	}

	sealed class NoOpCommand : ICommand
	{
		public static NoOpCommand Instance = new();

		public void Execute(ref CpuState cpu) => cpu.NoOp();
	}

	sealed class AddXCommand(int value) : ICommand
	{
		public void Execute(ref CpuState cpu) => cpu.AddX(value);
	}

	protected struct CpuState
	{
		public int Cycle;
		public int X;

		private int Counter;
		private int Delta;

		public CpuState()
		{
			X     = 1;
			Cycle = 1;
		}

		public void NoOp()
		{
			Delta   = 0;
			Counter = 1;
		}

		public void AddX(int value)
		{
			Delta   = value;
			Counter = 2;
		}

		public bool RunCycle()
		{
			if(Counter <= 0) return false;
			++Cycle;
			--Counter;
			if(Counter == 0)
			{
				X += Delta;
				return false;
			}
			return true;
		}
	}

	protected static ICommand ParseCommand(string line)
	{
		if(line is @"noop") return NoOpCommand.Instance;
		if(line.StartsWith(@"addx "))
		{
			var value = int.Parse(line.AsSpan(5));
			return new AddXCommand(value);
		}
		throw new ArgumentException($"Unknown command: {line}", nameof(line));
	}
}

public class Day10SolutionPart1 : Day10Solution
{
	public override string Process(TextReader reader)
	{
		var cpu = new CpuState();
		var sum = 0L;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line)) continue;
			ParseCommand(line).Execute(ref cpu);
			do
			{
				if(cpu.Cycle is 20 or 60 or 100 or 140 or 180 or 220)
				{
					sum += cpu.X * cpu.Cycle;
				}
			}
			while(cpu.RunCycle());
		}
		return sum.ToString();
	}
}

public class Day10SolutionPart2 : Day10Solution
{
	readonly struct Screen
	{
		const int PixelsPerLine = 40;

		const char Filled = '#';
		const char Empty  = '.';

		private readonly StringBuilder _sb;

		public Screen() => _sb = new(capacity: PixelsPerLine * 6);

		public readonly void Cycle(in CpuState cpu)
		{
			if(cpu.Cycle > 1 && ((cpu.Cycle - 1) % PixelsPerLine) == 0)
			{
				_sb.AppendLine();
			}
			var pos = (cpu.Cycle - 1) % PixelsPerLine;
			var x = cpu.X - 1;
			var c = (x <= pos && pos < x + 3) ? Filled : Empty;
			_sb.Append(c);
		}

		public override string ToString() => _sb.ToString();
	}

	public override string Process(TextReader reader)
	{
		var cpu    = new CpuState();
		var screen = new Screen();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(string.IsNullOrEmpty(line)) continue;
			ParseCommand(line).Execute(ref cpu);
			do
			{
				screen.Cycle(cpu);
			}
			while(cpu.RunCycle());
		}
		return screen.ToString();
	}
}
