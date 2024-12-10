using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day10Test
{
	const string SampleInput1 =
		"""
		89010123
		78121874
		87430965
		96549874
		45678903
		32019012
		01329801
		10456732
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day10SolutionPart1>(
		expected: "36", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "81", input: SampleInput1);
}
