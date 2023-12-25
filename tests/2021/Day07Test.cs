using NUnit.Framework;

namespace AoC.Year2021;

[TestFixture]
class Day07Test
{
	const string SampleInput1 =
		"""
		16,1,2,0,4,2,7,1,2,14
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day07SolutionPart1>(
		expected: "37", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day07SolutionPart2>(
		expected: "168", input: SampleInput1);
}
