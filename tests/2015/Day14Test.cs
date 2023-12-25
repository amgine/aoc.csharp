using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day14Test
{
	const string SampleInput =
		"""
		Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
		Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.
		""";

	[Test]
	[TestCase(1000, ExpectedResult = 1120)]
	public int GetMaxDistance(int time)
	{
		using var reader = new StringReader(SampleInput);
		return Day14SolutionPart1.GetMaxDistance(reader, time);
	}
}
