using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day14Test
{
	const string SampleInput =
		"""
		O....#....
		O.OO#....#
		.....##...
		OO.#O....O
		.O.....O#.
		O.#..O.#.#
		..O..#O..O
		.......O..
		#....###..
		#OO..#....
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day14SolutionPart1>(
		expected: "136", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day14SolutionPart2>(
		expected: "64", input: SampleInput);
}
