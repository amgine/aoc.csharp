using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day11Test
{
	const string SampleInput =
		"""
		...#......
		.......#..
		#.........
		..........
		......#...
		.#........
		.........#
		..........
		.......#..
		#...#.....
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day11SolutionPart1>(
		expected: "374", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day11SolutionPart2>(
		expected: "82000210", input: SampleInput);
}
