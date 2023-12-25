namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/23"/></remarks>
[Name(@"A Long Walk")]
public abstract class Day23Solution : Solution
{
}

public sealed class Day23SolutionPart1 : Day23Solution
{
	public sealed class MovesProvider(char[,] map) : IMovesProvider
	{
		public Span<Point2D> GetMoves(HashSet<Point2D> visited, Point2D position, Span<Point2D> buffer)
		{
			var current = position.GetValue(map);
			if(current != '.')
			{
				var offset = current switch
				{
					'<' => Vector2D.Left,
					'>' => Vector2D.Right,
					'^' => Vector2D.Up,
					'v' => Vector2D.Down,
					_ => throw new ApplicationException($"Invalid current position: {current} @ {position}"),
				};
				var pos = position + offset;
				if(pos.IsInside(map) && pos.GetValue(map) is not '#' && !visited.Contains(pos))
				{
					buffer[0] = pos;
					return buffer[..1];
				}
				return default;
			}

			var pos0 = position + Vector2D.Down;
			var pos1 = position + Vector2D.Left;
			var pos2 = position + Vector2D.Up;
			var pos3 = position + Vector2D.Right;

			static bool IsValidPosition(Point2D pos, char[,] map, HashSet<Point2D> visited)
				=> pos.IsInside(map) && pos.GetValue(map) is not '#' && !visited.Contains(pos);

			int count = 0;
			if(IsValidPosition(pos0, map, visited)) buffer[count++] = pos0;
			if(IsValidPosition(pos1, map, visited)) buffer[count++] = pos1;
			if(IsValidPosition(pos2, map, visited)) buffer[count++] = pos2;
			if(IsValidPosition(pos3, map, visited)) buffer[count++] = pos3;

			return buffer[..count];
		}
	}

	public override string Process(TextReader reader)
	{
		var map   = LoadCharMap2D(reader);
		var nodes = GraphBuilder.Build(map, new MovesProvider(map));
		return Graph.FindLongestPathLength(nodes[0], nodes[^1]).ToString();
	}
}

public sealed class Day23SolutionPart2 : Day23Solution
{
	public sealed class MovesProvider(char[,] map) : IMovesProvider
	{
		public Span<Point2D> GetMoves(HashSet<Point2D> visited, Point2D position, Span<Point2D> moves)
		{
			var pos0 = position + Vector2D.Down;
			var pos1 = position + Vector2D.Left;
			var pos2 = position + Vector2D.Up;
			var pos3 = position + Vector2D.Right;

			static bool IsValidPosition(Point2D pos, char[,] map, HashSet<Point2D> visited)
				=> pos.IsInside(map) && pos.GetValue(map) is not '#' && !visited.Contains(pos);

			int count = 0;
			if(IsValidPosition(pos0, map, visited)) moves[count++] = pos0;
			if(IsValidPosition(pos1, map, visited)) moves[count++] = pos1;
			if(IsValidPosition(pos2, map, visited)) moves[count++] = pos2;
			if(IsValidPosition(pos3, map, visited)) moves[count++] = pos3;

			return moves[..count];
		}
	}

	public override string Process(TextReader reader)
	{
		var map   = LoadCharMap2D(reader);
		var nodes = GraphBuilder.Build(map, new MovesProvider(map));
		return Graph.FindLongestPathLength(nodes[0], nodes[^1]).ToString();
	}
}
