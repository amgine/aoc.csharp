using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day01Test
{
	[Test]
	[TestCase("(())", ExpectedResult = "0")]
	[TestCase("()()", ExpectedResult = "0")]
	[TestCase("(((", ExpectedResult = "3")]
	[TestCase("(()(()(", ExpectedResult = "3")]
	[TestCase("())", ExpectedResult = "-1")]
	[TestCase("))(", ExpectedResult = "-1")]
	[TestCase(")())())", ExpectedResult = "-3")]
	[TestCase(")))", ExpectedResult = "-3")]
	public string SolvePart1(string input) => Helper.RunSolution<Day01SolutionPart1>(input);

	[Test]
	[TestCase(")", ExpectedResult = "1")]
	[TestCase("()())", ExpectedResult = "5")]
	public string SolvePart2(string input) => Helper.RunSolution<Day01SolutionPart2>(input);
}
