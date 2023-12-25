using System.Numerics;
using System.Runtime.CompilerServices;

namespace AoC;

public static class Mathematics
{
	/// <summary>Sorts 2 values.</summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="a">First value.</param>
	/// <param name="b">Second value.</param>
	/// <returns>Ordered values.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (T min, T max) Sort<T>(T a, T b)
		where T : IComparisonOperators<T, T, bool>
		=> a <= b ? (a, b) : (b, a);

	public static T GCD<T>(T a, T b)
		where T : INumber<T>
	{
		while(b != T.Zero)
		{
			var temp = b;
			b = a % b;
			a = temp;
		}
		return a;
	}

	public static T LCM<T>(T a, T b) where T : INumber<T>
		=> (a / GCD(a, b)) * b;

	public static T LCM<T>(ReadOnlySpan<T> span) where T : INumber<T>
	{
		var p = span[0];
		for(int i = 1; i < span.Length; ++i)
		{
			p = LCM(p, span[i]);
		}
		return p;
	}

	public static T LCM<T>(Span<T> span) where T : INumber<T>
		=> LCM((ReadOnlySpan<T>)span);

	public static T LCM<T>(T[] array) where T : INumber<T>
		=> LCM(array.AsSpan());

	public static T LCM<T>(IEnumerable<T> values) where T : INumber<T>
	{
		var p = T.MultiplicativeIdentity;
		foreach(var value in values)
		{
			p = LCM(p, value);
		}
		return p;
	}

	public static T Max<T>(ReadOnlySpan<T> span)
		where T : IComparisonOperators<T, T, bool>
	{
		var value = span[^1];
		for(int i = 0; i < span.Length - 1; ++i)
		{
			if(span[i] > value) value = span[i];
		}
		return value;
	}

	public static T Min<T>(ReadOnlySpan<T> span)
		where T : IComparisonOperators<T, T, bool>
	{
		var value = span[^1];
		for(int i = 0; i < span.Length - 1; ++i)
		{
			if(span[i] > value) value = span[i];
		}
		return value;
	}
}
