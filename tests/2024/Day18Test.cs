using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day18Test
{
	const string SampleInput1 =
		"""
		5,4
		4,2
		4,5
		3,0
		2,1
		6,3
		2,4
		1,5
		0,6
		3,3
		2,6
		5,1
		1,2
		5,5
		2,5
		6,5
		1,4
		0,4
		6,4
		1,1
		6,1
		1,0
		0,5
		1,6
		2,0
		""";

	[Test]
	public void SolvePart1() => Assert.That(
		Day18SolutionPart1.Solve(new StringReader(SampleInput1), 7, 7, 12),
		Is.EqualTo(22));

	[Test]
	public void SolvePart2() => Assert.That(
		Day18SolutionPart2.Solve(new StringReader(SampleInput1), 7, 7, 12),
		Is.EqualTo(new Point2D(6, 1)));
}
