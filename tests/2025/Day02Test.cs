using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day02Test
{
	const string SampleInput1 =
		"""
		11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day02SolutionPart1>(
		expected: "1227775554", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day02SolutionPart2>(
		expected: "4174379265", input: SampleInput1);
}
