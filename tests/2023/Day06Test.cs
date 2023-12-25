using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day06Test
{
	const string SampleInput =
		"""
		Time:      7  15   30
		Distance:  9  40  200
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day6SolutionPart1>(
		expected: "288", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day6SolutionPart2>(
		expected: "71503", input: SampleInput);
}
