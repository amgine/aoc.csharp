using NUnit.Framework;

namespace AoC.Year2018;

[TestFixture]
class Day01Test
{
	static string MakeInput(string line)
		=> string.Join(Environment.NewLine, line.Split(',', StringSplitOptions.TrimEntries));

	[Test]
	[TestCase("+1, +1, +1", ExpectedResult = "3")]
	[TestCase("+1, +1, -2", ExpectedResult = "0")]
	[TestCase("-1, -2, -3", ExpectedResult = "-6")]
	public string SolvePart1(string input) => Helper.RunSolution<Day01SolutionPart1>(MakeInput(input));

	[Test]
	[TestCase("+1, -1", ExpectedResult = "0")]
	[TestCase("+3, +3, +4, -2, -4", ExpectedResult = "10")]
	[TestCase("-6, +3, +8, +5, -6", ExpectedResult = "5")]
	[TestCase("+7, +7, -2, -7, -4", ExpectedResult = "14")]
	public string SolvePart2(string input) => Helper.RunSolution<Day01SolutionPart2>(MakeInput(input));
}
