using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
public class Day25Test
{
	const string SampleInput =
		"""
		1=-0-2
		12111
		2=0=
		21
		2=01
		111
		20012
		112
		1=-1=
		1-12
		12
		1=
		122
		""";

	[Test]
	[TestCase("1=-0-2", ExpectedResult = 1747L)]
	[TestCase ("12111", ExpectedResult =  906L)]
	[TestCase  ("2=0=", ExpectedResult =  198L)]
	[TestCase    ("21", ExpectedResult =   11L)]
	[TestCase  ("2=01", ExpectedResult =  201L)]
	[TestCase   ("111", ExpectedResult =   31L)]
	[TestCase ("20012", ExpectedResult = 1257L)]
	[TestCase   ("112", ExpectedResult =   32L)]
	[TestCase ("1=-1=", ExpectedResult =  353L)]
	[TestCase  ("1-12", ExpectedResult =  107L)]
	[TestCase    ("12", ExpectedResult =    7L)]
	[TestCase    ("1=", ExpectedResult =    3L)]
	[TestCase   ("122", ExpectedResult =   37L)]
	public long ConvertToDecimal(string value)
		=> SnafuConverter.Convert(value);

	[Test]
	[TestCase(        1L, ExpectedResult =             "1")]
	[TestCase(        2L, ExpectedResult =             "2")]
	[TestCase(        3L, ExpectedResult =            "1=")]
	[TestCase(        4L, ExpectedResult =            "1-")]
	[TestCase(        5L, ExpectedResult =            "10")]
	[TestCase(        6L, ExpectedResult =            "11")]
	[TestCase(        7L, ExpectedResult =            "12")]
	[TestCase(        8L, ExpectedResult =            "2=")]
	[TestCase(        9L, ExpectedResult =            "2-")]
	[TestCase(       10L, ExpectedResult =            "20")]
	[TestCase(       15L, ExpectedResult =           "1=0")]
	[TestCase(       20L, ExpectedResult =           "1-0")]
	[TestCase(     2022L, ExpectedResult =        "1=11-2")]
	[TestCase(    12345L, ExpectedResult =       "1-0---0")]
	[TestCase(314159265L, ExpectedResult = "1121-1110-1=0")]
	public string ConvertToSnafu(long value)
		=> SnafuConverter.Convert(value);

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day25SolutionPart1>(
		expected: "2=-1=0", input: SampleInput);

	/*
	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day1SolutionPart2>(
		expected: "281", input: SampleInput);
	*/
}
