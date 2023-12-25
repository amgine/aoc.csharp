using System.Diagnostics;
using System.Numerics;

namespace AoC;

public abstract class Solution
{
	protected static char[,] LoadCharMap2D(TextReader reader)
	{
		var lines = LoadInputAsListOfNonEmptyStrings(reader);
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

	protected static int[,] LoadDigitMap2D(TextReader reader)
	{
		var lines = LoadInputAsListOfNonEmptyStrings(reader);
		var map = new int[lines.Count, lines[0].Length];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				map[y, x] = lines[y][x] - '0';
			}
		}
		return map;
	}

	protected static T[,] LoadMap2D<T>(TextReader reader, Func<char, T> converter)
	{
		var lines = LoadInputAsListOfNonEmptyStrings(reader);
		var map = new T[lines.Count, lines[0].Length];
		for(int y = 0; y < lines.Count; ++y)
		{
			for(int x = 0; x < lines[y].Length; ++x)
			{
				map[y, x] = converter(lines[y][x]);
			}
		}
		return map;
	}

	protected static List<string> LoadInputAsListOfNonEmptyStrings(TextReader reader)
	{
		var list = new List<string>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			list.Add(line);
		}
		return list;
	}

	protected static List<T> LoadListFromNonEmptyStrings<T>(TextReader reader, Func<string, T> parseLine)
	{
		var list = new List<T>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			list.Add(parseLine(line));
		}
		return list;
	}

	protected static T SumFromNonEmptyLines<T>(TextReader reader, Func<string, T> parseLine)
		where T : INumberBase<T>
	{
		var sum = T.Zero;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			sum += parseLine(line);
		}
		return sum;
	}

	protected static int CountFromNonEmptyLines(TextReader reader, Predicate<string> test)
	{
		int count = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			if(test(line)) ++count;
		}
		return count;
	}

	static bool? TryCheckResult(int part, string result, string correctFileName)
	{
		try
		{
			using var reader = new StreamReader(correctFileName);
			string? line = default;
			for(int i = 0; i < part; ++i)
			{
				line = reader.ReadLine();
				if(line is null) break;
			}
			if(string.IsNullOrWhiteSpace(line)) return null;
			return result == line;
		}
		catch
		{
			return null;
		}
	}

	public void Run(
		int    partNumber,
		string inputFileName   = @"input.txt",
		string correctFileName = @"correct.txt")
	{
		FileStream fs;
		try
		{
			fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		catch(FileNotFoundException)
		{
			Console.Write($"Part {partNumber}: ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("no input file");
			Console.ResetColor();
			return;
		}
		var result = default(string);
		var t = default(TimeSpan);
		using(fs)
		{
			using var reader = new StreamReader(fs, leaveOpen: true);
			try
			{
				var s = Stopwatch.GetTimestamp();
				result = Process(reader);
				t = Stopwatch.GetElapsedTime(s);
			}
			catch(NotImplementedException)
			{
			}
		}
		Console.Write($"Part {partNumber}: ");
		if(result is null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("not implemented");
			Console.ResetColor();
			return;
		}

		Console.ForegroundColor = TryCheckResult(partNumber, result, correctFileName) switch
		{
			true  => ConsoleColor.Green,
			false => ConsoleColor.Red,
			_     => ConsoleColor.White,
		};
		Console.Write(result);
		Console.ResetColor();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(" (time: ");
		Console.ForegroundColor = ConsoleColor.Gray;
		if(t.Ticks >= TimeSpan.TicksPerMillisecond)
		{
			Console.Write($"{t.Ticks / TimeSpan.TicksPerMillisecond}ms");
		}
		else
		{
			Console.Write($"{t.Ticks / TimeSpan.TicksPerMicrosecond}us");
		}
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(")");
		Console.ResetColor();
		Console.WriteLine();
	}

	public static void Run<T>(
		int    partNumber,
		string inputFileName   = @"input.txt",
		string correctFileName = @"correct.txt")
		where T : Solution, new()
	{
		var solution = new T();
		solution.Run(partNumber, inputFileName, correctFileName);
	}

	public static void Run<T1, T2>(
		string inputFileName   = @"input.txt",
		string correctFileName = @"correct.txt")
		where T1 : Solution, new()
		where T2 : Solution, new()
	{
		Run<T1>(1, inputFileName, correctFileName);
		Run<T2>(2, inputFileName, correctFileName);
	}

    public abstract string Process(TextReader reader);
}
