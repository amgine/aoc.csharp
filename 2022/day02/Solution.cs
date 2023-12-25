namespace AoC.Year2022;

[Name(@"Rock Paper Scissors")]
public abstract class Day02Solution : Solution
{
	protected enum Choice
	{
		Rock     = 1,
		Paper    = 2,
		Scissors = 3,
	}

	protected static Choice Draw(Choice otherPlayerChoice)
		=> otherPlayerChoice;

	protected static Choice Win(Choice otherPlayerChoice)
		=> otherPlayerChoice switch
		{
			Choice.Rock     => Choice.Paper,
			Choice.Paper    => Choice.Scissors,
			Choice.Scissors => Choice.Rock,
			_ => throw new ArgumentException($"Invalid choice: {otherPlayerChoice}", nameof(otherPlayerChoice)),
		};

	protected static Choice Lose(Choice otherPlayerChoice)
		=> otherPlayerChoice switch
		{
			Choice.Rock     => Choice.Scissors,
			Choice.Paper    => Choice.Rock,
			Choice.Scissors => Choice.Paper,
			_ => throw new ArgumentException($"Invalid choice: {otherPlayerChoice}", nameof(otherPlayerChoice)),
		};

	protected static int GetScore(Choice a, Choice b)
	{
		if(b == Lose(a)) return (int)b + 0;
		if(b == Draw(a)) return (int)b + 3;
		if(b == Win (a)) return (int)b + 6;
		throw new ArgumentException($"Invalid choices: {a}, {b}");
	}

	protected static Choice ParseFirstPlayerChoice(char c)
		=> (Choice)(c - 'A' + 1);

	protected abstract Choice ParseSecondPlayerChoice(Choice firstPlayerChoice, char c);

	private int GetScore(string line)
	{
		if(line.Length != 3) throw new InvalidDataException();

		var a = ParseFirstPlayerChoice (   line[0]);
		var b = ParseSecondPlayerChoice(a, line[2]);

		return GetScore(a, b);
	}

	public override string Process(TextReader reader)
	{
		var sum = 0;
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) continue;
			sum += GetScore(line);
		}
		return sum.ToString();
	}
}

public class Day02SolutionPart1 : Day02Solution
{
	protected override Choice ParseSecondPlayerChoice(Choice firstPlayerChoice, char c)
		=> (Choice)(c - 'X' + 1);
}

public class Day02SolutionPart2 : Day02Solution
{
	static readonly Func<Choice, Choice>[] Variants = [Lose, Draw, Win];

	protected override Choice ParseSecondPlayerChoice(Choice otherPlayerChoice, char c)
		=> Variants[c - 'X'](otherPlayerChoice);
}
