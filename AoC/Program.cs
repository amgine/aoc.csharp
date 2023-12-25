using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC;

internal partial class Program
{
	[GeneratedRegex(@"^(?<year>\d{4})day(?<day>\d{2})$")]
	private static partial Regex CreateYearDirectoryPattern();

	static readonly Regex YearDirectoryPattern = CreateYearDirectoryPattern();

	static bool IsSolution(Type type)
	{
		if(type.IsAbstract) return false;
		if(!type.IsAssignableTo(typeof(Solution))) return false;
		if(!type.Name.EndsWith("Part1") && !type.Name.EndsWith("Part2")) return false;
		return true;
	}

	class Year(int value, Day[] days)
	{
		public int Value { get; } = value;

		public Day[] Days { get; } = days;
	}

	class Day(int year, int value, string? name, SolutionDescriptor[] solutions)
	{
		public int Year { get; } = year;

		public int Value { get; } = value;

		public string? Name { get; } = name;

		public SolutionDescriptor[] Solutions { get; } = solutions;
	}

	class SolutionDescriptor(int year, int day, int part, string directory, Type type)
	{
		public static int ExtractPart(Type type)
			=> type.Name[^1] - '0';

		public int Year { get; } = year;

		public int Day { get; } = day;

		public int Part { get; } = part;

		public string Directory { get; } = directory;

		public Type Type { get; } = type;
	}

	static string? TryGetName(SolutionDescriptor[] solutions)
	{
		static string? TryGetName(SolutionDescriptor solution)
			=> solution.Type.GetCustomAttribute<NameAttribute>()?.Name;

		foreach(var s in solutions)
		{
			var name = TryGetName(s);
			if(name is not null) return name;
		}
		return default;
	}

	static StreamReader OpenInputFile(SolutionDescriptor solution, string fileName = "input.txt")
	{
		var inputFileName = Path.Combine(solution.Directory, "input.txt");
		var data = File.ReadAllBytes(inputFileName);
		return new StreamReader(new MemoryStream(data, writable: false));
	}

	static void Main(string[] args)
	{
		var days = new Dictionary<int, List<Day>>();

		var cfg           = new string(Path.GetFileName(AppContext.BaseDirectory.AsSpan(0, AppContext.BaseDirectory.Length - 1)));
		var cfgPath       = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, ".."));
		var artifactsPath = Path.GetFullPath(Path.Combine(cfgPath, ".."));
		foreach(var dir in Directory.EnumerateDirectories(artifactsPath))
		{
			var name = Path.GetFileName(dir);
			var match = YearDirectoryPattern.Match(name);
			if(!match.Success) continue;

			var year = int.Parse(match.Groups["year"].ValueSpan);
			var day  = int.Parse(match.Groups["day"].ValueSpan);

			var dllDirectory = Path.Combine(dir, cfg);
			var dllPath      = Path.Combine(dllDirectory, name + ".dll");
			if(File.Exists(dllPath))
			{
				var asm       = Assembly.LoadFrom(dllPath);
				var solutions = Array.ConvertAll(
					Array.FindAll(asm.GetTypes(), IsSolution),
					type => new SolutionDescriptor(year, day, SolutionDescriptor.ExtractPart(type), dllDirectory, type));

				if(solutions.Length == 0) continue;
				Array.Sort(solutions, static (a, b) => a.Part.CompareTo(b.Part));

				if(!days.TryGetValue(year, out var list))
				{
					days.Add(year, list = []);
				}
				list.Add(new Day(year, day, TryGetName(solutions), solutions));
			}
		}

		var now    = DateTimeOffset.Now;
		var latest = now.Month == 12 ? now.Year : now.Year - 1;
		var first  = 2015;
		var years  = new Year[latest - first + 1];

		for(int i = 0; i < years.Length; ++i)
		{
			var y = first + i;
			var d = Array.Empty<Day>();
			if(days.TryGetValue(y, out var list))
			{
				list.Sort(static (a, b) => a.Value.CompareTo(b.Value));
				d = [.. list];
			}
			years[i] = new Year(y, d);
		}

		foreach(var year in years)
		{
			WriteYear(year);
			var dayCounter = 0;
			for(int j = 0; j < year.Days.Length; ++j)
			{
				++dayCounter;
				var day = year.Days[j];
				while(day.Value > dayCounter)
				{
					WriteMissingDay(year, dayCounter);
					++dayCounter;
				}
				WriteDay(year, day);
				foreach(var solution in day.Solutions)
				{
					RunSolution(year, day, solution);
				}
			}
		}
	}

	static void RunSolution(Year year, Day day, SolutionDescriptor solution)
	{
		WriteSolutionPrefix(year, day, solution);
		try
		{
			var s = (Solution)Activator.CreateInstance(solution.Type)!;
			string? answer;
			TimeSpan elapsed;
			using(var reader = OpenInputFile(solution))
			{
				var start = Stopwatch.GetTimestamp();
				answer = s.Process(reader);
				elapsed = Stopwatch.GetElapsedTime(start);
			}
			WriteAnswer(year, day, solution, answer);
			WriteRunTime(elapsed);
		}
		catch(Exception exc)
		{
			WriteFailed(exc);
		}
		Console.WriteLine();
	}

	static void WriteYear(Year year)
	{
		var now    = DateTimeOffset.Now;
		var latest = now.Month == 12 ? now.Year : now.Year - 1;

		Console.Write($"Year ");
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(year.Value);
		Console.ResetColor();
		if(year.Value == latest)
		{
			if(now.Month == 12 && now.Day <= 25)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(" [active] ");
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("================================");
				Console.ResetColor();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(" [latest] ");
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("================================");
				Console.ResetColor();
			}
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(" =========================================");
			Console.ResetColor();
		}
		Console.WriteLine();
	}

	static void WriteMissingDay(Year year, int day)
	{
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(day == 25 ? "└" : "├");
		Console.ResetColor();
		Console.Write(" ");
		Console.Write("Day ");
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(day);
		Console.ResetColor();
		Console.Write(": ");
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine("missing");
		Console.ResetColor();
	}

	static void WriteDay(Year year, Day day)
	{
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(day == year.Days[^1] ? "└" : "├");
		Console.ResetColor();
		Console.Write(" ");
		Console.Write("Day ");
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(day.Value);
		Console.ResetColor();
		Console.Write(":");
		WriteDayName(day);
		Console.WriteLine();
	}

	static void WriteDayName(Day day)
	{
		if(!string.IsNullOrWhiteSpace(day.Name))
		{
			Console.Write(' ');
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(day.Name);
			Console.ResetColor();
		}
	}

	static void WriteSolutionPrefix(Year year, Day day, SolutionDescriptor solution)
	{
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(day == year.Days[^1] ? ' ' : '│');
		Console.Write(' ');
		Console.Write(solution == day.Solutions[^1] ? '└' : '├');
		Console.ResetColor();
		Console.Write(' ');
		Console.Write($"Part {solution.Part}: ");
	}

	static void WriteRunTime(TimeSpan elapsed)
	{
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("(time: ");
		if(elapsed.Ticks == 0)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write('0');
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("us");
		}
		else
		{
			if(elapsed.Ticks > TimeSpan.TicksPerSecond)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(elapsed.Ticks / TimeSpan.TicksPerSecond);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("s");
				elapsed = new(elapsed.Ticks % TimeSpan.TicksPerSecond);
			}
			if(elapsed.Ticks > TimeSpan.TicksPerMillisecond)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(elapsed.Ticks / TimeSpan.TicksPerMillisecond);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("ms");
				elapsed = new(elapsed.Ticks % TimeSpan.TicksPerMillisecond);
			}
			if(elapsed.Ticks > TimeSpan.TicksPerMicrosecond)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(elapsed.Ticks / TimeSpan.TicksPerMicrosecond);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("us");
			}
		}
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(")");
		Console.ResetColor();
	}

	static void WriteAnswer(Year year, Day day, SolutionDescriptor solution, string answer)
	{
		var correct = TryReadCorrectAnswer(solution.Part, solution.Directory);
		var isMultilineAnswer = answer.AsSpan().IndexOfAny(['\r', '\n']) != -1;
		if(correct is null || isMultilineAnswer)
		{
			Console.ForegroundColor = ConsoleColor.White;
		}
		else if(correct == answer)
		{
			Console.ForegroundColor = ConsoleColor.Green;
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Red;
		}
		if(isMultilineAnswer)
		{
			var color = Console.ForegroundColor;
			using var ar = new StringReader(answer);
			string? al;
			while((al = ar.ReadLine()) is not null)
			{
				Console.ForegroundColor = color;
				Console.WriteLine(al);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write(day == year.Days[^1] ? ' ' : '│');
				Console.Write(' ');
				Console.Write(solution == day.Solutions[^1] ? ' ' : '│');
				Console.Write("         ");
			}
			Console.ResetColor();
		}
		else
		{
			Console.Write(answer);
			Console.ResetColor();
			Console.Write(" ");
		}
	}

	static void WriteFailed(Exception exc)
	{
		switch(exc)
		{
			case FileNotFoundException:
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("no input file");
				Console.ResetColor();
				break;
			case NotImplementedException:
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("not implemented");
				Console.ResetColor();
				break;
			case InvalidDataException:
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("invalid input");
				Console.ResetColor();
				break;
			default:
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"failed ({exc.GetType().Name})");
				Console.ResetColor();
				break;
		}
	}

	static string? TryReadCorrectAnswer(int part, string directory)
	{
		var fn = Path.Combine(directory, "correct.txt");
		if(!File.Exists(fn)) return default;
		try
		{
			using var reader = new StreamReader(fn);
			var line = default(string);
			while(part-- > 0)
			{
				line = reader.ReadLine();
				if(line is null) return default;
			}
			if(string.IsNullOrWhiteSpace(line)) return default;
			return line;
		}
		catch(FileNotFoundException)
		{
			return default;
		}
	}
}
