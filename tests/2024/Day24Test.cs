using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day24Test
{
	const string SampleInput1 =
		"""
		x00: 1
		x01: 1
		x02: 1
		y00: 0
		y01: 1
		y02: 0

		x00 AND y00 -> z00
		x01 XOR y01 -> z01
		x02 OR y02 -> z02
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day24SolutionPart1>(
		expected: "4", input: SampleInput1);
}
