using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day07Test
{
	const string SampleInput =
		"""
		123 -> x
		456 -> b
		x AND b -> d
		x OR b -> e
		x LSHIFT 2 -> f
		b RSHIFT 2 -> g
		NOT x -> h
		NOT b -> a
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day07SolutionPart1>(
		expected: "65079", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day07SolutionPart2>(
		expected: "456", input: SampleInput);
}
