
namespace AoC.Year2015;

/// <remarks><a href="https://adventofcode.com/2015/day/6"/></remarks>
[Name(@"Probably a Fire Hazard")]
public abstract class Day06Solution : Solution
{
	protected enum LightAction { TurnOn, TurnOff, Toggle }

	protected readonly record struct Instruction(LightAction Action, Rectangle2D Area);

	static class Parser
	{
		static readonly Dictionary<string, LightAction> _actionPrefixes = new()
		{
			["toggle "]   = LightAction.Toggle,
			["turn on "]  = LightAction.TurnOn,
			["turn off "] = LightAction.TurnOff,
		};

		static LightAction ParseAction(ref ReadOnlySpan<char> line)
		{
			foreach(var kvp in _actionPrefixes)
			{
				if(line.StartsWith(kvp.Key))
				{
					line = line[kvp.Key.Length..];
					return kvp.Value;
				}
			}
			throw new InvalidDataException($"Invalid instruction: {new string(line)}");
		}

		static Rectangle2D ParseArea(ReadOnlySpan<char> line)
		{
			static Point2D ParseCoordinates(ReadOnlySpan<char> text)
			{
				Span<Range> ranges = stackalloc Range[2];

				if(text.Split(ranges, ',', StringSplitOptions.TrimEntries) != 2)
				{
					throw new InvalidDataException();
				}

				return new(
					X: int.Parse(text[ranges[0]]),
					Y: int.Parse(text[ranges[1]]));
			}

			const string Through = @" through ";
			var s = line.IndexOf(Through);
			if(s == -1) throw new InvalidDataException($"Invalid instruction: {new string(line)}");
			return Rectangle2D.FromCorners(
				topLeft:     ParseCoordinates(line[..s]),
				bottomRight: ParseCoordinates(line[(s + Through.Length)..]));
		}

		public static Instruction ParseInstruction(ReadOnlySpan<char> line)
		{
			var action = ParseAction(ref line);
			var area   = ParseArea(line);
			return new(action, area);
		}
	}

	static long Count(int[,] grid)
	{
		long count = 0;
		foreach(var pos in grid) count += pos;
		return count;
	}

	protected abstract int ExecuteAction(LightAction action, int current);

	private void Execute(Instruction instruction, int[,] grid)
	{
		var maxX = instruction.Area.Position.X + instruction.Area.Size.Width;
		var maxY = instruction.Area.Position.Y + instruction.Area.Size.Height;
		for(int y = instruction.Area.Position.Y; y < maxY; ++y)
		{
			for(int x = instruction.Area.Position.X; x < maxX; ++x)
			{
				grid[x, y] = ExecuteAction(instruction.Action, grid[x, y]);
			}
		}
	}

	public override string Process(TextReader reader)
	{
		var grid = new int[1000, 1000];
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			Execute(Parser.ParseInstruction(line), grid);
		}
		return Count(grid).ToString();
	}
}

public sealed class Day06SolutionPart1 : Day06Solution
{
	protected override int ExecuteAction(LightAction action, int current)
		=> action switch
		{
			LightAction.TurnOn  => 1,
			LightAction.TurnOff => 0,
			LightAction.Toggle  => current == 0 ? 1 : 0,
			_ => throw new ArgumentException($"Unknown action: {action}"),
		};
}

public sealed class Day06SolutionPart2 : Day06Solution
{
	protected override int ExecuteAction(LightAction action, int current)
		=> action switch
		{
			LightAction.TurnOn  => current + 1,
			LightAction.TurnOff => Math.Max(0, current - 1),
			LightAction.Toggle  => current + 2,
			_ => throw new ArgumentException($"Unknown action: {action}"),
		};
}
