namespace AoC.Year2021;

[Name(@"Sea Cucumber")]
public abstract class Day25Solution : Solution
{
}

public sealed class Day25SolutionPart1 : Day25Solution
{
	static bool MoveEast(char[,] map)
	{
		var toMove = new List<Point2D>();
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] != '>') continue;
				var next = (x + 1) % map.GetLength(1);
				if(map[y, next] != '.') continue;
				toMove.Add(new Point2D(x, y));
			}
		}
		if(toMove.Count == 0) return false;
		foreach(var p in toMove)
		{
			p.GetValue(map) = '.';
			var next = (p.X + 1) % map.GetLength(1);
			map[p.Y, next] = '>';
		}
		return true;
	}

	static bool MoveNorth(char[,] map)
	{
		var toMove = new List<Point2D>();
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				if(map[y, x] != 'v') continue;
				var next = (y + 1) % map.GetLength(0);
				if(map[next, x] != '.') continue;
				toMove.Add(new Point2D(x, y));
			}
		}
		if(toMove.Count == 0) return false;
		foreach(var p in toMove)
		{
			p.GetValue(map) = '.';
			var next = (p.Y + 1) % map.GetLength(0);
			map[next, p.X] = 'v';
		}
		return true;
	}

	public override string Process(TextReader reader)
	{
		var map = LoadCharMap2D(reader);
		var steps = 1;
		while(MoveEast(map) | MoveNorth(map))
		{
			++steps;
		}
		return steps.ToString();
	}
}
