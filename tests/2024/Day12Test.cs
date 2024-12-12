using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day12Test
{
	const string SampleInput1 =
		"""
		RRRRIICCFF
		RRRRIICCCF
		VVRRRCCFFF
		VVRCCCJFFF
		VVVVCJJCFE
		VVIVCCJJEE
		VVIIICJJEE
		MIIIIIJJEE
		MIIISIJEEE
		MMMISSJEEE
		""";

	const string SampleInput2 =
		"""
		AAAAAA
		AAABBA
		AAABBA
		ABBAAA
		ABBAAA
		AAAAAA
		""";

	const string SampleInput3 =
		"""
		AAAA
		BBCD
		BBCC
		EEEC
		""";

	const string SampleInput4 =
		"""
		EEEEE
		EXXXX
		EEEEE
		EXXXX
		EEEEE
		""";

	const string SampleInput5 =
		"""
		OOOOO
		OXOXO
		OOOOO
		OXOXO
		OOOOO
		""";

	const string SampleInput6 =
		"""
		JJGGJG
		JJJJJJ
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day12SolutionPart1>(
		expected: "1930", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: "1206", input: SampleInput1);

	[Test]
	public void SolvePart2x() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: "368", input: SampleInput2);

	[Test]
	public void SolvePart2y() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: "80", input: SampleInput3);

	[Test]
	public void SolvePart2z() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: "236", input: SampleInput4);

	[Test]
	public void SolvePart2w() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: "436", input: SampleInput5);

	[Test]
	public void SolvePart2F() => Helper.ValidateSolution<Day12SolutionPart2>(
		expected: (8 + 4 + 90).ToString(), input: SampleInput6);
}
