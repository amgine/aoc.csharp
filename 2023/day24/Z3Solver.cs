using Microsoft.Z3;

namespace AoC.Year2023;

/// <summary>Uses <c>Z3</c> to solve a system of 9 equations with 7 unknowns.</summary>
static class Z3Solver
{
	/// <summary><c>x + dx * t == fx + fdx * t</c>.</summary>
	static void AddEq(Context context, Solver solver, IntExpr t, long x, long dx, IntExpr fx, IntExpr fdx)
	{
		using var a  = context.MkInt(x);
		using var da = context.MkInt(dx);
		using var m1 = context.MkMul(da, t);
		using var a1 = context.MkAdd(a, m1);
		using var m2 = context.MkMul(fdx, t);
		using var a2 = context.MkAdd(fx, m2);
		using var eq = context.MkEq (a1, a2);
		solver.Add(eq);
	}

	public static long SolvePart2(List<Hailstone> hailstones)
	{
		if(hailstones.Count < 3) throw new ArgumentException("At least 3 hailstones are required.", nameof(hailstones));

		using var context = new Context();
		using var fx      = context.MkIntConst(@"fx");
		using var fy      = context.MkIntConst(@"fy");
		using var fz      = context.MkIntConst(@"fz");
		using var fdx     = context.MkIntConst(@"fdx");
		using var fdy     = context.MkIntConst(@"fdy");
		using var fdz     = context.MkIntConst(@"fdz");
		using var solver  = context.MkSolver();
		for(int i = 0; i < 3; ++i)
		{
			var hs = hailstones[i];
			using var t = context.MkIntConst($"t{i}");
			// is it fair for the time to be = 0?
			// maybe yes, maybe not
			using(BoolExpr e = t > 0) { solver.Add(e); }
			AddEq(context, solver, t, hs.Position.X, hs.Velocity.DeltaX, fx, fdx);
			AddEq(context, solver, t, hs.Position.Y, hs.Velocity.DeltaY, fy, fdy);
			AddEq(context, solver, t, hs.Position.Z, hs.Velocity.DeltaZ, fz, fdz);
		}
		if(solver.Check() != Status.SATISFIABLE)
		{
			throw new InvalidDataException("Cannot solve for the specified data.");
		}
		using var sum    = context.MkAdd(fx, fy, fz);
		using var model  = solver.Model;
		using var result = model.Evaluate(sum);
		if(result is not IntNum intNum) throw new ApplicationException();
		return intNum.Int64;
	}
}
