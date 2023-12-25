namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/14"/></remarks>
[Name(@"Parabolic Reflector Dish")]
public abstract class Day14Solution : Solution
{
	protected static void TiltNorth(char[,] map)
	{
		var height = map.GetLength(0);
		var width  = map.GetLength(1);
		for(int x = 0; x < width; ++x)
		{
			var block = -1;
			for(int y = 0; y < height; ++y)
			{
				switch(map[y, x])
				{
					case '#': block = y; break;
					case 'O':
						map[y, x] = '.';
						map[++block, x] = 'O';
						break;
				}
			}
		}
	}

	protected static void TiltWest(char[,] map)
	{
		var height = map.GetLength(0);
		var width  = map.GetLength(1);
		for(int y = 0; y < height; ++y)
		{
			var block = -1;
			for(int x = 0; x < width; ++x)
			{
				switch(map[y, x])
				{
					case '#': block = x; break;
					case 'O':
						map[y, x] = '.';
						map[y, ++block] = 'O';
						break;
				}
			}
		}
	}

	protected static void TiltSouth(char[,] map)
	{
		var height = map.GetLength(0);
		var width  = map.GetLength(1);
		for(int x = 0; x < width; ++x)
		{
			var block = height;
			for(int y = height - 1; y >= 0; --y)
			{
				switch(map[y, x])
				{
					case '#': block = y; break;
					case 'O':
						map[y, x] = '.';
						map[--block, x] = 'O';
						break;
				}
			}
		}
	}

	protected static void TiltEast(char[,] map)
	{
		var height = map.GetLength(0);
		var width  = map.GetLength(1);
		for(int y = 0; y < height; ++y)
		{
			var block = width;
			for(int x = width - 1; x >= 0; --x)
			{
				switch(map[y, x])
				{
					case '#': block = x; break;
					case 'O':
						map[y, x] = '.';
						map[y, --block] = 'O';
						break;
				}
			}
		}
	}

	protected static long GetLoad(char[,] map)
	{
		var height = map.GetLength(0);
		var width  = map.GetLength(1);
		var score = 0L;
		for(int y = 0; y < height; ++y)
		{
			for(int x = 0; x < width; ++x)
			{
				if(map[y, x] != 'O') continue;
				score += height - y;
			}
		}
		return score;
	}

	protected static char[,] LoadMap(TextReader reader)
	{
		var lines = new List<string>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			lines.Add(line);
		}
		var map = new char[lines.Count, lines[0].Length];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				map[y, x] = lines[y][x];
			}
		}
		return map;
	}

	protected static void PrintMap(char[,] map)
	{
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				Console.Write(map[y, x]);
			}
			Console.WriteLine();
		}
	}
}

public sealed class Day14SolutionPart1 : Day14Solution
{
	public override string Process(TextReader reader)
	{
		var map = LoadMap(reader);
		TiltNorth(map);
		return GetLoad(map).ToString();
	}
}

public sealed class Day14SolutionPart2 : Day14Solution
{
	static bool HasCycle(List<long> sequence, out int len)
	{
		if(sequence.Count < 100) goto no_sequence;

		var last = sequence.Count - 2;
		var i = sequence.LastIndexOf(sequence[^1], last);
		if(i < sequence.Count / 2) goto no_sequence;

		for(int x = sequence.Count - 1 - i; x < sequence.Count / 2; ++x)
		{
			var found = true;
			var i1min = Math.Max(0, sequence.Count - x - 1);
			var i2min = Math.Max(0, sequence.Count - 2 * x - 1);
			for(int i1 = sequence.Count - 1, i2 = i1min; i1 > i1min && i2 > i2min; --i1, --i2)
			{
				if(sequence[i1] != sequence[i2])
				{
					found = false;
					break;
				}
			}
			if(found)
			{
				len = x;
				return true;
			}
		}
		no_sequence:
		len = 0;
		return false;
	}

	static void RunCycle(char[,] map)
	{
		TiltNorth(map);
		TiltWest (map);
		TiltSouth(map);
		TiltEast (map);
	}

	public override string Process(TextReader reader)
	{
		var sequence = new List<long>();
		var map = LoadMap(reader);

		const long cycles = 1_000_000_000;

		for(long i = 0; i < cycles; ++i)
		{
			RunCycle(map);
			var load = GetLoad(map);
			sequence.Add(load);
			if(HasCycle(sequence, out var len))
			{
				var cycleStart   = sequence.Count - 1 - len;
				var cyclesToSkip = (int)(cycles - 1 - (i - len));
				return sequence[cycleStart + cyclesToSkip % len].ToString();
			}
		}

		return GetLoad(map).ToString();
	}
}
