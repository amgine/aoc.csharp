using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day21Test
{
	const string SampleInput1 =
		"""
		029A
		980A
		179A
		456A
		379A
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day21SolutionPart1>(
		expected: "126384", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day21SolutionPart2>(
		expected: "154115708116294", input: SampleInput1);
}
