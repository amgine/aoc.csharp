using NUnit.Framework;

namespace AoC.Year2021;

[TestFixture]
class Day01Test
{
	const string SampleInput1 =
		"""
		199
		200
		208
		210
		200
		207
		240
		269
		260
		263
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day01SolutionPart1>(
		expected: "7", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day01SolutionPart2>(
		expected: "5", input: SampleInput1);
}
