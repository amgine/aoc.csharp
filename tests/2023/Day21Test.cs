using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day21Test
{
	const string SampleInput =
		"""
		...........
		.....###.#.
		.###.##..#.
		..#.#...#..
		....#.#....
		.##..S####.
		.##..#...#.
		.......##..
		.##.#.####.
		.##..##.##.
		...........
		""";

	[Test]
	public void SolvePart1()
	{
		using var reader = new StringReader(SampleInput);
		Assert.That(Day21SolutionPart1.Process(reader, 6), Is.EqualTo(16));
	}
}
