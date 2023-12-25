using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day20Test
{
	const string SampleInput1 =
		"""
		broadcaster -> a, b, c
		%a -> b
		%b -> c
		%c -> inv
		&inv -> a
		""";

	const string SampleInput2 =
		"""
		broadcaster -> a
		%a -> inv, con
		&inv -> b
		%b -> con
		&con -> output
		""";

	[Test]
	public void SolvePart1Sample1() => Helper.ValidateSolution<Day20SolutionPart1>(
		expected: "32000000", input: SampleInput1);

	[Test]
	public void SolvePart1Sample2() => Helper.ValidateSolution<Day20SolutionPart1>(
		expected: "11687500", input: SampleInput2);
}
