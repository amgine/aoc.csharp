using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day01Test
{
	const string SampleInput1 =
		"""
		1abc2
		pqr3stu8vwx
		a1b2c3d4e5f
		treb7uchet
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day01SolutionPart1>(
		expected: "142", input: SampleInput1);

	const string SampleInput2 =
		"""
		two1nine
		eightwothree
		abcone2threexyz
		xtwone3four
		4nineeightseven2
		zoneight234
		7pqrstsixteen
		""";

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day01SolutionPart2>(
		expected: "281", input: SampleInput2);
}
