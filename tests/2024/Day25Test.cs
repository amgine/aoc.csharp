using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day25Test
{
	const string SampleInput1 =
		"""
		#####
		.####
		.####
		.####
		.#.#.
		.#...
		.....

		#####
		##.##
		.#.##
		...##
		...#.
		...#.
		.....

		.....
		#....
		#....
		#...#
		#.#.#
		#.###
		#####

		.....
		.....
		#.#..
		###..
		###.#
		###.#
		#####

		.....
		.....
		.....
		#....
		#.#..
		#.#.#
		#####
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day25SolutionPart1>(
		expected: "3", input: SampleInput1);
}
