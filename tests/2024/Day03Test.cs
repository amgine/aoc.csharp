using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day03Test
{
	const string SampleInput1 =
		"""
		xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
		""";

	const string SampleInput2 =
		"""
		xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day03SolutionPart1>(
		expected: "161", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day03SolutionPart2>(
		expected: "48", input: SampleInput2);
}
