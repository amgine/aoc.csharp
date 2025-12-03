using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day03Test
{
	const string SampleInput1 =
		"""
		987654321111111
		811111111111119
		234234234234278
		818181911112111
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day03SolutionPart1>(
		expected: "357", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day03SolutionPart2>(
		expected: "3121910778619", input: SampleInput1);
}
