namespace AoC.Year2021;

[Name(@"Giant Squid")]
public abstract class Day04Solution : Solution
{
	protected const int BoardSize = 5;

	protected readonly struct Input(int[] sequence, List<Cell[,]> boards)
	{
		public readonly int[] Sequence = sequence;
		public readonly List<Cell[,]> Boards = boards;
	}

	protected struct Cell(int value)
	{
		public readonly int Value = value;
		public bool Marked = false;
	}

	static bool TryParseBoard(TextReader reader, out Cell[,] board)
	{
		board = new Cell[BoardSize, BoardSize];
		Span<Range> ranges = stackalloc Range[BoardSize];
		for(int i = 0; i < BoardSize; ++i)
		{
			var line = reader.ReadLine();
			if(line is null) return false;

			var c = line.AsSpan().Split(ranges, ' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			if(c != BoardSize) throw new InvalidDataException();

			for(int j = 0; j < BoardSize; ++j)
			{
				board[i, j] = new Cell(int.Parse(line.AsSpan(ranges[j])));
			}
		}
		reader.ReadLine();
		return true;
	}

	protected static Input ParseInput(TextReader reader)
	{
		var seq = reader.ReadLine() ?? throw new InvalidDataException();

		var sequence = Array.ConvertAll(seq.Split(','), int.Parse);

		reader.ReadLine();

		var boards = new List<Cell[,]>();
		while(TryParseBoard(reader, out var board))
		{
			boards.Add(board);
		}

		return new(sequence, boards);
	}

	protected static bool Mark(Cell[,] board, int value)
	{
		for(int i = 0; i < BoardSize; ++i)
		{
			for(int j = 0; j < BoardSize; ++j)
			{
				if(board[i, j].Value == value)
				{
					board[i, j].Marked = true;
					return true;
				}
			}
		}
		return false;
	}

	protected static bool IsWinning(Cell[,] board)
	{
		static bool CheckRow(Cell[,] board, int i)
		{
			for(int j = 0; j < BoardSize; ++j)
			{
				if(!board[i, j].Marked) return false;
			}
			return true;
		}

		static bool CheckColumn(Cell[,] board, int j)
		{
			for(int i = 0; i < BoardSize; ++i)
			{
				if(!board[i, j].Marked) return false;
			}
			return true;
		}

		for(int i = 0; i < BoardSize; ++i)
		{
			if(CheckRow   (board, i)) return true;
			if(CheckColumn(board, i)) return true;
		}
		return false;
	}

	protected static int GetScore(Cell[,] board)
	{
		var sum = 0;
		foreach(var cell in board)
		{
			if(!cell.Marked) sum += cell.Value;
		}
		return sum;
	}
}

public sealed class Day04SolutionPart1 : Day04Solution
{
	public override string Process(TextReader reader)
	{
		var input = ParseInput(reader);

		for(int i = 0; i < input.Sequence.Length; ++i)
		{
			var n = input.Sequence[i];
			foreach(var board in input.Boards)
			{
				if(Mark(board, n) && IsWinning(board))
				{
					return (GetScore(board) * n).ToString();
				}
			}
		}

		throw new InvalidDataException();
	}
}

public sealed class Day04SolutionPart2 : Day04Solution
{
	public override string Process(TextReader reader)
	{
		var input = ParseInput(reader);

		var lastScore = -1;

		for(int i = 0; i < input.Sequence.Length; ++i)
		{
			var n = input.Sequence[i];
			for(int j = input.Boards.Count - 1; j >= 0; --j)
			{
				var board = input.Boards[j];
				if(Mark(board, n) && IsWinning(board))
				{
					lastScore = GetScore(board) * n;
					input.Boards.RemoveAt(j);
				}
			}
			if(input.Boards.Count == 0) break;
		}

		if(lastScore == -1) throw new InvalidDataException();

		return lastScore.ToString();
	}
}
