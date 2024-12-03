using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day01Test
{
	const string SampleInput1 =
		"""
		3   4
		4   3
		2   5
		1   3
		3   9
		3   3
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day01SolutionPart1>(
		expected: "11", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day01SolutionPart2>(
		expected: "31", input: SampleInput1);
}
