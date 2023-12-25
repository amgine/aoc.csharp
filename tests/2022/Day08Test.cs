using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day08Test
{
	const string SampleInput =
		"""
		30373
		25512
		65332
		33549
		35390
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day8SolutionPart1>(
		expected: "21", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day8SolutionPart2>(
		expected: "8", input: SampleInput);
}
