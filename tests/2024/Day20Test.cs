using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day20Test
{
	const string SampleInput1 =
		"""
		###############
		#...#...#.....#
		#.#.#.#.#.###.#
		#S#...#.#.#...#
		#######.#.#.###
		#######.#.#...#
		#######.#.###.#
		###..E#...#...#
		###.#######.###
		#...###...#...#
		#.#####.#.###.#
		#.#...#.#.#...#
		#.#.#.#.#.#.###
		#...#...#...###
		###############
		""";

	[Test]
	public void SolvePart1() => Assert.That(
		Day20Solution.CountCheats(new StringReader(SampleInput1), 2, 1),
		Is.EqualTo(44));

	[Test]
	public void SolvePart2() => Assert.That(
		Day20Solution.CountCheats(new StringReader(SampleInput1), 20, 50),
		Is.EqualTo(285));
}
