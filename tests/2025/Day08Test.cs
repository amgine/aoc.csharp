using NUnit.Framework;

namespace AoC.Year2025;

[TestFixture]
class Day08Test
{
	const string SampleInput1 =
		"""
		162,817,812
		57,618,57
		906,360,560
		592,479,940
		352,342,300
		466,668,158
		542,29,236
		431,825,988
		739,650,466
		52,470,668
		216,146,977
		819,987,18
		117,168,530
		805,96,715
		346,949,466
		970,615,88
		941,993,340
		862,61,35
		984,92,344
		425,690,689
		""";

	[Test]
	public void SolvePart1()
		=> Assert.That(Day08SolutionPart1.Solve(new StringReader(SampleInput1), 10), Is.EqualTo(40));

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day08SolutionPart2>(
		expected: "25272", input: SampleInput1);
}
