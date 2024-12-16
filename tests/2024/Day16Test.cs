using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day16Test
{
	const string SampleInput1 =
		"""
		###############
		#.......#....E#
		#.#.###.#.###.#
		#.....#.#...#.#
		#.###.#####.#.#
		#.#.#.......#.#
		#.#.#####.###.#
		#...........#.#
		###.#.#####.#.#
		#...#.....#.#.#
		#.#.#.###.#.#.#
		#.....#...#.#.#
		#.###.#.#.#.#.#
		#S..#.....#...#
		###############
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day16SolutionPart1>(
		expected: "7036", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day16SolutionPart2>(
		expected: "45", input: SampleInput1);
}
