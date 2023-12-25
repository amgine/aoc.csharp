using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day16Test
{
	const string SampleInput =
		"""
		.|...\....
		|.-.\.....
		.....|-...
		........|.
		..........
		.........\
		..../.\\..
		.-.-/..|..
		.|....-|.\
		..//.|....
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day16SolutionPart1>(
		expected: "46", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day16SolutionPart2>(
		expected: "51", input: SampleInput);
}
