using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day02Test
{
	const string SampleInput =
		"""
		A Y
		B X
		C Z
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day02SolutionPart1>(
		expected: "15", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day02SolutionPart2>(
		expected: "12", input: SampleInput);
}
