using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day10Test
{
	const string SampleInput1 =
		"""
		[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
		[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
		[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day10SolutionPart1>(
		expected: "7", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "33", input: SampleInput1);
}
