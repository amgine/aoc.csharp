using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day06Test
{
	const string SampleInput1 =
		"""
		123 328  51 64 
		 45 64  387 23 
		  6 98  215 314
		*   +   *   +  
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day06SolutionPart1>(
		expected: "4277556", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day06SolutionPart2>(
		expected: "3263827", input: SampleInput1);
}
