using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
public class Day18Test
{
	const string SampleInput =
		"""
		2,2,2
		1,2,2
		3,2,2
		2,1,2
		2,3,2
		2,2,1
		2,2,3
		2,2,4
		2,2,6
		1,2,5
		3,2,5
		2,1,5
		2,3,5
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day18SolutionPart1>(
		expected: "64", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day18SolutionPart2>(
		expected: "58", input: SampleInput);
}
