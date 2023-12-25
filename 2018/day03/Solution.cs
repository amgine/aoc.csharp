namespace AoC.Year2018;

[Name(@"No Matter How You Slice It")]
public abstract class Day03Solution : Solution
{
	protected readonly record struct Claim(int Id, Rectangle2D Area);

	protected static Claim ParseClaim(string line)
	{
		var s1 = line.IndexOf('@');
		var s2 = line.IndexOf(',', s1 + 1);
		var s3 = line.IndexOf(':', s2 + 1);
		var s4 = line.IndexOf('x', s3 + 1);

		var id = int.Parse(line.AsSpan(1, s1 - 1).Trim());
		var x  = int.Parse(line.AsSpan(s1 + 1, s2 - s1 - 1).Trim());
		var y  = int.Parse(line.AsSpan(s2 + 1, s3 - s2 - 1).Trim());
		var w  = int.Parse(line.AsSpan(s3 + 1, s4 - s3 - 1).Trim());
		var h  = int.Parse(line.AsSpan(s4 + 1).Trim());

		return new(id, new(new(x, y), new(w, h)));
	}
}

public sealed class Day03SolutionPart1 : Day03Solution
{
	/*
	static bool TryGetIntersection(Rectangle2D a, Rectangle2D b, out Rectangle2D intersection)
	{
		if(    TryGetIntersection(a.Position.X, a.Size.Width,  b.Position.X, b.Size.Width,  out var x, out var w)
			&& TryGetIntersection(a.Position.Y, a.Size.Height, b.Position.Y, b.Size.Height, out var y, out var h))
		{
			intersection = new(new(x, y), new(w, h));
			return true;
		}
		intersection = default;
		return false;
	}

	static bool TryGetIntersection(int x0, int l0, int x1, int l1, out int c, out int s)
	{
		if(x0 > x1)
		{
			(x1, x0) = (x0, x1);
			(l1, l0) = (l0, l1);
		}
		if(x0 + l0 < x1)
		{
			c = 0;
			s = 0;
			return false;
		}
		c = Math.Min(x0 + l0, x1 + l1);
		s = c - x1;
		return s > 0;
	}
	*/

	public override string Process(TextReader reader)
	{
		var claims = LoadListFromNonEmptyStrings(reader, ParseClaim);
		//var intersections = new List<Rectangle2D>();
		//for(int i = 0; i < claims.Count - 1; ++i)
		//{
		//	for(int j = i + 1; j < claims.Count; ++j)
		//	{
		//		if(TryGetIntersection(claims[i].Area, claims[j].Area, out var intersection))
		//		{
		//			intersections.Add(intersection);
		//		}
		//	}
		//}
		var area = 0;
		var counts = new int[1000, 1000];
		foreach(var claim in claims)
		{
			var y0 = claim.Area.Position.Y;
			var y1 = claim.Area.Position.Y + claim.Area.Size.Height;
			var x0 = claim.Area.Position.X;
			var x1 = claim.Area.Position.X + claim.Area.Size.Width;
			for(int y = y0; y < y1; ++y)
			{
				for(int x = x0; x < x1; ++x)
				{
					++counts[y, x];
				}
			}
		}
		foreach(var c in counts)
		{
			if(c > 1) ++area;
		}
		return area.ToString();
	}
}

public sealed class Day03SolutionPart2 : Day03Solution
{
	public override string Process(TextReader reader)
	{
		var claims = LoadListFromNonEmptyStrings(reader, ParseClaim);
		var flagged = new HashSet<int>();
		for(int i = 0; i < claims.Count; ++i)
		{
			bool intersects = flagged.Contains(i);
			for(int j = i + 1; j < claims.Count; ++j)
			{
				if(claims[i].Area.IntersectsWith(claims[j].Area))
				{
					flagged.Add(j);
					intersects = true;
				}
			}
			if(!intersects)
			{
				return claims[i].Id.ToString();
			}
		}
		throw new InvalidDataException("All claims intersect with each other");
	}
}
