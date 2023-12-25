using NUnit.Framework;

namespace AoC.Year2021;

[TestFixture]
class Day03Test
{
	const string SampleInput1 =
		"""
		00100
		11110
		10110
		10111
		10101
		01111
		00111
		11100
		10000
		11001
		00010
		01010
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day03SolutionPart1>(
		expected: "198", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day03SolutionPart2>(
		expected: "230", input: SampleInput1);
}
