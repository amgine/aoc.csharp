using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day10Test
{
	const string SampleInput1 =
		"""
		..F7.
		.FJ|.
		SJ.L7
		|F--J
		LJ...
		""";

	const string SampleInput2 =
		"""
		-L|F7
		7S-7|
		L|7||
		-L-J|
		L|-JF
		""";

	const string SampleInput3 =
		"""
		FF7FSF7F7F7F7F7F---7
		L|LJ||||||||||||F--J
		FL-7LJLJ||||||LJL-77
		F--JF--7||LJLJ7F7FJ-
		L---JF-JLJ.||-FJLJJ7
		|F|F-JF---7F7-L7L|7|
		|FFJF7L7F-JF7|JL---7
		7-L-JL7||F7|L7F-7F7|
		L.L7LFJ|||||FJL7||LJ
		L7JLJL-JLJLJL--JLJ.L
		""";

	const string SampleInput4 =
		"""
		.F----7F7F7F7F-7....
		.|F--7||||||||FJ....
		.||.FJ||||||||L7....
		FJL7L7LJLJ||LJ.L-7..
		L--J.L7...LJS7F-7L7.
		....F-J..F7FJ|L7L7L7
		....L7.F7||L7|.L7L7|
		.....|FJLJ|FJ|F7|.LJ
		....FJL-7.||.||||...
		....L---J.LJ.LJLJ...
		""";

	const string SampleInput5 =
		"""
		...........
		.S-------7.
		.|F-----7|.
		.||.....||.
		.||.....||.
		.|L-7.F-J|.
		.|..|.|..|.
		.L--J.L--J.
		...........
		""";

	const string SampleInput6 =
		"""
		..........
		.S------7.
		.|F----7|.
		.||OOOO||.
		.||OOOO||.
		.|L-7F-J|.
		.|II||II|.
		.L--JL--J.
		..........
		""";

	const string SampleInput7 =
		"""
		S7F-7
		||L7|
		||.||
		|L-J|
		|F-7|
		||.||
		||FJ|
		LJL-J
		""";

	[Test]
	public void SolvePart1Sample1() => Helper.ValidateSolution<Day10SolutionPart1>(
		expected: "8", input: SampleInput1);

	[Test]
	public void SolvePart1Sample0() => Helper.ValidateSolution<Day10SolutionPart1>(
		expected: "4", input: SampleInput2);

	[Test]
	public void SolvePart2Sample0() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "10", input: SampleInput3);

	[Test]
	public void SolvePart2Sample1() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "8", input: SampleInput4);

	[Test]
	public void SolvePart2Sample2() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "4", input: SampleInput5);

	[Test]
	public void SolvePart2Sample3() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "4", input: SampleInput6);

	[Test]
	public void SolvePart2Sample4() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "0", input: SampleInput7);
}
