using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day25Test
{
	const string SampleInput =
		"""
		jqt: rhn xhk nvd
		rsh: frs pzl lsr
		xhk: hfx
		cmg: qnr nvd lhk bvb
		rhn: xhk bvb hfx
		bvb: xhk hfx
		pzl: lsr hfx nvd
		qnr: nvd
		ntq: jqt hfx bvb xhk
		nvd: lhk
		lsr: lhk
		rzs: qnr cmg lsr rsh
		frs: qnr lhk lsr
		""";

	[Test]
	public void SolvePart1() => Helper.ValidateSolution<Day25SolutionPart1>(
		expected: "54", input: SampleInput);
}
