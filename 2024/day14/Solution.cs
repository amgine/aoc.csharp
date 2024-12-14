namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/14"/></remarks>
[Name(@"Restroom Redoubt")]
public abstract class Day14Solution : Solution
{
	protected static Point2D Move(Point2D position, Vector2D velocity, int time, int w, int h)
	{
		static int Constrain(int value, int max)
		{
			if(value >= max) return value % max;
			if(value < 0)    return value + ((-value + max - 1) / max * max);
			return value;
		}

		position += velocity * time;
		return new(Constrain(position.X, w), Constrain(position.Y, h));
	}

	protected readonly record struct Robot(Point2D Position, Vector2D Velocity)
	{
		public static Robot Parse(string line)
		{
			var i0 = line.IndexOf("p=");
			var i1 = line.IndexOf(' ', i0 + 2);
			var i2 = line.IndexOf("v=", i1 + 1);
			return new(
				Position: Parsers.ParsePoint2D(line.AsSpan(i0 + 2, i1 - i0 - 2)),
				Velocity: Parsers.ParseVector2D(line.AsSpan(i2 + 2)));
		}
	}
}

public sealed class Day14SolutionPart1 : Day14Solution
{
	public static int Solve(TextReader reader, int w, int h)
	{
		var robots = LoadListFromNonEmptyStrings(reader, Robot.Parse);

		var qs = new Size2D(w / 2, h / 2);
		var r0 = new Rectangle2D(new(0, 0), qs);
		var r1 = new Rectangle2D(new(qs.Width + 1, 0), qs);
		var r2 = new Rectangle2D(new(0, qs.Height + 1), qs);
		var r3 = new Rectangle2D(new(qs.Width + 1, qs.Height + 1), qs);

		var c0 = 0;
		var c1 = 0;
		var c2 = 0;
		var c3 = 0;

		foreach(var robot in robots)
		{
			var pos = Move(robot.Position, robot.Velocity, 100, w, h);
			if(pos.IsInside(r0)) ++c0;
			if(pos.IsInside(r1)) ++c1;
			if(pos.IsInside(r2)) ++c2;
			if(pos.IsInside(r3)) ++c3;
		}

		return c0 * c1 * c2 * c3;
	}

	public override string Process(TextReader reader)
		=> Solve(reader, 101, 103).ToString();
}

public sealed class Day14SolutionPart2 : Day14Solution
{
	public override string Process(TextReader reader)
	{
		var robots = LoadListFromNonEmptyStrings(reader, Robot.Parse);

		const int w = 101;
		const int h = 103;

		var unique = new HashSet<Point2D>(capacity: robots.Count);
		var time = 1;
		while(true)
		{
			foreach(var robot in robots)
			{
				unique.Add(Move(robot.Position, robot.Velocity, time, w, h));
			}

			if(unique.Count == robots.Count) break;
			++time;
			unique.Clear();
		}
		return time.ToString();
	}
}
