using NUnit.Framework;

namespace AoC.Year2023;

[TestFixture]
class Day24Test
{
	const string SampleInput =
		"""
		19, 13, 30 @ -2,  1, -2
		18, 19, 22 @ -1, -1, -2
		20, 25, 34 @ -2, -2, -4
		12, 31, 28 @ -1, -2, -1
		20, 19, 15 @  1, -5, -3
		""";

	[Test]
	public void Intersects1()
	{
		var d1 = "19, 13, 30 @ -2, 1, -2";
		var d2 = "18, 19, 22 @ -1, -1, -2";
		var s1 = Day24Solution.ParseHailstone(d1);
		var s2 = Day24Solution.ParseHailstone(d2);

		Assert.That(Day24Solution.ProjectionsIntersect2D(s1, s2, out var p), Is.True);

		Console.WriteLine($"X = {p.X}, Y = {p.Y}");
	}

	[Test]
	public void Intersects2()
	{
		var d1 = "19, 13, 30 @ -2, 1, -2";
		var d2 = "20, 25, 34 @ -2, -2, -4";
		var s1 = Day24Solution.ParseHailstone(d1);
		var s2 = Day24Solution.ParseHailstone(d2);

		Assert.That(Day24Solution.ProjectionsIntersect2D(s1, s2, out var p), Is.True);

		Console.WriteLine($"X = {p.X}, Y = {p.Y}");
	}

	[Test]
	public void Intersects3()
	{
		var d1 = "20, 25, 34 @ -2, -2, -4";
		var d2 = "12, 31, 28 @ -1, -2, -1";
		var s1 = Day24Solution.ParseHailstone(d1);
		var s2 = Day24Solution.ParseHailstone(d2);

		Assert.That(Day24Solution.ProjectionsIntersect2D(s1, s2, out var p), Is.True);

		Console.WriteLine($"X = {p.X}, Y = {p.Y}");
	}

	[Test]
	[TestCase("19, 13, 30 @ -2, 1, -2", "20, 19, 15 @ 1, -5, -3")]
	[TestCase("18, 19, 22 @ -1, -1, -2", "20, 19, 15 @ 1, -5, -3")]
	[TestCase("20, 25, 34 @ -2, -2, -4", "20, 19, 15 @ 1, -5, -3")]
	[TestCase("12, 31, 28 @ -1, -2, -1", "20, 19, 15 @ 1, -5, -3")]
	public void IntersectInPast(string d1, string d2)
	{
		var s1 = Day24Solution.ParseHailstone(d1);
		var s2 = Day24Solution.ParseHailstone(d2);

		Assert.That(Day24Solution.ProjectionsIntersect2D(s1, s2, out _), Is.False);
	}

	[Test]
	[TestCase("18, 19, 22 @ -1, -1, -2", "20, 25, 34 @ -2, -2, -4")]
	public void IntersectsParallel(string d1, string d2)
	{
		var s1 = Day24Solution.ParseHailstone(d1);
		var s2 = Day24Solution.ParseHailstone(d2);

		Assert.That(Day24Solution.ProjectionsIntersect2D(s1, s2, out _), Is.False);
	}

	[Test]
	public void SolvePart1()
	{
		using var reader = new StringReader(SampleInput);
		var res = Day24Solution.CountIntersectionsInArea(reader, 7, 27);
		Assert.That(res, Is.EqualTo(2));
	}

	[Test]
	public void SolvePart2() => Helper.ValidateSolution<Day24SolutionPart2>(
		expected: "47", input: SampleInput);
}
