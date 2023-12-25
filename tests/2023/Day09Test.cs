using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day09Test
{
	const string SampleInput =
		"""
		0 3 6 9 12 15
		1 3 6 10 15 21
		10 13 16 21 30 45
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day9SolutionPart1>(
		expected: "114", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day9SolutionPart2>(
		expected: "2", input: SampleInput);
}
