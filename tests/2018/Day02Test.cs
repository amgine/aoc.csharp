using NUnit.Framework;

namespace AoC.Year2018;

[TestFixture]
class Day02Test
{
	const string SampleInput1 =
		"""
		abcdef
		bababc
		abbcde
		abcccd
		aabcdd
		abcdee
		ababab
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day02SolutionPart1>(
		expected: @"12", input: SampleInput1);

	const string SampleInput2 =
		"""
		abcde
		fghij
		klmno
		pqrst
		fguij
		axcye
		wvxyz
		""";

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day02SolutionPart2>(
		expected: @"fgij", input: SampleInput2);
}
