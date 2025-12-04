using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day04Test
{
	const string SampleInput1 =
		"""
		..@@.@@@@.
		@@@.@.@.@@
		@@@@@.@.@@
		@.@@@@..@.
		@@.@@@@.@@
		.@@@@@@@.@
		.@.@.@.@@@
		@.@@@.@@@@
		.@@@@@@@@.
		@.@.@@@.@.
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day04SolutionPart1>(
		expected: "13", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day04SolutionPart2>(
		expected: "43", input: SampleInput1);
}
