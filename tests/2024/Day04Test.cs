using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day04Test
{
	const string SampleInput1 =
		"""
		MMMSXXMASM
		MSAMXMSMSA
		AMXSXMAAMM
		MSAMASMSMX
		XMASAMXAMM
		XXAMMXXAMA
		SMSMSASXSS
		SAXAMASAAA
		MAMMMXMMMM
		MXMXAXMASX
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day04SolutionPart1>(
		expected: "18", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day04SolutionPart2>(
		expected: "9", input: SampleInput1);
}
