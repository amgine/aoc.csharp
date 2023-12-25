using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day11Test
{
	[Test]
	[TestCase(@"abcdefgh", ExpectedResult = @"abcdffaa")]
	[TestCase(@"ghijklmn", ExpectedResult = @"ghjaabcc")]
	public string SolvePart1(string input)
		=> Helper.RunSolution<Day11SolutionPart1>(input);

	/*
	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day11SolutionPart2>(
		expected: "4666278", input: "3113322113");
	*/
}
