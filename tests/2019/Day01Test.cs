using NUnit.Framework;

namespace AoC.Year2019;

[TestFixture]
class Day01Test
{
	[Test]
	[TestCase("12", ExpectedResult = "2")]
	[TestCase("14", ExpectedResult = "2")]
	[TestCase("1969", ExpectedResult = "654")]
	[TestCase("100756", ExpectedResult = "33583")]
	public string SolvePart1(string input) => Helper.RunSolution<Day01SolutionPart1>(input);

	[Test]
	[TestCase("14", ExpectedResult = "2")]
	[TestCase("1969", ExpectedResult = "966")]
	[TestCase("100756", ExpectedResult = "50346")]
	public string SolvePart2(string input) => Helper.RunSolution<Day01SolutionPart2>(input);
}
