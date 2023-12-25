namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/19"/></remarks>
[Name("Aplenty")]
public abstract class Day19Solution : Solution
{
	protected readonly record struct Part(int X, int M, int A, int S)
	{
		public static readonly Part Invalid = new(-1, -1, -1, -1);
	}

	protected readonly record struct PartRange(Part Min, Part Max)
	{
		public static readonly PartRange None = new(new(0, 0, 0, 0), Part.Invalid);
		public static readonly PartRange All  = new(
			Min: new(   1,    1,    1,    1),
			Max: new(4000, 4000, 4000, 4000));

		public static PartRange Validate(PartRange range)
		{
			if(range.Min.X > range.Max.X) return None;
			if(range.Min.M > range.Max.M) return None;
			if(range.Min.A > range.Max.A) return None;
			if(range.Min.S > range.Max.S) return None;
			return range;
		}

		public long PartsCount
		{
			get
			{
				long x = Max.X - Min.X + 1;
				long m = Max.M - Min.M + 1;
				long a = Max.A - Min.A + 1;
				long s = Max.S - Min.S + 1;
				return x * m * a * s;
			}
		}

		public bool Contains(Part part)
			=> part.X >= Min.X && part.X <= Max.X
			&& part.M >= Min.M && part.M <= Max.M
			&& part.A >= Min.A && part.A <= Max.A
			&& part.S >= Min.S && part.S <= Max.S;
	}

	protected sealed class Workflow(string name)
	{
		public string Name { get; } = name;

		public List<Rule> Rules { get; } = [];

		public RuleResult Process(Part part)
		{
			foreach(var rule in Rules)
			{
				if(rule.Condition.Test(part))
				{
					return rule.Action.Execute(part);
				}
			}
			throw new InvalidDataException($"Workflow {Name}: inconclusive result for part {part}");
		}

		public long Process(PartRange range)
		{
			var accepted = 0L;
			foreach(var rule in Rules)
			{
				if(range == PartRange.None) return accepted;
				var (negative, positive) = rule.Condition.Test(range);
				if(positive != PartRange.None)
				{
					accepted += rule.Action.Execute(positive);
				}
				range = negative;
			}
			return accepted;
		}
	}

	protected static Part ParsePart(string line)
	{
		Span<Range> ranges = stackalloc Range[4];
		var ls = line.AsSpan(1, line.Length - 2);
		if(ls.Split(ranges, ',') != 4) throw new InvalidDataException();

		int x = 0;
		int m = 0;
		int a = 0;
		int s = 0;

		foreach(var r in ranges)
		{
			var partSlice = ls[r];
			var eqs = partSlice.IndexOf('=');
			if(eqs < 0) throw new InvalidDataException();
			var value = int.Parse(partSlice[(eqs + 1)..]);
			switch(partSlice[0])
			{
				case 'x': x = value; break;
				case 'm': m = value; break;
				case 'a': a = value; break;
				case 's': s = value; break;
				default: throw new InvalidDataException();
			}
		}

		return new(x, m, a, s);
	}

	interface IPartProperty
	{
		int GetValue(Part part);

		Part Replace(Part part, int value);
	}

	sealed class PartX : IPartProperty
	{
		public static readonly IPartProperty Instance = new PartX();

		public int GetValue(Part part) => part.X;

		public Part Replace(Part part, int value) => part with { X = value };
	}

	sealed class PartM : IPartProperty
	{
		public static readonly IPartProperty Instance = new PartM();

		public int GetValue(Part part) => part.M;

		public Part Replace(Part part, int value) => part with { M = value };
	}

	sealed class PartA : IPartProperty
	{
		public static readonly IPartProperty Instance = new PartA();

		public int GetValue(Part part) => part.A;

		public Part Replace(Part part, int value) => part with { A = value };
	}

	sealed class PartS : IPartProperty
	{
		public static readonly IPartProperty Instance = new PartS();

		public int GetValue(Part part) => part.S;

		public Part Replace(Part part, int value) => part with { S = value };
	}

	protected interface IRuleCondition
	{
		bool Test(Part part);

		(PartRange Negative, PartRange Positive) Test(in PartRange range);
	}

	sealed class AlwaysCondition : IRuleCondition
	{
		public static readonly AlwaysCondition Instance = new();

		private AlwaysCondition() { }

		public bool Test(Part part) => true;

		public (PartRange Negative, PartRange Positive) Test(in PartRange range)
			=> (PartRange.None, range);
	}

	sealed class LessThanCondition(IPartProperty operand, int value) : IRuleCondition
	{
		public bool Test(Part part) => operand.GetValue(part) < value;

		public (PartRange Negative, PartRange Positive) Test(in PartRange range)
		{
			if(range == PartRange.None) return (PartRange.None, PartRange.None);
			return (
				Negative: PartRange.Validate(range with { Min = operand.Replace(range.Min, value    ) }),
				Positive: PartRange.Validate(range with { Max = operand.Replace(range.Max, value - 1) }));
		}
	}

	sealed class GreaterThanCondition(IPartProperty operand, int value) : IRuleCondition
	{
		public bool Test(Part part) => operand.GetValue(part) > value;

		public (PartRange Negative, PartRange Positive) Test(in PartRange range)
		{
			if(range == PartRange.None) return (PartRange.None, PartRange.None);
			return (
				Negative: PartRange.Validate(range with { Max = operand.Replace(range.Max, value    ) }),
				Positive: PartRange.Validate(range with { Min = operand.Replace(range.Min, value + 1) }));
		}
	}

	protected enum RuleResult { Accepted, Rejected }

	protected sealed class Rule(IRuleCondition condition, IRuleAction action)
	{
		public IRuleCondition Condition { get; } = condition;

		public IRuleAction Action { get; } = action;
	}

	protected interface IRuleAction
	{
		RuleResult Execute(Part part);

		long Execute(PartRange range);
	}

	sealed class DelegateRuleAction(Workflow other) : IRuleAction
	{
		public RuleResult Execute(Part part) => other.Process(part);

		public long Execute(PartRange range) => other.Process(range);
	}

	sealed class ResultRuleAction : IRuleAction
	{
		public static readonly IRuleAction Accept = new ResultRuleAction(RuleResult.Accepted);

		public static readonly IRuleAction Reject = new ResultRuleAction(RuleResult.Rejected);

		private readonly RuleResult _result;

		private ResultRuleAction(RuleResult result) => _result = result;

		public RuleResult Execute(Part part) => _result;

		public long Execute(PartRange range) => _result == RuleResult.Accepted ? range.PartsCount : 0;
	}

	sealed class WorkflowParser
	{
		private readonly Dictionary<string, Workflow> _lookup = new();

		private static IPartProperty ParsePartOperand(ReadOnlySpan<char> s)
			=> s switch
			{
				"x" => PartX.Instance,
				"m" => PartM.Instance,
				"a" => PartA.Instance,
				"s" => PartS.Instance,
				_   => throw new InvalidDataException($"Unknown operand: {new string(s)}"),
			};

		private static IRuleCondition CreateCondition(char @operator, IPartProperty comparand1, int comparand2)
			=> @operator switch
			{
				'<' => new LessThanCondition   (comparand1, comparand2),
				'>' => new GreaterThanCondition(comparand1, comparand2),
				_   => throw new InvalidDataException($"Unknown operator: {@operator}"),
			};

		private static IRuleCondition ParseCondition(ReadOnlySpan<char> s)
		{
			var i = s.IndexOfAny(['<', '>']);
			if(i == -1) throw new InvalidDataException();
			return CreateCondition(s[i],
				comparand1: ParsePartOperand(s[..i].Trim()),
				comparand2: int.Parse(s[(i + 1)..]));
		}

		private Rule ParseRule(ReadOnlySpan<char> rule)
		{
			var sep = rule.IndexOf(':');
			var condition = sep == -1
				? AlwaysCondition.Instance
				: ParseCondition(rule[..sep]);
			var to = sep == -1 ? rule : rule[(sep + 1)..];
			var action = to switch
			{
				"A" => ResultRuleAction.Accept,
				"R" => ResultRuleAction.Reject,
				_   => new DelegateRuleAction(GetOrCreateWorkflow(new(to))),
			};
			return new Rule(condition, action);
		}

		private Workflow GetOrCreateWorkflow(string name)
		{
			if(!_lookup.TryGetValue(name, out var wf))
			{
				_lookup.Add(name, wf = new(name));
			}
			return wf;
		}

		public Workflow ParseWorkflow(string line)
		{
			var s = line.IndexOf('{');
			var wf = GetOrCreateWorkflow(line[..s]);
			line = line.Substring(s + 1, line.Length - s - 2);
			var rules = line.Split(',', StringSplitOptions.TrimEntries);
			foreach(var p in rules)
			{
				wf.Rules.Add(ParseRule(p));
			}
			return wf;
		}
	}

	protected static Workflow ParseWorkflows(TextReader reader)
	{
		var parser = new WorkflowParser();
		var @in = default(Workflow);
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0) break;
			var wf = parser.ParseWorkflow(line);
			if(@in is null && wf.Name == "in")
			{
				@in = wf;
			}
		}
		return @in ?? throw new InvalidDataException("Workflow 'in' is not defined.");
	}
}

public sealed class Day19SolutionPart1 : Day19Solution
{
	static int GetRating(Part part)
		=> part.X + part.M + part.A + part.S;

	public override string Process(TextReader reader)
	{
		var workflow = ParseWorkflows(reader);
		var sum = SumFromNonEmptyLines(reader,
			line =>
			{
				var part = ParsePart(line);
				return workflow.Process(part) == RuleResult.Accepted
					? GetRating(part)
					: 0;
			});
		return sum.ToString();
	}
}

public sealed class Day19SolutionPart2 : Day19Solution
{
	public override string Process(TextReader reader)
		=> ParseWorkflows(reader).Process(PartRange.All).ToString();
}
