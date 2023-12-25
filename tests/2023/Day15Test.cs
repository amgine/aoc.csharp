using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day15Test
{
	const string SampleInput =
		"""
		rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
		""";

	[Test]
	[TestCase(@"HASH", ExpectedResult = (byte) 52)]
	[TestCase(@"rn=1", ExpectedResult = (byte) 30)]
	[TestCase(@"cm-",  ExpectedResult = (byte)253)]
	[TestCase(@"qp=3", ExpectedResult = (byte) 97)]
	[TestCase(@"cm=2", ExpectedResult = (byte) 47)]
	[TestCase(@"qp-",  ExpectedResult = (byte) 14)]
	[TestCase(@"pc=4", ExpectedResult = (byte)180)]
	[TestCase(@"ot=9", ExpectedResult = (byte)  9)]
	[TestCase(@"ab=5", ExpectedResult = (byte)197)]
	[TestCase(@"pc-",  ExpectedResult = (byte) 48)]
	[TestCase(@"pc=6", ExpectedResult = (byte)214)]
	[TestCase(@"ot=7", ExpectedResult = (byte)231)]
	public byte Hash(string input) => Day15Solution.GetHash(input);

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day15SolutionPart1>(
		expected: "1320", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day15SolutionPart2>(
		expected: "145", input: SampleInput);
}
