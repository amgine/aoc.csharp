using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AoC;

public enum Direction2D { Left, Up, Right, Down }

public static class Direction2DExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Direction2D RotateCW(this Direction2D direction)
		=> (Direction2D)(((int)direction + 1) % 4);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Direction2D RotateCW(this Direction2D direction, int count)
		=> (Direction2D)(((int)direction + count) % 4);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Direction2D RotateCCW(this Direction2D direction)
		=> (Direction2D)(((int)direction + 3) % 4);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Direction2D RotateCCW(this Direction2D direction, int count)
		=> (Direction2D)(((int)direction + count + 2) % 4);
}

public static partial class Parsers
{
	public static Point2D ParsePoint2D(ReadOnlySpan<char> text, char separator = ',')
	{
		Span<Range> ranges = stackalloc Range[2];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 2)
		{
			throw new ArgumentException($"Invalid position description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			X: int.Parse(text[ranges[0]], formatProvider),
			Y: int.Parse(text[ranges[1]], formatProvider));
	}

	public static Vector2D ParseVector2D(ReadOnlySpan<char> text, char separator = ',')
	{
		Span<Range> ranges = stackalloc Range[2];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 2)
		{
			throw new ArgumentException($"Invalid position description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			DeltaX: int.Parse(text[ranges[0]], formatProvider),
			DeltaY: int.Parse(text[ranges[1]], formatProvider));
	}

	public static Point2D<T> ParsePoint2D<T>(ReadOnlySpan<char> text, char separator = ',')
		where T : INumber<T>, ISpanParsable<T>
	{
		Span<Range> ranges = stackalloc Range[2];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 2)
		{
			throw new ArgumentException($"Invalid position description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			X: T.Parse(text[ranges[0]], formatProvider),
			Y: T.Parse(text[ranges[1]], formatProvider));
	}

	public static Vector2D<T> ParseVector2D<T>(ReadOnlySpan<char> text, char separator = ',')
		where T : INumber<T>, ISpanParsable<T>
	{
		Span<Range> ranges = stackalloc Range[2];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 2)
		{
			throw new ArgumentException($"Invalid position description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			DeltaX: T.Parse(text[ranges[0]], formatProvider),
			DeltaY: T.Parse(text[ranges[1]], formatProvider));
	}
}

public static partial class Geometry
{
	public static int ManhattanDistance(Point2D a, Point2D b)
		=> Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

	public static T ManhattanDistance<T>(Point2D<T> a, Point2D<T> b) where T : INumber<T>
		=> T.Abs(a.X - b.X) + T.Abs(a.Y - b.Y);

	static bool RayContainsPoint<T, V>(in Ray2D<T> ray, in Point2D<V> intersection)
		where T : INumber<T>
		where V : INumber<V>
	{
		var dx1 = intersection.X - V.CreateChecked(ray.Origin.X);
		var dy1 = intersection.Y - V.CreateChecked(ray.Origin.Y);
		return V.Sign(dx1) == T.Sign(ray.Vector.DeltaX)
			&& V.Sign(dy1) == T.Sign(ray.Vector.DeltaY);
	}

	static Point2D<V> GetIntersectionSaturating<T, V>(in Ray2D<T> ray1, in Ray2D<T> ray2, T d)
		where T : INumber<T>
		where V : INumber<V>
	{
		var p2 = ray1.Origin + ray1.Vector;
		var p4 = ray2.Origin + ray2.Vector;

		var a = ray2.Origin.X * p4.Y - ray2.Origin.Y * p4.X;
		var b = ray1.Origin.X * p2.Y - ray1.Origin.Y * p2.X;

		var dd = V.CreateSaturating(d);
		var x  = V.CreateSaturating(a * ray1.Vector.DeltaX - b * ray2.Vector.DeltaX) / dd;
		var y  = V.CreateSaturating(a * ray1.Vector.DeltaY - b * ray2.Vector.DeltaY) / dd;

		return new(x, y);
	}

	static Point2D<V> GetIntersectionTruncating<T, V>(in Ray2D<T> ray1, in Ray2D<T> ray2, T d)
		where T : INumber<T>
		where V : INumber<V>
	{
		var p2 = ray1.Origin + ray1.Vector;
		var p4 = ray2.Origin + ray2.Vector;

		var a = ray2.Origin.X * p4.Y - ray2.Origin.Y * p4.X;
		var b = ray1.Origin.X * p2.Y - ray1.Origin.Y * p2.X;

		var dd = V.CreateTruncating(d);
		var x  = V.CreateTruncating(a * ray1.Vector.DeltaX - b * ray2.Vector.DeltaX) / dd;
		var y  = V.CreateTruncating(a * ray1.Vector.DeltaY - b * ray2.Vector.DeltaY) / dd;

		return new(x, y);
	}

	static Point2D<V> GetIntersectionChecked<T, V>(in Ray2D<T> ray1, in Ray2D<T> ray2, T d)
		where T : INumber<T>
		where V : INumber<V>
	{
		var p2 = ray1.Origin + ray1.Vector;
		var p4 = ray2.Origin + ray2.Vector;

		var a = ray2.Origin.X * p4.Y - ray2.Origin.Y * p4.X;
		var b = ray1.Origin.X * p2.Y - ray1.Origin.Y * p2.X;

		var dd = V.CreateChecked(d);
		var x  = V.CreateChecked(a * ray1.Vector.DeltaX - b * ray2.Vector.DeltaX) / dd;
		var y  = V.CreateChecked(a * ray1.Vector.DeltaY - b * ray2.Vector.DeltaY) / dd;

		return new(x, y);
	}

	static T GetDiscriminant<T>(in Vector2D<T> vector1, in Vector2D<T> vector2) where T : INumber<T>
		=> vector1.DeltaX * vector2.DeltaY
		 - vector1.DeltaY * vector2.DeltaX;

	public static bool TryGetIntersection<T, V>(in Ray2D<T> ray1, in Ray2D<T> ray2, out Point2D<V> intersection)
		where T : INumber<T>
		where V : INumber<V>
	{
		var d = GetDiscriminant(ray1.Vector, ray2.Vector);

		if(T.IsZero(d)) goto no_intersection;

		intersection = GetIntersectionChecked<T, V>(ray1, ray2, d);

		if(!RayContainsPoint(ray1, intersection)) goto no_intersection;
		if(!RayContainsPoint(ray2, intersection)) goto no_intersection;

		return true;

	no_intersection:
		intersection = default;
		return false;
	}
}

public readonly record struct Point2D(int X, int Y)
	: IAdditionOperators   <Point2D, Vector2D, Point2D>,
	  ISubtractionOperators<Point2D, Vector2D, Point2D>,
	  ISubtractionOperators<Point2D, Point2D, Vector2D>,
	  IEqualityOperators   <Point2D, Point2D, bool>
{
	public static readonly Point2D Zero = new(0, 0);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsInside<T>(T[,] map)
		=> X >= 0
		&& Y >= 0
		&& X < map.GetLength(1)
		&& Y < map.GetLength(0);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point2D operator +(Point2D position, Vector2D offset)
		=> new(position.X + offset.DeltaX, position.Y + offset.DeltaY);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point2D operator -(Point2D position, Vector2D offset)
		=> new(position.X - offset.DeltaX, position.Y - offset.DeltaY);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2D operator -(Point2D left, Point2D right)
		=> new(DeltaX: left.X - right.X, DeltaY: left.Y - right.Y);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref T GetValue<T>(T[,] map)
		=> ref map[Y, X];
}

public readonly record struct Point2D<T>(T X, T Y)
	: IAdditionOperators   <Point2D<T>, Vector2D<T>, Point2D<T>>,
	  ISubtractionOperators<Point2D<T>, Vector2D<T>, Point2D<T>>,
	  ISubtractionOperators<Point2D<T>, Point2D<T>, Vector2D<T>>,
	  IEqualityOperators   <Point2D<T>, Point2D<T>, bool>
	where T : INumber<T>
{
	public static readonly Point2D<T> Zero = new(T.Zero, T.Zero);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsInside<K>(K[,] map)
		=> X >= T.Zero
		&& Y >= T.Zero
		&& X <  T.CreateChecked(map.GetLength(1))
		&& Y <  T.CreateChecked(map.GetLength(0));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point2D<T> operator +(Point2D<T> position, Vector2D<T> offset)
		=> new(position.X + offset.DeltaX, position.Y + offset.DeltaY);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point2D<T> operator -(Point2D<T> position, Vector2D<T> offset)
		=> new(position.X - offset.DeltaX, position.Y - offset.DeltaY);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2D<T> operator -(Point2D<T> left, Point2D<T> right)
		=> new(DeltaX: left.X - right.X, DeltaY: left.Y - right.Y);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref K GetValue<K>(K[,] map)
		=> ref map[int.CreateChecked(Y), int.CreateChecked(X)];
}

public readonly record struct Rectangle2D(Point2D Position, Size2D Size)
{
	public static readonly Rectangle2D Empty = new(Point2D.Zero, Size2D.Zero);

	public static Rectangle2D FromCorners(Point2D topLeft, Point2D bottomRight)
		=> new(topLeft, new(Width: bottomRight.X - topLeft.X + 1, Height: bottomRight.Y - topLeft.Y + 1));

	public bool Contains(Rectangle2D other)
		=> Position.X <= other.Position.X
		&& Position.Y <= other.Position.Y
		&& Position.X + Size.Width  >= other.Position.X + other.Size.Width
		&& Position.Y + Size.Height >= other.Position.Y + other.Size.Height;

	static bool Intersects(int x0, int w0, int x1, int w1)
		=> (x0 >= x1 && x0 < x1 + w1)
		|| (x1 >= x0 && x1 < x0 + w0);

	public bool IntersectsWith(Rectangle2D other)
		=> Intersects(Position.X, Size.Width,  other.Position.X, other.Size.Width)
		&& Intersects(Position.Y, Size.Height, other.Position.Y, other.Size.Height);

	public static int UnionAsNonIntersecting(Rectangle2D a, Rectangle2D b, Span<Rectangle2D> output)
	{
		var ax0 = a.Position.X;
		var ax1 = a.Position.X + a.Size.Width - 1;
		var ay0 = a.Position.Y;
		var ay1 = a.Position.Y + a.Size.Height - 1;
		var bx0 = b.Position.X;
		var bx1 = b.Position.X + b.Size.Width - 1;
		var by0 = b.Position.Y;
		var by1 = b.Position.Y + b.Size.Height - 1;

		var aIsEmpty = ax1 < ax0 || ay1 < ay0;
		var bIsEmpty = bx1 < ax0 || by1 < ay0;

		if(aIsEmpty)
		{
			if(bIsEmpty) return 0;
			output[0] = b;
			return 1;
		}
		if(bIsEmpty)
		{
			output[0] = a;
			return 1;
		}
		if(ax1 < bx0 || ax0 > bx1 || ay1 < by0 || ay0 > by1)
		{
			output[0] = a;
			output[1] = b;
			return 2;
		}

		if(ax0 <= bx0)
		{
			if(ax1 >= bx1)
			{
				//   ax0 <= bx0 <= bx1 <= ax1
				if(ay0 <= by0)
				{
					if(ay1 >= by1)
					{
						output[0] = a;
						return 1;
					}
					else
					{
						output[0] = a;
						by0 = ay1 + 1;
						output[1] = new(new(bx0, by0), new(bx1 - bx0 + 1, by1 - by0 + 1));
						return 2;
					}
				}
				else
				{
					if(ay1 >= by1)
					{
						output[0] = a;
						by1 = ay1 - 1;
						output[1] = new(new(bx0, by0), new(bx1 - bx0 + 1, by1 - by0 + 1));
						return 2;
					}
					else
					{
						//   ax0  bx0   bx1   ax1
						// by0     +-----+
						//         |     |
						// ay0 +---+-----+-----+
						//     |   |     |     |
						// ay1 +---+-----+-----+
						//         |     |
						// by1     +-----+
						output[0] = a;
						var h1 = ay0 - by0;
						output[1] = new(new(bx0, by0), new(bx1 - bx0 + 1, h1));
						output[2] = new(new(bx0, ay1 + 1), new(bx1 - bx0 + 1, by1 - ay0));
						return 3;
					}
				}
			}
			else
			{
				//   ax0 <= bx0 <= ax1 < bx1
			}
		}
		else
		{
			if(ax1 <= bx1)
			{
				if(by0 <= ay0)
				{
					if(by1 >= ay1)
					{
						output[0] = b;
						return 1;
					}
					else
					{
						ay0 = by1 + 1;
						output[0] = new(new(ax0, ay0), new(ax1 - ax0 + 1, ay1 - ay0 + 1));
						output[1] = b;
						return 2;
					}
				}
				else
				{

				}
			}
			else
			{
			}
		}


		throw new NotImplementedException();
	}
}

public readonly record struct Vector2D(int DeltaX = 0, int DeltaY = 0)
	: IAdditionOperators     <Vector2D, Vector2D, Vector2D>,
	  ISubtractionOperators  <Vector2D, Vector2D, Vector2D>,
	  IUnaryNegationOperators<Vector2D, Vector2D>,
	  IMultiplyOperators     <Vector2D, int, Vector2D>,
	  IEqualityOperators     <Vector2D, Vector2D, bool>
{
	public static readonly Vector2D Zero  = new();

	public static readonly Vector2D Left  = new(DeltaX: -1);
	public static readonly Vector2D Up    = new(DeltaY: -1);
	public static readonly Vector2D Right = new(DeltaX:  1);
	public static readonly Vector2D Down  = new(DeltaY:  1);

	static readonly Vector2D[] Directions = [Left, Up, Right, Down];

	public static Vector2D FromDirection(Direction2D direction)
		=> Directions[(int)direction];

	public static Vector2D operator *(Vector2D offset, int m)
		=> new(offset.DeltaX * m, offset.DeltaY * m);

	public static Vector2D operator *(int m, Vector2D offset)
		=> new(offset.DeltaX * m, offset.DeltaY * m);

	public static Vector2D operator -(Vector2D offset)
		=> new(-offset.DeltaX, -offset.DeltaY);

	public static Vector2D operator +(Vector2D a, Vector2D b)
		=> new(a.DeltaX + b.DeltaX, a.DeltaY + b.DeltaY);

	public static Vector2D operator -(Vector2D a, Vector2D b)
		=> new(a.DeltaX - b.DeltaX, a.DeltaY - b.DeltaY);
}

public readonly record struct Vector2D<T>(T DeltaX = default!, T DeltaY = default!)
	: IAdditionOperators     <Vector2D<T>, Vector2D<T>, Vector2D<T>>,
	  ISubtractionOperators  <Vector2D<T>, Vector2D<T>, Vector2D<T>>,
	  IUnaryNegationOperators<Vector2D<T>, Vector2D<T>>,
	  IMultiplyOperators     <Vector2D<T>, T, Vector2D<T>>,
	  IEqualityOperators     <Vector2D<T>, Vector2D<T>, bool>
	where T : INumber<T>
{
	public static readonly Vector2D<T> Zero  = new(T.Zero, T.Zero);

	public static readonly Vector2D<T> Left  = new(DeltaX: -T.One);
	public static readonly Vector2D<T> Up    = new(DeltaY: -T.One);
	public static readonly Vector2D<T> Right = new(DeltaX:  T.One);
	public static readonly Vector2D<T> Down  = new(DeltaY:  T.One);

	static readonly Vector2D<T>[] Directions = [Left, Up, Right, Down];

	public static Vector2D<T> FromDirection(Direction2D direction)
		=> Directions[(int)direction];

	public static Vector2D<T> operator *(Vector2D<T> offset, T m)
		=> new(offset.DeltaX * m, offset.DeltaY * m);

	public static Vector2D<T> operator *(T m, Vector2D<T> offset)
		=> new(offset.DeltaX * m, offset.DeltaY * m);

	public static Vector2D<T> operator -(Vector2D<T> offset)
		=> new(-offset.DeltaX, -offset.DeltaY);

	public static Vector2D<T> operator +(Vector2D<T> a, Vector2D<T> b)
		=> new(a.DeltaX + b.DeltaX, a.DeltaY + b.DeltaY);

	public static Vector2D<T> operator -(Vector2D<T> a, Vector2D<T> b)
		=> new(a.DeltaX - b.DeltaX, a.DeltaY - b.DeltaY);
}

public readonly record struct Ray2D<T>(
	Point2D <T> Origin,
	Vector2D<T> Vector)
	where T : INumber<T>
{
}

public readonly record struct Size2D(int Width, int Height)
{
	public static readonly Size2D Zero = new(0, 0);

	public static Size2D FromArray<T>(T[,] array)
		=> new(array.GetLength(1), array.GetLength(0));

	public int GetArea() => Width * Height;

	public static Size2D operator -(Size2D a)
		=> new(-a.Width, -a.Height);

	public static Size2D operator +(Size2D a, Size2D b)
		=> new(a.Width + b.Width, a.Height + b.Height);

	public static Size2D operator -(Size2D a, Size2D b)
		=> new(a.Width - b.Width, a.Height - b.Height);

	public static Size2D operator *(Size2D a, int b)
		=> new(a.Width * b, a.Height * b);
}

public static class Display2D
{
	public static void Print<T>(T[,] map, Func<T, char> convert)
		=> Print(map, Console.Out, convert);

	public static void Print<T>(T[,] map, TextWriter textWriter, Func<T, char> convert)
	{
		for(int y = 0; y < map.GetLength(0); ++y)
		{
			for(int x = 0; x < map.GetLength(1); ++x)
			{
				textWriter.Write(convert(map[y, x]));
			}
			textWriter.WriteLine();
		}
	}
}
