using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day01Test
{
	const string SampleInput =
		"""
		1000
		2000
		3000

		4000

		5000
		6000

		7000
		8000
		9000

		10000
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day01SolutionPart1>(
		expected: "24000", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day01SolutionPart2>(
		expected: "45000", input: SampleInput);
}
