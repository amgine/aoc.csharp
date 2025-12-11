using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day11Test
{
	const string SampleInput1 =
		"""
		aaa: you hhh
		you: bbb ccc
		bbb: ddd eee
		ccc: ddd eee fff
		ddd: ggg
		eee: out
		fff: out
		ggg: out
		hhh: ccc fff iii
		iii: out
		""";

	const string SampleInput2 =
		"""
		svr: aaa bbb
		aaa: fft
		fft: ccc
		bbb: tty
		tty: ccc
		ccc: ddd eee
		ddd: hub
		hub: fff
		eee: dac
		dac: fff
		fff: ggg hhh
		ggg: out
		hhh: out
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day11SolutionPart1>(
		expected: "5", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day11SolutionPart2>(
		expected: "2", input: SampleInput2);
}
