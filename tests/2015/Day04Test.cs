using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day04Test
{
	[Test]
	[TestCase(@"abcdef",  ExpectedResult = "609043")]
	[TestCase(@"pqrstuv", ExpectedResult = "1048970")]
	public string SolvePart1(string input) => Helper.RunSolution<Day04SolutionPart1>(input);

	[Test]
	[TestCase(@"ckczppom", ExpectedResult = "3938038")]
	public string SolvePart2(string input) => Helper.RunSolution<Day04SolutionPart2>(input);
}
