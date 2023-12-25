using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AoC;

public readonly record struct Point3D(int X, int Y, int Z)
{
	public static readonly Point3D Zero = new(0, 0, 0);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point3D operator +(Point3D position, Vector3D offset)
		=> new(position.X + offset.DeltaX, position.Y + offset.DeltaY, position.Z + offset.DeltaZ);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point3D operator -(Point3D position, Vector3D offset)
		=> new(position.X - offset.DeltaX, position.Y - offset.DeltaY, position.Z - offset.DeltaZ);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3D operator -(Point3D left, Point3D right)
		=> new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

	public Point2D GetXYProjection() => new(X, Y);
}

public readonly record struct Point3D<T>(T X, T Y, T Z)
	: IAdditionOperators   <Point3D<T>, Vector3D<T>, Point3D<T>>,
	  ISubtractionOperators<Point3D<T>, Vector3D<T>, Point3D<T>>,
	  ISubtractionOperators<Point3D<T>, Point3D<T>, Vector3D<T>>,
	  IEqualityOperators   <Point3D<T>, Point3D<T>, bool>
	where T : INumber<T>
{
	public static readonly Point3D<T> Zero = new(T.Zero, T.Zero, T.Zero);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point3D<T> operator +(Point3D<T> position, Vector3D<T> offset)
		=> new(position.X + offset.DeltaX, position.Y + offset.DeltaY, position.Z + offset.DeltaZ);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Point3D<T> operator -(Point3D<T> position, Vector3D<T> offset)
		=> new(position.X - offset.DeltaX, position.Y - offset.DeltaY, position.Z - offset.DeltaZ);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3D<T> operator -(Point3D<T> left, Point3D<T> right)
		=> new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

	public Point2D<T> GetXYProjection() => new(X, Y);
}

public readonly record struct Vector3D(int DeltaX = 0, int DeltaY = 0, int DeltaZ = 0)
	: IAdditionOperators     <Vector3D, Vector3D, Vector3D>,
	  ISubtractionOperators  <Vector3D, Vector3D, Vector3D>,
	  IUnaryNegationOperators<Vector3D, Vector3D>,
	  IMultiplyOperators     <Vector3D, int, Vector3D>,
	  IEqualityOperators     <Vector3D, Vector3D, bool>
{
	public static readonly Vector3D Zero = new();

	public static Vector3D operator *(Vector3D offset, int m)
		=> new(offset.DeltaX * m, offset.DeltaY * m, offset.DeltaZ * m);

	public static Vector3D operator *(int m, Vector3D offset)
		=> new(offset.DeltaX * m, offset.DeltaY * m, offset.DeltaZ * m);

	public static Vector3D operator -(Vector3D offset)
		=> new(-offset.DeltaX, -offset.DeltaY, -offset.DeltaZ);

	public static Vector3D operator +(Vector3D a, Vector3D b)
		=> new(a.DeltaX + b.DeltaX, a.DeltaY + b.DeltaY, a.DeltaZ + b.DeltaZ);

	public static Vector3D operator -(Vector3D a, Vector3D b)
		=> new(a.DeltaX - b.DeltaX, a.DeltaY - b.DeltaY, a.DeltaZ - b.DeltaZ);

	public Vector2D GetXYProjection() => new(DeltaX, DeltaY);
}

public readonly record struct Vector3D<T>(T DeltaX = default!, T DeltaY = default!, T DeltaZ = default!)
	: IAdditionOperators     <Vector3D<T>, Vector3D<T>, Vector3D<T>>,
	  ISubtractionOperators  <Vector3D<T>, Vector3D<T>, Vector3D<T>>,
	  IUnaryNegationOperators<Vector3D<T>, Vector3D<T>>,
	  IMultiplyOperators     <Vector3D<T>, T, Vector3D<T>>,
	  IEqualityOperators     <Vector3D<T>, Vector3D<T>, bool>
	where T : INumber<T>
{
	public static readonly Vector3D<T> Zero  = new(T.Zero, T.Zero, T.Zero);

	public static Vector3D<T> operator *(Vector3D<T> offset, T m)
		=> new(offset.DeltaX * m, offset.DeltaY * m, offset.DeltaZ * m);

	public static Vector3D<T> operator *(T m, Vector3D<T> offset)
		=> new(offset.DeltaX * m, offset.DeltaY * m, offset.DeltaZ * m);

	public static Vector3D<T> operator -(Vector3D<T> offset)
		=> new(-offset.DeltaX, -offset.DeltaY, -offset.DeltaZ);

	public static Vector3D<T> operator +(Vector3D<T> a, Vector3D<T> b)
		=> new(a.DeltaX + b.DeltaX, a.DeltaY + b.DeltaY, a.DeltaZ + b.DeltaZ);

	public static Vector3D<T> operator -(Vector3D<T> a, Vector3D<T> b)
		=> new(a.DeltaX - b.DeltaX, a.DeltaY - b.DeltaY, a.DeltaZ - b.DeltaZ);

	public Vector2D<T> GetXYProjection() => new(DeltaX, DeltaY);
}

public static partial class Parsers
{
	public static Point3D ParsePoint3D(ReadOnlySpan<char> text, char separator = ',')
	{
		Span<Range> ranges = stackalloc Range[3];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 3)
		{
			throw new ArgumentException($"Invalid position description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			X: int.Parse(text[ranges[0]], formatProvider),
			Y: int.Parse(text[ranges[1]], formatProvider),
			Z: int.Parse(text[ranges[2]], formatProvider));
	}

	public static Point3D<T> ParsePoint3D<T>(ReadOnlySpan<char> text, char separator = ',')
		where T : INumber<T>, ISpanParsable<T>
	{
		Span<Range> ranges = stackalloc Range[3];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 3)
		{
			throw new ArgumentException($"Invalid position description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			X: T.Parse(text[ranges[0]], formatProvider),
			Y: T.Parse(text[ranges[1]], formatProvider),
			Z: T.Parse(text[ranges[2]], formatProvider));
	}

	public static Vector3D ParseVector3D(ReadOnlySpan<char> text, char separator = ',')
	{
		Span<Range> ranges = stackalloc Range[3];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 3)
		{
			throw new ArgumentException($"Invalid offset description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			DeltaX: int.Parse(text[ranges[0]], formatProvider),
			DeltaY: int.Parse(text[ranges[1]], formatProvider),
			DeltaZ: int.Parse(text[ranges[2]], formatProvider));
	}

	public static Vector3D<T> ParseVector3D<T>(ReadOnlySpan<char> text, char separator = ',')
		where T : INumber<T>, ISpanParsable<T>
	{
		Span<Range> ranges = stackalloc Range[3];
		if(text.Split(ranges, separator, StringSplitOptions.TrimEntries) != 3)
		{
			throw new ArgumentException($"Invalid offset description: {new string(text)}", nameof(text));
		}
		var formatProvider = CultureInfo.CurrentCulture;
		return new(
			DeltaX: T.Parse(text[ranges[0]], formatProvider),
			DeltaY: T.Parse(text[ranges[1]], formatProvider),
			DeltaZ: T.Parse(text[ranges[2]], formatProvider));
	}
}

public static partial class Geometry
{
	public static int ManhattanDistance(Point3D a, Point3D b)
		=> Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

	public static T ManhattanDistance<T>(Point3D<T> a, Point3D<T> b) where T : INumber<T>
		=> T.Abs(a.X - b.X) + T.Abs(a.Y - b.Y) + T.Abs(a.Z - b.Z);
}
