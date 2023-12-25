using NUnit.Framework;

namespace AoC.Year2017;

[TestFixture]
class Day03Test
{
	[Test]
	[TestCase("1", ExpectedResult = "0")]
	[TestCase("2", ExpectedResult = "1")]
	[TestCase("3", ExpectedResult = "2")]
	[TestCase("4", ExpectedResult = "1")]
	[TestCase("5", ExpectedResult = "2")]
	[TestCase("12", ExpectedResult = "3")]
	[TestCase("19", ExpectedResult = "2")]
	[TestCase("23", ExpectedResult = "2")]
	[TestCase("9", ExpectedResult = "2")]
	[TestCase("25", ExpectedResult = "4")]
	[TestCase("49", ExpectedResult = "6")]
	[TestCase("81", ExpectedResult = "8")]
	[TestCase("1024", ExpectedResult = "31")]
	public string SolvePart1(string input) => Helper.RunSolution<Day03SolutionPart1>(input);
}
