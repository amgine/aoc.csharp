using System.Diagnostics.CodeAnalysis;

namespace AoC;

public static class HashSetExtensions
{
	public static bool TryGetAny<T>(this HashSet<T> set, [MaybeNullWhen(returnValue: false)] out T value)
	{
		using var e = set.GetEnumerator();
		if(!e.MoveNext())
		{
			value = default;
			return false;
		}
		value = e.Current;
		return true;
	}
}
