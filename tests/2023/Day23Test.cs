using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day23Test
{
	const string SampleInput =
		"""
		#.#####################
		#.......#########...###
		#######.#########.#.###
		###.....#.>.>.###.#.###
		###v#####.#v#.###.#.###
		###.>...#.#.#.....#...#
		###v###.#.#.#########.#
		###...#.#.#.......#...#
		#####.#.#.#######.#.###
		#.....#.#.#.......#...#
		#.#####.#.#.#########v#
		#.#...#...#...###...>.#
		#.#.#v#######v###.###v#
		#...#.>.#...>.>.#.###.#
		#####v#.#.###v#.#.###.#
		#.....#...#...#.#.#...#
		#.#########.###.#.#.###
		#...###...#...#...#.###
		###.###.#.###v#####v###
		#...#...#.#.>.>.#.>.###
		#.###.###.#.###.#.#v###
		#.....###...###...#...#
		#####################.#
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day23SolutionPart1>(
		expected: "94", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day23SolutionPart2>(
		expected: "154", input: SampleInput);
}
