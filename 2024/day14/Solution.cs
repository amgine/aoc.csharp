namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/14"/></remarks>
[Name(@"Restroom Redoubt")]
public abstract class Day14Solution : Solution
{
	protected readonly record struct Robot(Point2D Position, Vector2D Velocity)
	{
		public Point2D GetPosition(int time, Size2D boxSize)
		{
			static int Constrain(int value, int max)
			{
				value %= max;
				return value >= 0 ? value : value + max;
			}

			var position = Position + Velocity * time;
			return new(
				X: Constrain(position.X, boxSize.Width),
				Y: Constrain(position.Y, boxSize.Height));
		}

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
	public static int Solve(TextReader reader, Size2D boxSize)
	{
		var robots = LoadListFromNonEmptyStrings(reader, Robot.Parse);

		var qs = boxSize / 2;
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
			var pos = robot.GetPosition(100, boxSize);
			if(pos.IsInside(r0)) ++c0;
			if(pos.IsInside(r1)) ++c1;
			if(pos.IsInside(r2)) ++c2;
			if(pos.IsInside(r3)) ++c3;
		}

		return c0 * c1 * c2 * c3;
	}

	public override string Process(TextReader reader)
		=> Solve(reader, new(101, 103)).ToString();
}

public sealed class Day14SolutionPart2 : Day14Solution
{
	public override string Process(TextReader reader)
	{
		var robots  = LoadListFromNonEmptyStrings(reader, Robot.Parse);
		var boxSize = new Size2D(101, 103);
		var unique  = new HashSet<Point2D>(capacity: robots.Count);
		var time    = 0;
		do
		{
			++time;
			unique.Clear();
			foreach(var robot in robots)
			{
				if(!unique.Add(robot.GetPosition(time, boxSize))) break;
			}
		}
		while(unique.Count != robots.Count);
		return time.ToString();
	}
}
