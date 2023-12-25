using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day22Test
{
	const string SampleInput =
		"""
		        ...#
		        .#..
		        #...
		        ....
		...#.......#
		........#...
		..#....#....
		..........#.
		        ...#....
		        .....#..
		        .#......
		        ......#.

		10R5L5R10L4R5L5
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day22SolutionPart1>(
		expected: "6032", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day22SolutionPart2>(
		expected: "5031", input: SampleInput);
}
