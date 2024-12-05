using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day05Test
{
	const string SampleInput1 =
		"""
		47|53
		97|13
		97|61
		97|47
		75|29
		61|13
		75|53
		29|13
		97|29
		53|29
		61|53
		97|53
		61|29
		47|13
		75|47
		97|75
		47|61
		75|61
		47|29
		75|13
		53|13

		75,47,61,53,29
		97,61,53,29,13
		75,29,13
		75,97,47,61,53
		61,13,29
		97,13,75,29,47
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day05SolutionPart1>(
		expected: "143", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day05SolutionPart2>(
		expected: "123", input: SampleInput1);
}
