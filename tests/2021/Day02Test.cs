using NUnit.Framework;

namespace AoC.Year2021;

[TestFixture]
class Day02Test
{
	const string SampleInput1 =
		"""
		forward 5
		down 5
		forward 8
		up 3
		down 8
		forward 2
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day02SolutionPart1>(
		expected: "150", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day02SolutionPart2>(
		expected: "900", input: SampleInput1);
}
