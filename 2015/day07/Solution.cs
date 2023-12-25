using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/7"/></remarks>
[Name(@"Some Assembly Required")]
public abstract partial class Day07Solution : Solution
{
	protected interface ICommand
	{
		bool TryExecute(Dictionary<string, ushort> memory);
	}

	protected interface IOperand
	{
		bool TryGetValue(Dictionary<string, ushort> memory, out ushort value);
	}

	sealed class ConstantOperand(ushort constant) : IOperand
	{
		public bool TryGetValue(Dictionary<string, ushort> memory, out ushort value)
		{
			value = constant;
			return true;
		}

		public override string ToString() => constant.ToString();
	}

	sealed class VariableOperand(string name) : IOperand
	{
		public bool TryGetValue(Dictionary<string, ushort> memory, out ushort value)
			=> memory.TryGetValue(name, out value);

		public override string ToString() => name;
	}

	static IOperand ParseOperand(Match match, string name)
	{
		var group = match.Groups[name];
		if(ushort.TryParse(group.ValueSpan, out var constant))
		{
			return new ConstantOperand(constant);
		}
		return new VariableOperand(group.Value);
	}

	static bool TryWrite(Dictionary<string, ushort> memory, string name, ushort value)
		=> memory.TryAdd(name, value);

	sealed partial class WriteCommand(IOperand value, string target) : ICommand
	{
		[GeneratedRegex(@"^(?<value>(\d+)|([a-z]+))\s*\-\>\s*(?<target>[a-z]+)$")]
		private static partial Regex CreateRegex();

		private static readonly Regex Regex = CreateRegex();

		public static bool TryParse(string line, [MaybeNullWhen(returnValue: false)] out ICommand command)
		{
			if(Regex.Match(line) is { Success: true } match)
			{
				command = new WriteCommand(
					ParseOperand(match, "value"),
					match.Groups["target"].Value);
				return true;
			}
			command = default;
			return false;
		}

		public bool TryExecute(Dictionary<string, ushort> memory)
		{
			if(!value.TryGetValue(memory, out var valueToWrite)) return false;
			TryWrite(memory, target, valueToWrite);
			return true;
		}

		public override string ToString() => $"{value} -> {target}";
	}

	sealed partial class LeftShiftCommand(IOperand a, IOperand b, string target) : ICommand
	{
		[GeneratedRegex(@"^(?<a>(\d+)|([a-z]+))\s+LSHIFT\s+(?<b>(\d+)|([a-z]+))\s*\-\>\s*(?<target>[a-z]+)$")]
		private static partial Regex CreateRegex();

		private static readonly Regex Regex = CreateRegex();

		public static bool TryParse(string line, [MaybeNullWhen(returnValue: false)] out ICommand command)
		{
			if(Regex.Match(line) is { Success: true } match)
			{
				command = new LeftShiftCommand(
					ParseOperand(match, "a"),
					ParseOperand(match, "b"),
					match.Groups["target"].Value);
				return true;
			}
			command = default;
			return false;
		}

		public bool TryExecute(Dictionary<string, ushort> memory)
		{
			if(!a.TryGetValue(memory, out var l)) return false;
			if(!b.TryGetValue(memory, out var r)) return false;
			TryWrite(memory, target, (ushort)(l << r));
			return true;
		}

		public override string ToString() => $"{a} LSHIFT {b} -> {target}";
	}

	sealed partial class RightShiftCommand(IOperand a, IOperand b, string target) : ICommand
	{
		[GeneratedRegex(@"^(?<a>(\d+)|([a-z]+))\s+RSHIFT\s+(?<b>(\d+)|([a-z]+))\s*\-\>\s*(?<target>[a-z]+)$")]
		private static partial Regex CreateRegex();

		private static readonly Regex Regex = CreateRegex();

		public static bool TryParse(string line, [MaybeNullWhen(returnValue: false)] out ICommand command)
		{
			if(Regex.Match(line) is { Success: true } match)
			{
				command = new RightShiftCommand(
					ParseOperand(match, "a"),
					ParseOperand(match, "b"),
					match.Groups["target"].Value);
				return true;
			}
			command = default;
			return false;
		}

		public bool TryExecute(Dictionary<string, ushort> memory)
		{
			if(!a.TryGetValue(memory, out var l)) return false;
			if(!b.TryGetValue(memory, out var r)) return false;
			TryWrite(memory, target, (ushort)(l >> r));
			return true;
		}

		public override string ToString() => $"{a} RSHIFT {b} -> {target}";
	}

	sealed partial class OrCommand(IOperand a, IOperand b, string target) : ICommand
	{
		[GeneratedRegex(@"^(?<a>(\d+)|([a-z]+))\s+OR\s+(?<b>(\d+)|([a-z]+))\s*\-\>\s*(?<target>[a-z]+)$")]
		private static partial Regex CreateRegex();

		private static readonly Regex Regex = CreateRegex();

		public static bool TryParse(string line, [MaybeNullWhen(returnValue: false)] out ICommand command)
		{
			if(Regex.Match(line) is { Success: true } match)
			{
				command = new OrCommand(
					ParseOperand(match, "a"),
					ParseOperand(match, "b"),
					match.Groups["target"].Value);
				return true;
			}
			command = default;
			return false;
		}

		public bool TryExecute(Dictionary<string, ushort> memory)
		{
			if(!a.TryGetValue(memory, out var l)) return false;
			if(!b.TryGetValue(memory, out var r)) return false;
			TryWrite(memory, target, (ushort)(l | r));
			return true;
		}

		public override string ToString() => $"{a} OR {b} -> {target}";
	}

	sealed partial class AndCommand(IOperand a, IOperand b, string target) : ICommand
	{
		[GeneratedRegex(@"^(?<a>(\d+)|([a-z]+))\s+AND\s+(?<b>(\d+)|([a-z]+))\s*\-\>\s*(?<target>[a-z]+)$")]
		private static partial Regex CreateRegex();

		private static readonly Regex Regex = CreateRegex();

		public static bool TryParse(string line, [MaybeNullWhen(returnValue: false)] out ICommand command)
		{
			if(Regex.Match(line) is { Success: true } match)
			{
				command = new AndCommand(
					ParseOperand(match, "a"),
					ParseOperand(match, "b"),
					match.Groups["target"].Value);
				return true;
			}
			command = default;
			return false;
		}

		public bool TryExecute(Dictionary<string, ushort> memory)
		{
			if(!a.TryGetValue(memory, out var l)) return false;
			if(!b.TryGetValue(memory, out var r)) return false;
			TryWrite(memory, target, (ushort)(l & r));
			return true;
		}

		public override string ToString() => $"{a} AND {b} -> {target}";
	}

	sealed partial class NotCommand(IOperand a, string target) : ICommand
	{
		[GeneratedRegex(@"^NOT\s+(?<a>(\d+)|([a-z]+))\s*\-\>\s*(?<target>[a-z]+)$")]
		private static partial Regex CreateRegex();

		private static readonly Regex Regex = CreateRegex();

		public static bool TryParse(string line, [MaybeNullWhen(returnValue: false)] out ICommand command)
		{
			if(Regex.Match(line) is { Success: true } match)
			{
				command = new NotCommand(
					ParseOperand(match, "a"),
					match.Groups["target"].Value);
				return true;
			}
			command = default;
			return false;
		}

		public bool TryExecute(Dictionary<string, ushort> memory)
		{
			if(!a.TryGetValue(memory, out var l)) return false;
			TryWrite(memory, target, (ushort)(~l));
			return true;
		}

		public override string ToString() => $"NOT {a} -> {target}";
	}

	private static ICommand ParseCommand(string line)
	{
		ICommand? command;

		if(WriteCommand.TryParse(line, out command)) return command;
		if(AndCommand.TryParse(line, out command)) return command;
		if(OrCommand.TryParse(line, out command)) return command;
		if(LeftShiftCommand.TryParse(line, out command)) return command;
		if(RightShiftCommand.TryParse(line, out command)) return command;
		if(NotCommand.TryParse(line, out command)) return command;

		throw new InvalidDataException($"Invalid command: {line}");
	}

	protected static List<ICommand> LoadCommands(TextReader reader)
	{
		var commands = new List<ICommand>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			commands.Add(ParseCommand(line));
		}
		return commands;
	}

	protected static void Execute(List<ICommand> commands, Dictionary<string, ushort> memory)
	{
		while(commands.Count > 0)
		{
			if(commands.RemoveAll(c => c.TryExecute(memory)) == 0)
			{
				throw new InvalidDataException();
			}
		}
	}
}

public sealed class Day07SolutionPart1 : Day07Solution
{
	public override string Process(TextReader reader)
	{
		var memory = new Dictionary<string, ushort>();
		Execute(LoadCommands(reader), memory);
		return memory["a"].ToString();
	}
}

public sealed class Day07SolutionPart2 : Day07Solution
{
	public override string Process(TextReader reader)
	{
		var memory = new Dictionary<string, ushort>();
		var commands = LoadCommands(reader);
		Execute(new(commands), memory);
		var a = memory["a"];
		memory.Clear();
		memory["b"] = a;
		Execute(commands, memory);
		return memory["a"].ToString();
	}
}
