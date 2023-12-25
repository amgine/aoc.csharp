namespace AoC.Year2023;

[Name(@"Lens Library")]
public abstract class Day15Solution : Solution
{
	public static byte GetHash(ReadOnlySpan<char> span)
	{
		var hash = 0;
		foreach(var ch in span)
		{
			hash +=  ch;
			hash *=  17;
			hash %= 256;
		}
		return (byte)hash;
	}
}

public sealed class Day15SolutionPart1 : Day15Solution
{
	public override string Process(TextReader reader)
		=> (reader.ReadLine() ?? throw new InvalidDataException())
			.Split(',')
			.Sum(static s => GetHash(s))
			.ToString();
}

public sealed class Day15SolutionPart2 : Day15Solution
{
	readonly record struct Lens(string Label, int Strength);

	static int GetPower(List<Lens>[] boxes)
		=> boxes.Select(
			(box, i) => box.Select(
				(lens, j) => (i + 1) * (j + 1) * lens.Strength).Sum()).Sum();

	static void Execute(List<Lens>[] boxes, ReadOnlySpan<char> instruction)
	{
		var i     = instruction.IndexOfAny(['-', '=']);
		var label = new string(instruction[..i]);
		var box   = boxes[GetHash(label)];
		switch(instruction[i])
		{
			case '-':
				box.RemoveAll(s => s.Label == label);
				break;
			case '=':
				var str = int.Parse(instruction[(i + 1)..]);
				var l = new Lens(label, str);
				var existing = box.FindIndex(s => s.Label == label);
				if(existing >= 0)
				{
					box[existing] = l;
				}
				else
				{
					box.Add(l);
				}
				break;
			default: throw new InvalidDataException();
		}
	}

	public override string Process(TextReader reader)
	{
		var boxes = new List<Lens>[256];
		for(int i = 0; i < boxes.Length; ++i) boxes[i] = [];
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		foreach(var instruction in line.Split(','))
		{
			Execute(boxes, instruction);
		}
		return GetPower(boxes).ToString();
	}
}
