using System.Diagnostics;
using System.Globalization;

namespace AoC.Year2022;

[Name(@"Treetop Tree House")]
public abstract class Day8Solution : Solution
{
	protected struct Tree(int height)
	{
		public int Height { get; } = height;

		public bool IsVisible;
	}

	protected static Tree[,] Parse(TextReader reader)
	{
		var lines = new List<Tree[]>();
		{
			string? line;
			while((line = reader.ReadLine()) is not null)
			{
				var current = new Tree[line.Length];
				lines.Add(current);
				for(int i = 0; i < line.Length; ++i)
				{
					current[i] = new(height: line[i] - '0');
				}
			}
		}
		var forest = new Tree[lines.Count, lines[0].Length];
		for(int i = 0; i < lines.Count; i++)
		{
			var line = lines[i];
			for(int j = 0; j < line.Length; ++j)
			{
				forest[i, j] = line[j];
			}
		}
		return forest;
	}
}

public class Day8SolutionPart1 : Day8Solution
{
	record struct Coordinates(int X, int Y)
	{
		public static Coordinates operator +(Coordinates coordinates, Offset offset)
			=> new(coordinates.X + offset.X, coordinates.Y + offset.Y);
	}

	record struct Offset(int X, int Y);

	private static void Fill(Tree[,] forest, Coordinates initial, Offset nextRow, Offset nextColumn, int length1, int length2)
	{
		for(int i = 0; i < length1; ++i)
		{
			var max = forest[initial.X, initial.Y].Height;
			forest[initial.X, initial.Y].IsVisible = true;
			var cell = initial + nextColumn;
			for(int j = 1; j < length2; ++j)
			{
				if(forest[cell.X, cell.Y].Height > max)
				{
					forest[cell.X, cell.Y].IsVisible = true;
					max = forest[cell.X, cell.Y].Height;
				}
				cell += nextColumn;
			}
			initial += nextRow;
		}
	}

	private static void FillFromLeft(Tree[,] forest)
		=> Fill(forest, new(0, 0), new(1, 0), new(0, 1), forest.GetLength(0), forest.GetLength(1));

	private static void FillFromTop(Tree[,] forest)
		=> Fill(forest, new(0, 0), new(0, 1), new(1, 0), forest.GetLength(1), forest.GetLength(0));

	private static void FillFromRight(Tree[,] forest)
		=> Fill(forest, new(0, forest.GetLength(1) - 1), new(1, 0), new(0, -1), forest.GetLength(0), forest.GetLength(1));

	private static void FillFromBottom(Tree[,] forest)
		=> Fill(forest, new(forest.GetLength(0) - 1, 0), new(0, 1), new(-1, 0), forest.GetLength(1), forest.GetLength(0));

	private static int CountVisible(Tree[,] forest)
	{
		var count = 0;
		for(int i = 0; i < forest.GetLength(0); ++i)
		{
			for(int j = 0; j < forest.GetLength(1); ++j)
			{
				if(forest[i, j].IsVisible)
				{
					++count;
				}
			}
		}
		return count;
	}

	[Conditional("DEBUG")]
	private static void Debug(Tree[,] forest)
	{
		for(int i = 0; i < forest.GetLength(0); ++i)
		{
			for(int j = 0; j < forest.GetLength(1); ++j)
			{
				if(forest[i, j].IsVisible)
				{
					Console.Write('*');
				}
				else
				{
					Console.Write(' ');
				}
			}
			Console.WriteLine();
		}
	}

	public override string Process(TextReader reader)
	{
		var forest = Parse(reader);

		FillFromLeft(forest);
		FillFromTop(forest);
		FillFromRight(forest);
		FillFromBottom(forest);

		Debug(forest);

		return CountVisible(forest).ToString(CultureInfo.InvariantCulture);
	}
}

public class Day8SolutionPart2 : Day8Solution
{
	private static int GetScenicScore(Tree[,] forest, int x, int y)
	{
		var h = forest[x, y].Height;
		var score = 1;
		{
			var dist = 0;
			for(int i = x - 1; i >= 0; --i)
			{
				++dist;
				if(forest[i, y].Height >= h) break;
			}
			score *= dist;
		}
		{
			var dist = 0;
			for(int i = y - 1; i >= 0; --i)
			{
				++dist;
				if(forest[x, i].Height >= h) break;
			}
			score *= dist;
		}
		{
			var dist = 0;
			for(int i = x + 1; i < forest.GetLength(0); ++i)
			{
				++dist;
				if(forest[i, y].Height >= h) break;
			}
			score *= dist;
		}
		{
			var dist = 0;
			for(int i = y + 1; i < forest.GetLength(1); ++i)
			{
				++dist;
				if(forest[x, i].Height >= h) break;
			}
			score *= dist;
		}
		return score;
	}

	public override string Process(TextReader reader)
	{
		var forest = Parse(reader);

		var max = 0;
		for(int i = 1; i < forest.GetLength(0) - 1; ++i)
		{
			for(int j = 1; j < forest.GetLength(1) - 1; ++j)
			{
				var score = GetScenicScore(forest, i, j);
				if(score > max) max = score;
			}
		}

		return max.ToString(CultureInfo.InvariantCulture);
	}
}
