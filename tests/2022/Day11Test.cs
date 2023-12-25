using NUnit.Framework;

namespace AoC.Year2022;

[TestFixture]
class Day11Test
{
	const string SampleInput =
		"""
		Monkey 0:
		  Starting items: 79, 98
		  Operation: new = old * 19
		  Test: divisible by 23
		    If true: throw to monkey 2
		    If false: throw to monkey 3

		Monkey 1:
		  Starting items: 54, 65, 75, 74
		  Operation: new = old + 6
		  Test: divisible by 19
		    If true: throw to monkey 2
		    If false: throw to monkey 0

		Monkey 2:
		  Starting items: 79, 60, 97
		  Operation: new = old * old
		  Test: divisible by 13
		    If true: throw to monkey 1
		    If false: throw to monkey 3

		Monkey 3:
		  Starting items: 74
		  Operation: new = old + 3
		  Test: divisible by 17
		    If true: throw to monkey 0
		    If false: throw to monkey 1
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day11SolutionPart1>(
		expected: "10605", input: SampleInput);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day11SolutionPart2>(
		expected: "2713310158", input: SampleInput);
}
