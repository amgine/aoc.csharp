using NUnit.Framework;

namespace AoC.Year2021;

[TestFixture]
class Day06Test
{
	const string SampleInput1 =
		"""
		3,4,3,1,2
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day06SolutionPart1>(
		expected: "5934", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day06SolutionPart2>(
		expected: "26984457539", input: SampleInput1);
}
