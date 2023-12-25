using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day03Test
{
	const string SampleInput =
		"""
		467..114..
		...*......
		..35..633.
		......#...
		617*......
		.....+.58.
		..592.....
		......755.
		...$.*....
		.664.598..
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day3SolutionPart1>(
		expected: "4361", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day3SolutionPart2>(
		expected: "467835", input: SampleInput);
}
