using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day02Test
{
	[Test]
	[TestCase("2x3x4", ExpectedResult = "58")]
	[TestCase("1x1x10", ExpectedResult = "43")]
	public string SolvePart1(string input) => Helper.RunSolution<Day02SolutionPart1>(input);

	[Test]
	[TestCase("2x3x4", ExpectedResult = "34")]
	[TestCase("1x1x10", ExpectedResult = "14")]
	public string SolvePart2(string input) => Helper.RunSolution<Day02SolutionPart2>(input);
}
