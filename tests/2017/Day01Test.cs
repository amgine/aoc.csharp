using NUnit.Framework;

namespace AoC.Year2017;

[TestFixture]
class Day01Test
{
	[Test]
	[TestCase("1122", ExpectedResult = "3")]
	[TestCase("1111", ExpectedResult = "4")]
	[TestCase("1234", ExpectedResult = "0")]
	[TestCase("91212129", ExpectedResult = "9")]
	public string SolvePart1(string input) => Helper.RunSolution<Day01SolutionPart1>(input);

	[Test]
	[TestCase("1212", ExpectedResult = "6")]
	[TestCase("1221", ExpectedResult = "0")]
	[TestCase("123425", ExpectedResult = "4")]
	[TestCase("123123", ExpectedResult = "12")]
	[TestCase("12131415", ExpectedResult = "4")]
	public string SolvePart2(string input) => Helper.RunSolution<Day01SolutionPart2>(input);
}
