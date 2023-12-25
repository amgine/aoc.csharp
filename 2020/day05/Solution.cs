namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/5"/></remarks>
[Name(@"Binary Boarding")]
public abstract class Day05Solution : Solution
{
	const int Rows    = 128;
	const int Columns =   8;

	readonly record struct Int32Range(int Min, int Count)
	{
		public Int32Range TakeLeftHalf() => new(Min, Count / 2);

		public Int32Range TakeRightHalf() => new(Min + Count / 2, Count / 2);
	}

	static Int32Range BinaryReduce(Int32Range range, ReadOnlySpan<char> commands, char left, char right)
	{
		foreach(var c in commands)
		{
			if(c == left ) { range = range.TakeLeftHalf();  continue; }
			if(c == right) { range = range.TakeRightHalf(); continue; }
			throw new InvalidDataException();
		}
		return range;
	}

	protected static int GetRow(ReadOnlySpan<char> text)
		=> BinaryReduce(new Int32Range(0, Rows), text, 'F', 'B').Min;

	protected static int GetColumn(ReadOnlySpan<char> text)
		=> BinaryReduce(new Int32Range(0, Columns), text, 'L', 'R').Min;

	protected static int GetSeatID(int row, int column)
		=> row * Columns + column;

	protected static int ParseSeatID(string boardingPass)
		=> GetSeatID(
			row:    GetRow   (boardingPass.AsSpan(0, 7)),
			column: GetColumn(boardingPass.AsSpan(7, 3)));
}

public sealed class Day05SolutionPart1 : Day05Solution
{
	public override string Process(TextReader reader)
	{
		var max = -1;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var id = ParseSeatID(line);
			if(id > max) max = id;
		}
		return max.ToString();
	}
}

public sealed class Day05SolutionPart2 : Day05Solution
{
	public override string Process(TextReader reader)
	{
		static void Increment(Dictionary<int, int> d, int key)
		{
			if(!d.TryGetValue(key, out var value)) value = 0;
			d[key] = value + 1;
		}

		var min = int.MaxValue;
		var max = int.MinValue;
		var d = new Dictionary<int, int>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			var id = ParseSeatID(line);
			if(id > max) max = id;
			if(id < min) min = id;
			Increment(d, id - 1);
			Increment(d, id);
			Increment(d, id + 1);
		}

		d.Remove(min);
		d.Remove(max);

		var values = d
			.Where(static kvp => kvp.Value == 2)
			.Select(static kvp => kvp.Key)
			.ToList();
		values.Sort();
		return values[1].ToString();
	}
}
