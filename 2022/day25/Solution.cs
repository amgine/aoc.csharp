namespace AoC.Year2022;

[Name(@"Full of Hot Air")]
public abstract class Day25Solution : Solution
{
}

public sealed class Day25SolutionPart1 : Day25Solution
{
	public override string Process(TextReader reader)
		=> SnafuConverter.Convert(SumFromNonEmptyLines(reader, SnafuConverter.Convert));
}
