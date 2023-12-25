using NUnit.Framework;

namespace AoC;

static class Helper
{
	public static void ValidateSolution<T>(string expected, string input)
		where T : Solution, new()
	{
		string output;
		try
		{
			output = RunSolution<T>(input);
		}
		catch(NotImplementedException)
		{
			Assert.Inconclusive("Solution is not implemented yet.");
			return;
		}
		Assert.That(output, Is.EqualTo(expected));
	}

	public static string RunSolution<T>(string input)
		where T : Solution, new()
	{
		var solution = new T();
		using var reader = new StringReader(input);
		return solution.Process(reader);
	}
}
