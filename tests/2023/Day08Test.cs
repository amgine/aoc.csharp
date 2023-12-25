using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day08Test
{
	const string SampleInput1 =
		"""
		RL

		AAA = (BBB, CCC)
		BBB = (DDD, EEE)
		CCC = (ZZZ, GGG)
		DDD = (DDD, DDD)
		EEE = (EEE, EEE)
		GGG = (GGG, GGG)
		ZZZ = (ZZZ, ZZZ)
		""";

	const string SampleInput2 =
		"""
		LLR

		AAA = (BBB, BBB)
		BBB = (AAA, ZZZ)
		ZZZ = (ZZZ, ZZZ)
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day8SolutionPart1>(
		expected: "2", input: SampleInput1);

	[Test]
	public void SolvePart1Sample2() => Helper.ValidateSolution<Day8SolutionPart1>(
		expected: "6", input: SampleInput2);

	const string SampleInput3 =
		"""
		LR

		11A = (11B, XXX)
		11B = (XXX, 11Z)
		11Z = (11B, XXX)
		22A = (22B, XXX)
		22B = (22C, 22C)
		22C = (22Z, 22Z)
		22Z = (22B, 22B)
		XXX = (XXX, XXX)
		""";

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day8SolutionPart2>(
		expected: "6", input: SampleInput3);
}
