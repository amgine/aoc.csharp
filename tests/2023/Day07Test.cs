using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day07Test
{
	const string SampleInput =
		"""
		32T3K 765
		T55J5 684
		KK677 28
		KTJJT 220
		QQQJA 483
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day7SolutionPart1>(
		expected: "6440", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day7SolutionPart2>(
		expected: "5905", input: SampleInput);
}
