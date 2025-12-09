namespace AoC.Year2025;

/// <remarks><a href="https://adventofcode.com/2025/day/9"/></remarks>
[Name(@"XXXXXX")]
public abstract class Day09Solution : Solution
{
}

public sealed class Day09SolutionPart1 : Day09Solution
{
	public override string Process(TextReader reader)
	{
		var points = LoadListFromNonEmptyStrings(reader, static line => Parsers.ParsePoint2D(line));
		var maxArea = 0L;
		for(int i = 0; i < points.Count - 1; ++i)
		{
			var p0 = points[i];
			for(int j = i + 1; j < points.Count; ++j)
			{
				var p1 = points[j];
				var x0 = Math.Min(p0.X, p1.X);
				var x1 = Math.Max(p0.X, p1.X);
				var y0 = Math.Min(p0.Y, p1.Y);
				var y1 = Math.Max(p0.Y, p1.Y);
				var area = (long)(x1 - x0 + 1) * (long)(y1 - y0 + 1);
				if(area > maxArea) maxArea = area;
			}
		}
		return maxArea.ToString();
	}
}

public sealed class Day09SolutionPart2 : Day09Solution
{
	static bool IsInside(Point2D p, List<Point2D> polygon)
	{
		var odd = false;
		for(int i = 0, j = polygon.Count - 1; i < polygon.Count; i++)
		{
			var p0 = polygon[i];
			var p1 = polygon[j];
			if(p0.X == p1.X)
			{
				var y0 = Math.Min(p0.Y, p1.Y);
				var y1 = Math.Max(p0.Y, p1.Y);
				if(p.X == p0.X && p.Y >= y0 && p.Y <= y1) return true;
			}
			else
			{
				var x0 = Math.Min(p0.X, p1.X);
				var x1 = Math.Max(p0.X, p1.X);
				if(p.Y == p0.Y && p.X >= x0 && p.X <= x1) return true;
			}
			if(polygon[i].Y < p.Y && polygon[j].Y >= p.Y
			|| polygon[j].Y < p.Y && polygon[i].Y >= p.Y)
			{
				if(polygon[i].X + (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < p.X)
				{
					odd = !odd;
				}
			}
			j = i;
		}
		return odd;
	}

	static bool Intersects(Point2D a0, Point2D a1, Point2D b0, Point2D b1)
	{
		if(a0.X == a1.X)
		{
			if(b0.X == b1.X) return false;

			var x0 = Math.Min(b0.X, b1.X);
			var x1 = Math.Max(b0.X, b1.X);

			if(x0 >= a0.X || x1 <= a0.X) return false;

			var y0 = Math.Min(a0.Y, a1.Y);
			var y1 = Math.Max(a0.Y, a1.Y);

			return y0 < b0.Y && y1 > b0.Y;
		}
		else if(a0.Y == a1.Y)
		{
			if(b0.Y == b1.Y) return false;

			var y0 = Math.Min(b0.Y, b1.Y);
			var y1 = Math.Max(b0.Y, b1.Y);

			if(y0 >= a0.Y || y1 <= a0.Y) return false;

			var x0 = Math.Min(a0.X, a1.X);
			var x1 = Math.Max(a0.X, a1.X);

			return x0 < b0.X && x1 > b0.X;
		}
		else throw new InvalidDataException();
	}

	static bool Intersects(Point2D a0, Point2D a1, List<Point2D> points)
	{
		var p = points[0];
		for(int i = 1; i < points.Count; i++)
		{
			var c = points[i];
			if(Intersects(a0, a1, c, p)) return true;
			p = c;
		}
		if(Intersects(a0, a1, points[0], p)) return true;
		return false;
	}

	public override string Process(TextReader reader)
	{
		var points = LoadListFromNonEmptyStrings(reader, static line => Parsers.ParsePoint2D(line));
		var maxArea = 0L;
		for(int i = 0; i < points.Count - 1; ++i)
		{
			var p0 = points[i];
			for(int j = i + 1; j < points.Count; ++j)
			{
				var p1 = points[j];
				var x0 = Math.Min(p0.X, p1.X);
				var x1 = Math.Max(p0.X, p1.X);
				var y0 = Math.Min(p0.Y, p1.Y);
				var y1 = Math.Max(p0.Y, p1.Y);
				var w = x1 - x0 + 1;
				var h = y1 - y0 + 1;
				var area = (long)w * (long)h;
				if(area <= maxArea) continue;
				if(Intersects(new(x0, y0), new(x1, y0), points)) continue;
				if(Intersects(new(x1, y0), new(x1, y1), points)) continue;
				if(Intersects(new(x1, y1), new(x0, y1), points)) continue;
				if(Intersects(new(x0, y1), new(x0, y0), points)) continue;
				if(!IsInside(new(x0, y0), points)) continue;
				if(!IsInside(new(x0, y1), points)) continue;
				if(!IsInside(new(x1, y1), points)) continue;
				if(!IsInside(new(x1, y0), points)) continue;
				maxArea = area;
			}
		}
		return maxArea.ToString();
	}
}
