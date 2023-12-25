using System;
using System.Numerics;

namespace AoC;

public static class SpanHelper
{
	public static T Sum<T>(ReadOnlySpan<T> span)
		where T : INumberBase<T>
	{
		var sum = T.Zero;
		foreach(var item in span) sum += item;
		return sum;
	}
}
