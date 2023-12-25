using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day03Test
{
	const string SampleInput =
		"""
		vJrwpWtwJgWrhcsFMMfFFhFp
		jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
		PmmdzqPrVvPwwTWBwg
		wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
		ttgJtRGJQctTZtZT
		CrZsJsPPZsGzwwsLwLmpwMDw
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day03SolutionPart1>(
		expected: "157", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day03SolutionPart2>(
		expected: "70", input: SampleInput);
}
