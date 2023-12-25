using NUnit.Framework;

namespace AoC.Year2015;

[TestFixture]
class Day10Test
{
	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day10SolutionPart1>(
		expected: "329356", input: "3113322113");

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day10SolutionPart2>(
		expected: "4666278", input: "3113322113");
}
