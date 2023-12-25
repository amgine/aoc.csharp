
namespace AoC.Year2017;

/// <remarks><a href="https://adventofcode.com/2017/day/3"/></remarks>
[Name(@"Spiral Memory")]
public abstract class Day03Solution : Solution
{
}

public sealed class Day03SolutionPart1 : Day03Solution
{
	static int GetDistance(int n)
	{
		var q = (int)Math.Ceiling(Math.Sqrt(n)) / 2;
		var s = 1 + (q * 2);
		var c0 = s * s;
		var c1 = c0 - s + 1;
		var c2 = c1 - s + 1;
		var c3 = c2 - s + 1;

		if(n == c0 || n == c1 || n == c2 || n == c3)
		{
			return s & ~1;
		}

		int d;

		     if(n < c3) { d = c3 - n; }
		else if(n < c2) { d = c2 - n; }
		else if(n < c1) { d = c1 - n; }
		else            { d = c0 - n; }

		return s / 2 + Math.Abs(s / 2 - d);
	}

	public override string Process(TextReader reader)
	{
		var input = int.Parse(reader.ReadLine() ?? throw new InvalidDataException());
		return GetDistance(input).ToString();
	}
}

public sealed class Day03SolutionPart2 : Day03Solution
{
	public override string Process(TextReader reader)
	{
		var input = int.Parse(reader.ReadLine() ?? throw new InvalidDataException());

		var s = 1;
		var prev = new int[] { 1 };
		while(true)
		{
			s += 2;
			var next = new int[((s - 1) * 4)];

			var p_index = 0;
			var n_index = 0;
			for(int i = 0; i < s - 2; ++i)
			{
				if(n_index == 0)
				{
					next[n_index] = prev.Length == 1
						? prev[0]
						: prev[0] + prev[^1];
				}
				else
				{
					next[n_index] = next[n_index - 1] + prev[p_index];
					if(prev.Length > 1)
					{
						next[n_index] += prev[(prev.Length + p_index - 1) % prev.Length];
						if(i < s - 3)
						{
							next[n_index] += prev[p_index + 1];
							++p_index;
						}
					}
				}
				if(next[n_index] > input)
				{
					return next[n_index].ToString();
				}
				++n_index;
			}

			for(int j = 0; j < 3; ++j)
			{
				next[n_index] = next[n_index - 1] + prev[p_index];
				if(next[n_index] > input)
				{
					return next[n_index].ToString();
				}
				++n_index;

				for(int i = 0; i < s - 2; ++i)
				{
					if(i == 0)
					{
						next[n_index] = next[n_index - 1] + next[n_index - 2] + prev[p_index];
						if(prev.Length > 1)
						{
							next[n_index] += prev[p_index + 1];
							++p_index;
						}
					}
					else
					{
						next[n_index] = next[n_index - 1] + prev[p_index];
						if(prev.Length > 1)
						{
							next[n_index] += prev[p_index - 1];
							if(i < s - 3)
							{
								next[n_index] += prev[p_index + 1];
								++p_index;
							}
						}
					}
					if(i == s - 3 && j == 2)
					{
						next[n_index] += next[0];
					}
					if(next[n_index] > input)
					{
						return next[n_index].ToString();
					}
					++n_index;
				}
			}

			next[n_index] = next[n_index - 1] + prev[^1] + next[0];
			if(next[n_index] > input)
			{
				return next[n_index].ToString();
			}

			prev = next;
		}
	}
}
