using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day22Test
{
	const string SampleInput =
		"""
		1,0,1~1,2,1
		0,0,2~2,0,2
		0,2,3~2,2,3
		0,0,4~0,2,4
		2,0,5~2,2,5
		0,1,6~2,1,6
		1,1,8~1,1,9
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day22SolutionPart1>(
		expected: "5", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day22SolutionPart2>(
		expected: "7", input: SampleInput);
}
