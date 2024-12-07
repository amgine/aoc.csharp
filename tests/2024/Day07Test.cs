using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day07Test
{
	const string SampleInput1 =
		"""
		190: 10 19
		3267: 81 40 27
		83: 17 5
		156: 15 6
		7290: 6 8 6 15
		161011: 16 10 13
		192: 17 8 14
		21037: 9 7 18 13
		292: 11 6 16 20
		""";

	[Test]
	[TestCase(10, 0, ExpectedResult = 100)]
	[TestCase(10, 10, ExpectedResult = 1010)]
	[TestCase(12, 54, ExpectedResult = 1254)]
	[TestCase(0, 1, ExpectedResult = 1)]
	public long Concat(long a, long b) => Day07Solution.Concat(a, b);

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day07SolutionPart1>(
		expected: "3749", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day07SolutionPart2>(
		expected: "11387", input: SampleInput1);
}
