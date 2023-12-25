namespace AoC.Year2022;

[Name(@"Boiling Boulders")]
public abstract class Day18Solution : Solution
{
	protected readonly record struct Vertex3D(int X, int Y, int Z)
	{
		public static Vector3D operator -(Vertex3D a, Vertex3D b)
			=> new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	protected readonly record struct Vector3D(int X, int Y, int Z);

	protected static Vertex3D ParseVertex3D(string line)
	{
		Span<Range> ranges = stackalloc Range[3];
		if(line.AsSpan().Split(ranges, ',') != 3) throw new InvalidDataException();
		return new(
			X: int.Parse(line.AsSpan(ranges[0])),
			Y: int.Parse(line.AsSpan(ranges[1])),
			Z: int.Parse(line.AsSpan(ranges[2])));
	}

	protected static List<Vertex3D> ParseInput(TextReader reader)
		=> LoadListFromNonEmptyStrings(reader, ParseVertex3D);
}

public class Day18SolutionPart1 : Day18Solution
{
	static bool AreAdjacent(Vertex3D a, Vertex3D b)
	{
		var vec = a - b;

		if(vec.X is 1 or -1 && vec.Y == 0 && vec.Z == 0)
		{
			return true;
		}
		if(vec.Y is 1 or -1 && vec.X == 0 && vec.Z == 0)
		{
			return true;
		}
		if(vec.Z is 1 or -1 && vec.X == 0 && vec.Y == 0)
		{
			return true;
		}
		return false;
	}

	public override string Process(TextReader reader)
	{
		var vertices = ParseInput(reader);
		var count    = vertices.Count * 6;
		for(int i = 0; i < vertices.Count - 1; ++i)
		{
			var va = vertices[i];
			for(int j = i + 1; j < vertices.Count; ++j)
			{
				var vb = vertices[j];
				if(AreAdjacent(va, vb)) count -= 2;
			}
		}
		return count.ToString();
	}
}

public class Day18SolutionPart2 : Day18Solution
{
	public override string Process(TextReader reader)
	{
		var vertices = ParseInput(reader);
		throw new NotImplementedException();
	}
}
