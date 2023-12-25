using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day05Test
{
	[Test]
	[TestCase(@"ugknbfddgicrmopn", ExpectedResult = "1")]
	[TestCase(@"aaa", ExpectedResult = "1")]
	[TestCase(@"jchzalrnumimnmhp", ExpectedResult = "0")]
	[TestCase(@"haegwjzuvuyypxyu", ExpectedResult = "0")]
	[TestCase(@"dvszwmarrgswjxmb", ExpectedResult = "0")]
	public string SolvePart1(string input) => Helper.RunSolution<Day05SolutionPart1>(input);

	[Test]
	[TestCase(@"qjhvhtzxzqqjkmpb", ExpectedResult = "1")]
	[TestCase(@"aaa", ExpectedResult = "0")]
	[TestCase(@"xxyxx", ExpectedResult = "1")]
	[TestCase(@"uurcxstgmygtbstg", ExpectedResult = "0")]
	[TestCase(@"ieodomkazucvgmuy", ExpectedResult = "0")]
	public string SolvePart2(string input) => Helper.RunSolution<Day05SolutionPart2>(input);
}
