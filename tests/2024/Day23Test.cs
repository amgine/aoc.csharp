using NUnit.Framework;

namespace AoC.Year2024;

[TestFixture]
class Day23Test
{
	const string SampleInput1 =
		"""
		kh-tc
		qp-kh
		de-cg
		ka-co
		yn-aq
		qp-ub
		cg-tb
		vc-aq
		tb-ka
		wh-tc
		yn-cg
		kh-ub
		ta-co
		de-co
		tc-td
		tb-wq
		wh-td
		ta-ka
		td-qp
		aq-cg
		wq-ub
		ub-vc
		de-ta
		wq-aq
		wq-vc
		wh-yn
		ka-de
		kh-ta
		co-tc
		wh-qp
		tb-vc
		td-yn
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day23SolutionPart1>(
		expected: "7", input: SampleInput1);

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day23SolutionPart2>(
		expected: "co,de,ka,ta", input: SampleInput1);
}
