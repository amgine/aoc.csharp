using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day09Test
{
	const string SampleInput1 =
		"""
		7,1
		11,1
		11,7
		9,7
		9,5
		2,5
		2,3
		7,3
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day09SolutionPart1>(
		expected: "50", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day09SolutionPart2>(
		expected: "24", input: SampleInput1);
}
