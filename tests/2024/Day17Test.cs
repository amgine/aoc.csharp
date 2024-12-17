using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day17Test
{
	const string SampleInput1 =
		"""
		Register A: 729
		Register B: 0
		Register C: 0

		Program: 0,1,5,4,3,0
		""";

	const string SampleInput2 =
		"""
		Register A: 2024
		Register B: 0
		Register C: 0

		Program: 0,3,5,4,3,0
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day17SolutionPart1>(
		expected: "4,6,3,5,6,3,5,2,1,0", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day17SolutionPart2>(
		expected: "117440", input: SampleInput2);
}
