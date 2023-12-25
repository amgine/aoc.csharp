using NUnit.Framework;

namespace AoC.Year2021;

[TestFixture]
class Day05Test
{
	const string SampleInput1 =
		"""

		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day05SolutionPart1>(
		expected: "198", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day05SolutionPart2>(
		expected: "230", input: SampleInput1);
}
