using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day11Test
{
	const string SampleInput1 =
		"""
		125 17
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day11SolutionPart1>(
		expected: "55312", input: SampleInput1);
}
