using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day04Test
{
	const string SampleInput =
		"""
		2-4,6-8
		2-3,4-5
		5-7,7-9
		2-8,3-7
		6-6,4-6
		2-6,4-8
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day04SolutionPart1>(
		expected: "2", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day04SolutionPart2>(
		expected: "4", input: SampleInput);
}
