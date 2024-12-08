﻿using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day08Test
{
	const string SampleInput1 =
		"""
		............
		........0...
		.....0......
		.......0....
		....0.......
		......A.....
		............
		............
		........A...
		.........A..
		............
		............
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day08SolutionPart1>(
		expected: "14", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day08SolutionPart2>(
		expected: "34", input: SampleInput1);
}
