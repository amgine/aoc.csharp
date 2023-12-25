using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day03Test
{
	[Test]
	[TestCase(">", ExpectedResult = "2")]
	[TestCase("^>v<", ExpectedResult = "4")]
	[TestCase("^v^v^v^v^v", ExpectedResult = "2")]
	public string SolvePart1(string input) => Helper.RunSolution<Day03SolutionPart1>(input);

	[Test]
	[TestCase("^v", ExpectedResult = "3")]
	[TestCase("^>v<", ExpectedResult = "3")]
	[TestCase("^v^v^v^v^v", ExpectedResult = "11")]
	public string SolvePart2(string input) => Helper.RunSolution<Day03SolutionPart2>(input);
}
