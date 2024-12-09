using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day09Test
{
	const string SampleInput1 =
		"""
		2333133121414131402
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day09SolutionPart1>(
		expected: "1928", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day09SolutionPart2>(
		expected: "2858", input: SampleInput1);
}
