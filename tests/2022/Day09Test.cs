using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day09Test
{
	const string SampleInput =
		"""
		R 4
		U 4
		L 3
		D 1
		R 4
		D 1
		L 5
		R 2
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day9SolutionPart1>(
		expected: "13", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day9SolutionPart2>(
		expected: "1", input: SampleInput);

	const string SampleInput2 =
		"""
		R 5
		U 8
		L 8
		D 3
		R 17
		D 10
		L 25
		U 20
		""";

	[Test]
	public void SolvePart2Sample2() => Helper.ValidateSolution<Day9SolutionPart2>(
		expected: "36", input: SampleInput2);
}
