using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day02Test
{
	const string SampleInput1 =
		"""
		7 6 4 2 1
		1 2 7 8 9
		9 7 6 2 1
		1 3 2 4 5
		8 6 4 4 1
		1 3 6 7 9
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day02SolutionPart1>(
		expected: "2", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day02SolutionPart2>(
		expected: "4", input: SampleInput1);
}
