using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day22Test
{
	const string SampleInput1 =
		"""
		1
		10
		100
		2024
		""";

	const string SampleInput2 =
		"""
		1
		2
		3
		2024
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day22SolutionPart1>(
		expected: "37327623", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day22SolutionPart2>(
		expected: "23", input: SampleInput2);
}
