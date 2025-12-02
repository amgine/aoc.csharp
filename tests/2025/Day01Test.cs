using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day01Test
{
	const string SampleInput1 =
		"""
		L68
		L30
		R48
		L5
		R60
		L55
		L1
		L99
		R14
		L82
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day01SolutionPart1>(
		expected: "3", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day01SolutionPart2>(
		expected: "6", input: SampleInput1);

	[Test]
	public void SolvePart2Case1() => Helper.ValidateSolution<Day01SolutionPart2>(
		expected: "10", input: "R1000");
}
