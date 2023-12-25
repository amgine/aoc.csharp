using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day12Test
{
	const string SampleInput =
		"""
		Sabqponm
		abcryxxl
		accszExk
		acctuvwj
		abdefghi
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day12SolutionPart1>(
		expected: "31", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: "1", input: SampleInput);
}
