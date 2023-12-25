using NUnit.Framework;

namespace AoC.Year2018;

[TestFixture]
class Day03Test
{
	const string SampleInput =
		"""
		#1 @ 1,3: 4x4
		#2 @ 3,1: 4x4
		#3 @ 5,5: 2x2
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day03SolutionPart1>(
		expected: @"4", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day03SolutionPart2>(
		expected: @"3", input: SampleInput);
}
