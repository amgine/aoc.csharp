using NUnit.Framework;

namespace AoC.Year2020;

[TestFixture]
class Day01Test
{
	const string SampleInput1 =
		"""
		1721
		979
		366
		299
		675
		1456
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day01SolutionPart1>(
		expected: "514579", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day01SolutionPart2>(
		expected: "241861950", input: SampleInput1);
}
