using NUnit.Framework;

namespace AoC.Year2016;

[TestFixture]
class Day01Test
{
	[Test]
	[TestCase("R2, L3", ExpectedResult = "5")]
	[TestCase("R2, R2, R2", ExpectedResult = "2")]
	[TestCase("R5, L5, R5, R3", ExpectedResult = "12")]
	public string SolvePart1(string input) => Helper.RunSolution<Day01SolutionPart1>(input);

	[Test]
	[TestCase("R8, R4, R4, R8", ExpectedResult = "4")]
	public string SolvePart2(string input) => Helper.RunSolution<Day01SolutionPart2>(input);
}
