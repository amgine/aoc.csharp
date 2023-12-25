using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day17Test
{
	const string SampleInput =
		"""
		2413432311323
		3215453535623
		3255245654254
		3446585845452
		4546657867536
		1438598798454
		4457876987766
		3637877979653
		4654967986887
		4564679986453
		1224686865563
		2546548887735
		4322674655533
		""";

	const string SampleInput2 =
		"""
		111111111111
		999999999991
		999999999991
		999999999991
		999999999991
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day17SolutionPart1>(
		expected: "102", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day17SolutionPart2>(
		expected: "94", input: SampleInput);

	[Test]
	public void SolvePart2Sample2() => Helper.ValidateSolution<Day17SolutionPart2>(
		expected: "71", input: SampleInput2);
}
