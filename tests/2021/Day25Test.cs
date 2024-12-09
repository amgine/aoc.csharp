using NUnit.Framework;

namespace AoC.Year2021;

[TestFixture]
class Day25Test
{
	const string SampleInput1 =
		"""
		v...>>.vv>
		.vv>>.vv..
		>>.>v>...v
		>>v>>.>.v.
		v>v.vv.v..
		>.>>..v...
		.vv..>.>v.
		v.v..>>v.v
		....v..v.>
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day25SolutionPart1>(
		expected: "58", input: SampleInput1);
}
