using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day05Test
{
	const string SampleInput1 =
		"""
		3-5
		10-14
		16-20
		12-18

		1
		5
		8
		11
		17
		32
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day05SolutionPart1>(
		expected: "3", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day05SolutionPart2>(
		expected: "14", input: SampleInput1);
}
