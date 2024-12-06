using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day06Test
{
	const string SampleInput1 =
		"""
		....#.....
		.........#
		..........
		..#.......
		.......#..
		..........
		.#..^.....
		........#.
		#.........
		......#...
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day06SolutionPart1>(
		expected: "41", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day06SolutionPart2>(
		expected: "6", input: SampleInput1);
}
