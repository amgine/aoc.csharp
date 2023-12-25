using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day08Test
{
	[Test]
	[TestCase("\"\"", ExpectedResult = "2")]
	[TestCase("\"abc\"", ExpectedResult = "2")]
	[TestCase("\"aaa\\\"aaa\"", ExpectedResult = "3")]
	[TestCase("\"\\x27\"", ExpectedResult = "5")]
	public string SolvePart1(string input) => Helper.RunSolution<Day08SolutionPart1>(input);

	[Test]
	[TestCase("\"\"", ExpectedResult = "4")]
	[TestCase("\"abc\"", ExpectedResult = "4")]
	[TestCase("\"aaa\\\"aaa\"", ExpectedResult = "6")]
	[TestCase("\"\\x27\"", ExpectedResult = "5")]
	public string SolvePart2(string input) => Helper.RunSolution<Day08SolutionPart2>(input);
}
