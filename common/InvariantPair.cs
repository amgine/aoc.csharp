using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AoC;

public readonly struct InvariantPair(int a, int b)
	: IEquatable<InvariantPair>, IEqualityOperators<InvariantPair, InvariantPair, bool>
{
	private readonly int _a = a;
	private readonly int _b = b;

	public override int GetHashCode() => _a ^ _b;

	public override bool Equals([NotNullWhen(true)] object? obj)
		=> obj is InvariantPair other && this == other;

	public bool Equals(InvariantPair other) => this == other;

	public static bool operator ==(InvariantPair left, InvariantPair right)
		=> (left._a == right._a && left._b == right._b)
		|| (left._a == right._b && left._b == right._a);

	public static bool operator !=(InvariantPair left, InvariantPair right)
		=> !(left == right);

	public override string ToString() => $"{_a}, {_b}";
}
