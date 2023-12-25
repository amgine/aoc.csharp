using System;
using System.Diagnostics;
using System.Globalization;

namespace AoC.Year2023;

[Name(@"Lavaduct Lagoon")]
public abstract class Day18Solution : Solution
{
	protected readonly record struct Instruction(Direction2D Direction, int Count);

	protected static Point2D ApplyInstruction(Point2D pos, Instruction instruction)
		=> pos + Vector2D.FromDirection(instruction.Direction) * instruction.Count;

	static class RectangleEdges
	{
		public const int TopLeft     = 1 << 0;
		public const int Top         = 1 << 1;
		public const int TopRight    = 1 << 2;
		public const int Right       = 1 << 3;
		public const int BottomRight = 1 << 4;
		public const int Bottom      = 1 << 5;
		public const int BottomLeft  = 1 << 6;
		public const int Left        = 1 << 7;
	}

	struct Rectangle
	{
		public readonly bool HasBottomBorder => (Borders & RectangleEdges.Bottom) != 0;
		public readonly bool HasTopBorder    => (Borders & RectangleEdges.Top)    != 0;
		public readonly bool HasLeftBorder   => (Borders & RectangleEdges.Left)   != 0;
		public readonly bool HasRightBorder  => (Borders & RectangleEdges.Right)  != 0;

		public byte Borders;
		public byte AreaFlags;

		public void MarkBorder(int flag) => Borders = (byte)(Borders | flag);

		public void MarkArea(int flag) => AreaFlags = (byte)(AreaFlags | flag);
	}

	static long GetArea(Rectangle[,] rectangles, int[] xValues, int[] yValues, int x, int y)
	{
		static void Mark(Rectangle[,] rectangles, int i, int j, int part)
		{
			if(i >= 0 && j >= 0 && i < rectangles.GetLength(0) && j < rectangles.GetLength(1))
			{
				rectangles[i, j].MarkArea(part);
			}
		}

		var area = 0L;

		// a          b
		// +----------+ c
		// |          |
		// |          |
		// +----------+ d

		var a = xValues[x + 0];
		var b = xValues[x + 1];
		var c = yValues[y + 0];
		var d = yValues[y + 1];

		area += Math.Max(0, (long)(b - a - 1)) * Math.Max(0, (long)(d - c - 1));

		var rc = rectangles[y, x];
		if((rc.AreaFlags & RectangleEdges.TopLeft) == 0)
		{
			++area;
			//Mark(rectangles, y, x - 1, RectangleEdges.TopRight);
			//Mark(rectangles, y - 1, x, RectangleEdges.BottomLeft);
			//Mark(rectangles, y - 1, x - 1, RectangleEdges.BottomRight);
		}
		if((rc.AreaFlags & RectangleEdges.TopRight) == 0)
		{
			++area;
			Mark(rectangles, y, x + 1, RectangleEdges.TopLeft);
			//Mark(rectangles, y - 1, x, RectangleEdges.BottomRight);
			//Mark(rectangles, y - 1, x + 1, RectangleEdges.BottomLeft);
		}
		if((rc.AreaFlags & RectangleEdges.BottomRight) == 0)
		{
			++area;
			Mark(rectangles, y, x + 1, RectangleEdges.BottomLeft);
			Mark(rectangles, y + 1, x, RectangleEdges.TopRight);
			Mark(rectangles, y + 1, x + 1, RectangleEdges.TopLeft);
		}
		if((rc.AreaFlags & RectangleEdges.BottomLeft) == 0)
		{
			++area;
			//Mark(rectangles, y, x - 1, RectangleEdges.BottomRight);
			Mark(rectangles, y + 1, x, RectangleEdges.TopLeft);
			Mark(rectangles, y + 1, x - 1, RectangleEdges.TopRight);
		}
		if(b > a + 1)
		{
			if((rc.AreaFlags & RectangleEdges.Top) == 0)
			{
				area += b - a - 1;
				//Mark(rectangles, y - 1, x, RectangleEdges.Bottom);
			}
			if((rc.AreaFlags & RectangleEdges.Bottom) == 0)
			{
				area += b - a - 1;
				Mark(rectangles, y + 1, x, RectangleEdges.Top);
			}
		}
		if(d > c + 1)
		{
			if((rc.AreaFlags & RectangleEdges.Left) == 0)
			{
				area += d - c - 1;
				//Mark(rectangles, y, x - 1, RectangleEdges.Right);
			}
			if((rc.AreaFlags & RectangleEdges.Right) == 0)
			{
				area += d - c - 1;
				Mark(rectangles, y, x + 1, RectangleEdges.Left);
			}
		}

		return area;
	}

	static long GetArea(Rectangle[,] rectangles, int[] xValues, int[] yValues)
	{
		var area = 0L;
		for(int y = 0; y < rectangles.GetLength(0); y++)
		{
			var isInside = false;
			for(int x = 0; x < rectangles.GetLength(1); x++)
			{
				var rc = rectangles[y, x];
				if(rc.HasLeftBorder) isInside = true;
				if(isInside)
				{
					area += GetArea(rectangles, xValues, yValues, x, y);
				}
				if(rc.HasRightBorder) isInside = false;
			}
		}
		return area;
	}

	static void MarkBorder(Rectangle[,] rectangles, int[] xValues, int[] yValues, List<Instruction> instructions)
	{
		var pos = new Point2D(0, 0);
		foreach(var instruction in instructions)
		{
			var x = Array.BinarySearch(xValues, pos.X);
			var y = Array.BinarySearch(yValues, pos.Y);
			pos = ApplyInstruction(pos, instruction);
			switch(instruction.Direction)
			{
				case Direction2D.Down:
					{
						var y2 = Array.BinarySearch(yValues, y + 1, yValues.Length - y - 1, pos.Y);
						for(int i = y; i < y2; i++)
						{
							rectangles[i, x - 1].MarkBorder(RectangleEdges.Right);
						}
					}
					break;
				case Direction2D.Up:
					{
						var y2 = Array.BinarySearch(yValues, 0, y, pos.Y);
						for(int i = y2; i < y; i++)
						{
							rectangles[i, x].MarkBorder(RectangleEdges.Left);
						}
					}
					break;
				case Direction2D.Right:
					{
						var x2 = Array.BinarySearch(xValues, x + 1, xValues.Length - x - 1, pos.X);
						for(int i = x; i < x2; i++)
						{
							rectangles[y, i].MarkBorder(RectangleEdges.Top);
						}
					}
					break;
				case Direction2D.Left:
					{
						var x2 = Array.BinarySearch(xValues, 0, x, pos.X);
						for(int i = x2; i < x; i++)
						{
							rectangles[y - 1, i].MarkBorder(RectangleEdges.Bottom);
						}
					}
					break;
			}
		}
	}

	[Conditional("DEBUG")]
	static void Print(Rectangle[,] map)
	{
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			var isInside = false;
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x].HasLeftBorder) isInside = true;
				if(map[y, x].HasLeftBorder || map[y, x].HasBottomBorder || map[y, x].HasTopBorder || map[y, x].HasRightBorder)
				{
					if(map[y, x].HasLeftBorder)
					{
						if(map[y, x].HasRightBorder)
						{
							Console.Write('o');
						}
						else
						{
							Console.Write('<');
						}
					}
					else if(map[y, x].HasRightBorder)
					{
						Console.Write('>');
					}
					else
					{
						Console.Write('#');
					}
				}
				else
				{
					Console.Write(isInside ? 'X' : '.');
				}
				if(map[y, x].HasRightBorder) isInside = false;
			}
			Console.WriteLine();
		}
	}

	protected abstract Instruction ParseInstruction(string line);

	private List<Instruction> ParseInstructions(TextReader reader)
	{
		var instructions = new List<Instruction>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var instruction = ParseInstruction(line);
			instructions.Add(instruction);
		}
		return instructions;
	}

	static void GetOffsets(List<Instruction> instructions, out int[] xValues, out int[] yValues)
	{
		var uniqueX = new HashSet<int>(capacity: instructions.Count) { 0 };
		var uniqueY = new HashSet<int>(capacity: instructions.Count) { 0 };
		var pos = new Point2D(0, 0);
		foreach(var i in instructions)
		{
			pos = ApplyInstruction(pos, i);
			uniqueX.Add(pos.X);
			uniqueY.Add(pos.Y);
		}

		xValues = [.. uniqueX];
		yValues = [.. uniqueY];
		Array.Sort(xValues);
		Array.Sort(yValues);
	}

	public override string Process(TextReader reader)
	{
		var instructions = ParseInstructions(reader);

		GetOffsets(instructions, out var xValues, out var yValues);

		var rectangles = new Rectangle[yValues.Length - 1, xValues.Length - 1];
		MarkBorder(rectangles, xValues, yValues, instructions);

		Print(rectangles);

		return GetArea(rectangles, xValues, yValues).ToString();
	}
}

public sealed class Day18SolutionPart1 : Day18Solution
{
	static Direction2D ParseDirection(char d)
		=> d switch
		{
			'R' => Direction2D.Right,
			'D' => Direction2D.Down,
			'L' => Direction2D.Left,
			'U' => Direction2D.Up,
			_ => throw new InvalidDataException(),
		};

	protected override Instruction ParseInstruction(string line)
	{
		Span<Range> ranges = stackalloc Range[3];
		if(line.AsSpan().Split(ranges, ' ') != 3) throw new InvalidDataException();
		var parts     = line.Split(' ');
		var direction = ParseDirection(line.AsSpan(ranges[0])[0]);
		var count     = int.Parse(line.AsSpan(ranges[1]));
		return new(direction, count);
	}
}

public sealed class Day18SolutionPart2 : Day18Solution
{
	static Direction2D ParseDirection(char d)
		=> d switch
		{
			'0' => Direction2D.Right,
			'1' => Direction2D.Down,
			'2' => Direction2D.Left,
			'3' => Direction2D.Up,
			_ => throw new InvalidDataException(),
		};

	protected override Instruction ParseInstruction(string line)
	{
		Span<Range> ranges = stackalloc Range[3];
		if(line.AsSpan().Split(ranges, ' ') != 3) throw new InvalidDataException();
		var hexPart   = line.AsSpan(ranges[2]);
		var direction = ParseDirection(hexPart[^2]);
		var count     = int.Parse(hexPart[2..^2], NumberStyles.HexNumber);
		return new(direction, count);
	}
}
