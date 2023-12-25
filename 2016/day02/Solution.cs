using System.Text;

namespace AoC.Year2016;

/// <remarks><a href="https://adventofcode.com/2016/day/2"/></remarks>
[Name(@"Bathroom Security")]
public abstract class Day02Solution(char[,] keyPad) : Solution
{
	protected static Vector2D GetOffset(char instruction)
		=> instruction switch
		{
			'U' => Vector2D.Up,
			'R' => Vector2D.Right,
			'D' => Vector2D.Down,
			'L' => Vector2D.Left,
			_ => throw new InvalidDataException($"Unexpected instruction: {instruction}"),
		};

	private char GetCode(string instructions)
	{
		var pos = new Point2D(1, 1);
		foreach(var c in instructions)
		{
			var next = pos + GetOffset(c);
			if(IsValidPosition(next))
			{
				pos = next;
			}
		}
		return pos.GetValue(keyPad);
	}

	protected virtual bool IsValidPosition(Point2D position)
		=> position.IsInside(keyPad);

	public override string Process(TextReader reader)
	{
		var code = new StringBuilder();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			code.Append(GetCode(line));
		}
		return code.ToString();
	}
}

public sealed class Day02SolutionPart1() : Day02Solution(KeyPad)
{
	static readonly char[,] KeyPad = new[,]
	{
		{ '1', '2', '3' },
		{ '4', '5', '6' },
		{ '7', '8', '9' },
	};
}

public sealed class Day02SolutionPart2() : Day02Solution(KeyPad)
{
	static readonly char[,] KeyPad = new[,]
	{
		{ ' ', ' ', '1', ' ', ' ' },
		{ ' ', '2', '3', '4', ' ' },
		{ '5', '6', '7', '8', '9' },
		{ ' ', 'A', 'B', 'C', ' ' },
		{ ' ', ' ', 'D', ' ', ' ' },
	};

	protected override bool IsValidPosition(Point2D position)
		=> position.IsInside(KeyPad) && position.GetValue(KeyPad) != ' ';
}
