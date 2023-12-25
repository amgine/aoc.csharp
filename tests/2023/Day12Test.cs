using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day12Test
{
	const string SampleInput =
		"""
		???.### 1,1,3
		.??..??...?##. 1,1,3
		?#?#?#?#?#?#?#? 1,3,1,6
		????.#...#... 4,1,1
		????.######..#####. 1,6,5
		?###???????? 3,2,1
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day12SolutionPart1>(
		expected: "21", input: SampleInput);

	[Test]
	[TestCase(@"???.### 1,1,3", ExpectedResult = @"1")]
	[TestCase(@".??..??...?##. 1,1,3", ExpectedResult = @"4")]
	[TestCase(@"???.???#?.#????#?.# 1,4,1,1,1,1", ExpectedResult = @"12")]
	[TestCase(@"??#??????##?.??#. 3,4,2", ExpectedResult = @"6")]
	[TestCase(@"?###???????? 3,2,1", ExpectedResult = "10")]
	[TestCase(@"?#??#..#?#???#??#. 5,1,1,4", ExpectedResult = "1")]
	[TestCase(@"??#?##???##?????#? 1,9,1,1", ExpectedResult = "4")]
	[TestCase(@".?...??#??? 1,1", ExpectedResult = "4")]
	public string SolvePart1x(string input) => Helper.RunSolution<Day12SolutionPart1>(input);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: "525152", input: SampleInput);

	[Test]
	[TestCase(@"???.### 1,1,3", ExpectedResult = @"1")]
	[TestCase(@".??..??...?##. 1,1,3", ExpectedResult = @"16384")]
	[TestCase(@"?#?#?#?#?#?#?#? 1,3,1,6", ExpectedResult = @"1")]
	[TestCase(@"????.#...#... 4,1,1", ExpectedResult = @"16")]
	[TestCase(@"????.######..#####. 1,6,5", ExpectedResult = @"2500")]
	[TestCase(@"?###???????? 3,2,1", ExpectedResult = @"506250")]
	public string SolvePart2x(string input) => Helper.RunSolution<Day12SolutionPart2>(input);
}
