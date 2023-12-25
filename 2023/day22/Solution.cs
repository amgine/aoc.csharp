using System.Numerics;
using System.Runtime.InteropServices;

namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/22"/></remarks>
[Name(@"Sand Slabs")]
public abstract class Day22Solution : Solution
{
	protected readonly record struct Brick(Point3D A, Point3D B)
	{
		public static Brick Parse(string line)
		{
			var sep = line.IndexOf('~');
			if(sep < 0) throw new InvalidDataException(
				$"Invalid brick definition: {line}");
			return FromPoints(
				a: Parsers.ParsePoint3D(line.AsSpan(0, sep)),
				b: Parsers.ParsePoint3D(line.AsSpan(sep + 1)));
		}

		public static Brick FromPoints(Point3D a, Point3D b)
		{
			// solution code implies that A contains min coordinates
			// and B contains all max coordinates for simplicity
			// to avoid doing Math.Min()/Max() at every step
			// so sort them just in case (input seems to be pre-sorted)
			var (x0, x1) = Mathematics.Sort(a.X, b.X);
			var (y0, y1) = Mathematics.Sort(a.Y, b.Y);
			var (z0, z1) = Mathematics.Sort(a.Z, b.Z);

			var min = new Point3D(x0, y0, z0);
			var max = new Point3D(x1, y1, z1);
			return new(A: min, B: max);
		}

		/// <summary>Replaces brick minimum Z coordinate.</summary>
		/// <param name="z">New minimum Z.</param>
		/// <returns>Brick with replaced minimum Z.</returns>
		/// <remarks>Respects vertical bricks.</remarks>
		public Brick SetMinZ(int z) => new(
			A: A with { Z = z },
			B: B with { Z = z + (B.Z - A.Z) });

		public override string ToString()
			=> $"{A.X},{A.Y},{A.Z}~{B.X},{B.Y},{B.Z}";
	}

	protected static bool XYProjectionsIntersect(in Brick a, in Brick b)
	{
		var ax0 = a.A.X;
		var ay0 = a.A.Y;
		var ax1 = a.B.X;
		var ay1 = a.B.Y;

		var bx0 = b.A.X;
		var by0 = b.A.Y;
		var bx1 = b.B.X;
		var by1 = b.B.Y;

		if(ax0 == ax1) // 'a' projection is vertical (or has square of 1)
		{
			return (bx0 == bx1 && bx0 == ax0 && ay0 <= by1 && by0 <= ay1)
				|| (ax0 >= bx0 && ax0 <= bx1 && by0 >= ay0 && by0 <= ay1);
		}
		else // 'a' projection is horizontal
		{
			return (by0 == by1 && by0 == ay0 && ax0 <= bx1 && bx0 <= ax1)
				|| (ay0 >= by0 && ay0 <= by1 && bx0 >= ax0 && bx0 <= ax1);
		}
	}

	/// <summary>
	/// Checks if brick <paramref name="a"/> supports
	/// brick <paramref name="b"/>.
	/// </summary>
	/// <param name="a">Supporting brick.</param>
	/// <param name="b">Supported brick.</param>
	/// <returns>
	/// <see langword="true"/>, if <paramref name="a"/> supports <paramref name="b"/>,<br/>
	/// <see langword="false"/> otherwise.
	/// </returns>
	/// <remarks>
	/// Implies that both bricks are "settled",
	/// as in "not hanging in the air".
	/// </remarks>
	protected static bool Supports(in Brick a, in Brick b)
		=> a.B.Z == b.A.Z - 1            // max Z of 'a' is min Z of 'B' + 1 => they can touch in 3D
		&& XYProjectionsIntersect(a, b); // XY projections of 'a' and 'b' share at least 1 point

	/// <summary>Sorts bricks by minimum Z.</summary>
	/// <param name="bricks">List of bricks to sort.</param>
	static void SortByMinZ(List<Brick> bricks)
		=> bricks.Sort(static (a, b) => a.A.Z - b.A.Z);

	/// <summary>
	/// Makes all bricks "fall down" until they hit the
	/// ground or "settled" supporting brick.
	/// </summary>
	/// <param name="bricks">Bricks sorted by minimum Z.</param>
	/// <remarks>Reasonably fast <c>O(n*log(N))</c> and uses no extra memory.</remarks>
	protected static void SettleUsingBacktracking(List<Brick> bricks)
	{
		for(int i = 0; i < bricks.Count; ++i)
		{
			var brick = bricks[i];
			var z0 = brick.A.Z;
			if(z0 == 1) continue;
			var fallDownTo = 1;
			// bricks to the right cannot stop us
			for(int j = i - 1; j >= 0; --j)
			{
				var other  = bricks[j];
				var otherZ = other.B.Z;
				if(fallDownTo >= otherZ + 1)
				{
					// already left brick 'j' behind
					continue;
				}
				if(XYProjectionsIntersect(brick, other))
				{
					fallDownTo = Math.Max(fallDownTo, otherZ + 1);
					if(fallDownTo == z0) break; // stay in place
				}
			}
			if(z0 != fallDownTo)
			{
				bricks[i] = brick.SetMinZ(fallDownTo);
			}
		}
		SortByMinZ(bricks);
	}

	/// <summary>
	/// Makes all bricks "fall down" until they hit the
	/// ground or "settled" supporting brick.
	/// </summary>
	/// <param name="bricks">Bricks sorted by minimum Z.</param>
	/// <remarks>
	/// Uses height map (2D point -&gt; Z dictionary) to track maximum Z.<br/>
	/// Efficient considering input generates a thin and tall brick tower.
	/// </remarks>
	protected static void SettleUsingHeightMap(List<Brick> bricks)
	{
		static int GetHeight(Dictionary<Point2D, int> map, Point2D position)
			=> map.TryGetValue(position, out var height) ? height : 0;

		static int GetMaxHeight(Dictionary<Point2D, int> map, in Brick brick)
		{
			var max = 0;
			if(brick.A.X == brick.B.X)
			{
				var x0 = brick.A.X;
				var y0 = brick.A.Y;
				var y1 = brick.B.Y;
				for(int y = y0; y <= y1; ++y)
				{
					var h = GetHeight(map, new(x0, y));
					if(h > max) max = h;
				}
			}
			else
			{
				var x0 = brick.A.X;
				var x1 = brick.B.X;
				var y0 = brick.A.Y;
				for(int x = x0; x <= x1; ++x)
				{
					var h = GetHeight(map, new(x, y0));
					if(h > max) max = h;
				}
			}
			return max;
		}

		static void SetHeight(Dictionary<Point2D, int> map, in Brick brick)
		{
			var z1 = brick.B.Z;
			if(brick.A.X == brick.B.X)
			{
				var x0 = brick.A.X;
				var y0 = brick.A.Y;
				var y1 = brick.B.Y;
				for(int y = y0; y <= y1; ++y)
				{
					map[new(x0, y)] = z1;
				}
			}
			else
			{
				var x0 = brick.A.X;
				var x1 = brick.B.X;
				var y0 = brick.A.Y;
				for(int x = x0; x <= x1; ++x)
				{
					map[new(x, y0)] = z1;
				}
			}
		}

		var heightmap = new Dictionary<Point2D, int>();
		for(int i = 0; i < bricks.Count; ++i)
		{
			var brick    = bricks[i];
			var currentZ = brick.A.Z;
			var targetZ  = GetMaxHeight(heightmap, brick) + 1;
			if(currentZ != targetZ)
			{
				bricks[i] = brick = brick.SetMinZ(targetZ);
			}
			SetHeight(heightmap, brick);
		}
		SortByMinZ(bricks);
	}

	protected static List<Brick> ParseInput(TextReader reader)
	{
		var bricks = LoadListFromNonEmptyStrings(reader, Brick.Parse);
		SortByMinZ(bricks);
		return bricks;
	}

	/// <summary>Stores brick relationships.</summary>
	/// <remarks>
	/// Also reduces bricks to their indexes - no need to check
	/// their coordinates after we have the graph.
	/// </remarks>
	protected sealed class BrickSupportGraph
	{
		private readonly List<int>?[] _supported;
		private readonly List<int>?[] _supportedBy;

		/// <summary>Creates relationship graph.</summary>
		/// <param name="bricks">Bricks sorted by min Z.</param>
		public BrickSupportGraph(ReadOnlySpan<Brick> bricks)
		{
			_supported   = new List<int>[bricks.Length];
			_supportedBy = new List<int>[bricks.Length];
			for(int i = 0; i < bricks.Length - 1; ++i)
			{
				var a = bricks[i];
				var supportedZ = a.B.Z + 1;
				for(int j = i + 1; j < bricks.Length; ++j)
				{
					var b = bricks[j];
					var z = b.A.Z;
					if(z < supportedZ) continue;
					if(z > supportedZ) break;
					if(XYProjectionsIntersect(a, b))
					{
						AddRelation(i, j);
					}
				}
			}
		}

		private void AddRelation(int supportedBy, int supported)
		{
			static void Add(List<int>?[] lists, int index, int value)
			{
				var list = lists[index];
				if(list is null) lists[index] = [value];
				else             list.Add(value);
			}

			Add(_supported,   supportedBy, supported);
			Add(_supportedBy, supported, supportedBy);
		}

		/// <summary>
		/// Returns a list of brick supported by <paramref name="index"/>.
		/// </summary>
		/// <remarks>Returns <see langword="null"/> if none.</remarks>
		public List<int>? GetSupportedBy(int index) => _supportedBy[index];

		/// <summary>
		/// Returns a list of brick that <paramref name="index"/> supports.
		/// </summary>
		/// <remarks>Returns <see langword="null"/> if none.</remarks>
		public List<int>? GetSupported(int index) => _supported[index];
	}
}

public sealed class Day22SolutionPart1 : Day22Solution
{
	private static bool IsSafeToDesintegrate(BrickSupportGraph graph, int index)
	{
		var supported = graph.GetSupported(index);
		if(supported is null) return true;
		foreach(var child in supported)
		{
			if(graph.GetSupportedBy(child) is { Count: 1 })
			{
				// supported by 1 == supported by 'child'
				// will collapse if 'child' is removed
				return false;
			}
		}
		return true;
	}

	private static int CountSafeToDesintegrate(BrickSupportGraph graph, int bricksCount)
	{
		int count = 0;
		for(int i = 0; i < bricksCount; ++i)
		{
			if(IsSafeToDesintegrate(graph, i))
			{
				++count;
			}
		}
		return count;
	}

	public override string Process(TextReader reader)
	{
		var bricks = ParseInput(reader);
		SettleUsingHeightMap(bricks);
		var graph = new BrickSupportGraph(CollectionsMarshal.AsSpan(bricks));
		return CountSafeToDesintegrate(graph, bricks.Count).ToString();
	}
}

public sealed class Day22SolutionPart2 : Day22Solution
{
	/// <summary>
	/// Returns a set of brick indexes that will collapse
	/// if brick #<paramref name="index"/> is removed.
	/// </summary>
	/// <param name="index">Brick index to test.</param>
	/// <param name="graph">Brick support graph.</param>
	/// <returns>Set of collapsing bricks.</returns>
	/// <remarks>
	/// Removed brick <paramref name="index"/> itself is not
	/// included in the resulting set.
	/// </remarks>
	static HashSet<int> GetCollapsingSet(int index, BrickSupportGraph graph)
	{
		static bool WillCollapse(HashSet<int> set, int index, BrickSupportGraph graph)
		{
			if(graph.GetSupportedBy(index) is { Count: > 1 } parents)
			{
				foreach(var parent in parents)
				{
					if(!set.Contains(parent))
					{
						// something non-collapsing is supporting this brick
						return false;
					}
				}
			}
			return true;
		}

		static void Append(HashSet<int> set, Queue<int> queue, int index, BrickSupportGraph graph)
		{
			var supported = graph.GetSupported(index);
			if(supported is null) return;
			foreach(var child in supported)
			{
				if(WillCollapse(set, child, graph) && set.Add(child))
				{
					// inspect later - may cause further collapse
					queue.Enqueue(child);
				}
			}
		}

		var collapsing = new HashSet<int>();
		var queue      = new Queue<int>();
		var next       = index;
		do Append(collapsing, queue, next, graph);
		while(queue.TryDequeue(out next));

		return collapsing;
	}

	public override string Process(TextReader reader)
	{
		var bricks = ParseInput(reader);
		SettleUsingHeightMap(bricks);
		var graph = new BrickSupportGraph(CollectionsMarshal.AsSpan(bricks));
		var sum   = 0;
		for(int i = 0; i < bricks.Count; ++i)
		{
			sum += GetCollapsingSet(i, graph).Count;
		}
		return sum.ToString();
	}
}
