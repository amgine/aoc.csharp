using System;
using System.Diagnostics;
using System.Globalization;

namespace AoC.Year2022;

[Name(@"Rope Bridge")]
public abstract class Day9Solution : Solution
{
	protected readonly record struct Command(char Direction, int Amount);

	protected readonly record struct Coordinates(int X, int Y);

	protected static bool IsAdjacent(Coordinates a, Coordinates b)
		=> Math.Abs(a.X - b.X) <= 1 && Math.Abs(a.Y - b.Y) <= 1;

	protected static Coordinates MoveCloser(Coordinates src, Coordinates dst)
	{
		var dx = Math.Sign(dst.X - src.X);
		var dy = Math.Sign(dst.Y - src.Y);
		return new(src.X + dx, src.Y + dy);
	}

	protected static bool ParseNextCommand(TextReader reader, out Command command)
	{
		var line = reader.ReadLine();
		if(line is not { Length: > 0 })
		{
			command = default;
			return false;
		}
		command = new(line[0], int.Parse(line.AsSpan(2)));
		return true;
	}
}

public class Day9SolutionPart1 : Day9Solution
{
	private readonly HashSet<Coordinates> _visited = new();
	private Coordinates _head;
	private Coordinates _tail;

	private void Mark(Coordinates c) => _visited.Add(c);

	private void MoveTail()
	{
		if(IsAdjacent(_head, _tail)) return;
		_tail = MoveCloser(_tail, _head);
		Mark(_tail);
	}

	private void MoveRight()
	{
		_head = _head with { X = _head.X + 1 };
		MoveTail();
	}

	private void MoveUp()
	{
		_head = _head with { Y = _head.Y - 1 };
		MoveTail();
	}

	private void MoveLeft()
	{
		_head = _head with { X = _head.X - 1 };
		MoveTail();
	}

	private void MoveDown()
	{
		_head = _head with { Y = _head.Y + 1 };
		MoveTail();
	}

	private void Execute(Command command)
	{
		switch(command.Direction)
		{
			case 'R': for(int i = 0; i < command.Amount; ++i) MoveRight(); break;
			case 'U': for(int i = 0; i < command.Amount; ++i) MoveUp();    break;
			case 'L': for(int i = 0; i < command.Amount; ++i) MoveLeft();  break;
			case 'D': for(int i = 0; i < command.Amount; ++i) MoveDown();  break;
			default: throw new Exception();
		}
	}

	public override string Process(TextReader reader)
	{
		Mark(_tail);
		while(ParseNextCommand(reader, out Command command))
		{
			Execute(command);
		}
		return _visited.Count.ToString(CultureInfo.InvariantCulture);
	}
}

public class Day9SolutionPart2 : Day9Solution
{
	private readonly HashSet<Coordinates> _visited = new();
	private Coordinates[] _rope = new Coordinates[10];

	private void Mark(Coordinates c) => _visited.Add(c);

	private void MoveTail()
	{
		for(int i = _rope.Length - 2; i >= 0; --i)
		{
			if(IsAdjacent(_rope[i], _rope[i + 1])) return;
			_rope[i] = MoveCloser(_rope[i], _rope[i + 1]);
		}
		Mark(_rope[0]);
	}

	private void MoveRight()
	{
		var old = _rope[^1];
		_rope[^1] = old with { X = old.X + 1 };
		MoveTail();
	}

	private void MoveUp()
	{
		var old = _rope[^1];
		_rope[^1] = old with { Y = old.Y - 1 };
		MoveTail();
	}

	private void MoveLeft()
	{
		var old = _rope[^1];
		_rope[^1] = old with { X = old.X - 1 };
		MoveTail();
	}

	private void MoveDown()
	{
		var old = _rope[^1];
		_rope[^1] = old with { Y = old.Y + 1 };
		MoveTail();
	}

	private void Execute(Command command)
	{
		switch(command.Direction)
		{
			case 'R': for(int i = 0; i < command.Amount; ++i) MoveRight(); break;
			case 'U': for(int i = 0; i < command.Amount; ++i) MoveUp();    break;
			case 'L': for(int i = 0; i < command.Amount; ++i) MoveLeft();  break;
			case 'D': for(int i = 0; i < command.Amount; ++i) MoveDown();  break;
			default: throw new Exception();
		}
	}

	[Conditional("DEBUG")]
	private void Debug(Command command)
	{
		Console.WriteLine($"{command.Direction} {command.Amount}");
		var minX = int.MaxValue;
		var maxX = int.MinValue;
		var minY = int.MaxValue;
		var maxY = int.MinValue;
		foreach(var c in _rope)
		{
			if(c.X < minX) minX = c.X;
			if(c.X > maxX) maxX = c.X;
			if(c.Y < minY) minY = c.Y;
			if(c.Y > maxY) maxY = c.Y;
		}

		for(int y = minY; y <= maxY; ++y)
		{
			for(int x = minX; x <= maxX; ++x)
			{
				var c = new Coordinates(x, y);
				var found = false;
				for(int i = _rope.Length - 1; i >= 0; --i)
				{
					if(_rope[i] == c)
					{
						if(i == _rope.Length - 1)
						{
							Console.Write('H');
						}
						else
						{
							Console.Write((char)((9 - i) + '0'));
						}
						found = true;
						break;
					}
				}
				if(!found)
				{
					Console.Write('.');
				}
			}
			Console.WriteLine();
		}
	}

	public override string Process(TextReader reader)
	{
		Mark(_rope[0]);
		while(ParseNextCommand(reader, out Command command))
		{
			Execute(command);
			Debug(command);
		}
		return _visited.Count.ToString(CultureInfo.InvariantCulture);
	}
}
