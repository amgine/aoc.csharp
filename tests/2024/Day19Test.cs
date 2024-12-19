using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day19Test
{
	const string SampleInput1 =
		"""
		r, wr, b, g, bwu, rb, gb, br

		brwrr
		bggr
		gbbr
		rrbgbr
		ubwu
		bwurrg
		brgr
		bbrgwb
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day19SolutionPart1>(
		expected: "6", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day19SolutionPart2>(
		expected: "16", input: SampleInput1);
}
