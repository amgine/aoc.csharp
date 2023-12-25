using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day18Test
{
	const string SampleInput =
		"""
		R 6 (#70c710)
		D 5 (#0dc571)
		L 2 (#5713f0)
		D 2 (#d2c081)
		R 2 (#59c680)
		D 2 (#411b91)
		L 5 (#8ceee2)
		U 2 (#caa173)
		L 1 (#1b58a2)
		U 2 (#caa171)
		R 2 (#7807d2)
		U 3 (#a77fa3)
		L 2 (#015232)
		U 2 (#7a21e3)
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day18SolutionPart1>(
		expected: "62", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day18SolutionPart2>(
		expected: "952408144115", input: SampleInput);
}
