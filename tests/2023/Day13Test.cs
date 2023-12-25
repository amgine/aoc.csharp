using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day13Test
{
	const string SampleInput =
		"""
		#.##..##.
		..#.##.#.
		##......#
		##......#
		..#.##.#.
		..##..##.
		#.#.##.#.

		#...##..#
		#....#..#
		..##..###
		#####.##.
		#####.##.
		..##..###
		#....#..#
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day13SolutionPart1>(
		expected: "405", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day13SolutionPart2>(
		expected: "400", input: SampleInput);
}
