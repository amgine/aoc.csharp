namespace AoC.Year2023;

/// <remarks><a href="https://adventofcode.com/2023/day/20"/></remarks>
[Name("Pulse Propagation")]
public abstract class Day20Solution : Solution
{
	protected enum Pulse { Low, High }

	protected readonly record struct Signal(Module From, Module To, Pulse Pulse);

	protected readonly record struct SignalCounts(int Low, int High)
	{
		public static readonly SignalCounts Zero = new(0, 0);

		public static SignalCounts operator +(SignalCounts a, SignalCounts b)
			=> new(a.Low + b.Low, a.High + b.High);
	}

	protected sealed class Module(string name)
	{
		public string Name => name;

		public Module[] Outputs { get; private set; } = [];

		public IModuleProcessor Processor { get; set; } = NullModuleProcessor.Instance;

		public List<Module> Inputs { get; } = [];

		public void QueuePulse(Pulse pulse, ISignalQueue queue)
		{
			foreach(var output in Outputs)
			{
				queue.Enqueue(new(this, output, pulse));
			}
		}

		public void Emit(Pulse pulse, ISignalSequence queue)
		{
			QueuePulse(pulse, queue);
			while(queue.TryDequeue(out var signal))
			{
				signal.To.Processor.EmitSignals(signal, queue);
			}
		}

		public void Emit(Pulse pulse, ISignalSequence queue, int count)
		{
			while(count-- > 0) Emit(pulse, queue);
		}

		public void SetOutputs(Module[] outputs)
		{
			Outputs = outputs;
			foreach(var output in outputs)
			{
				output.Inputs.Add(this);
			}
		}

		public override string ToString() => name;
	}

	protected class ObservableSignalQueue : ISignalSequence
	{
		private readonly Queue<Signal> _queue = new();

		protected virtual void OnEnqueue(Signal signal) { }

		public void Enqueue(Signal signal)
		{
			OnEnqueue(signal);
			_queue.Enqueue(signal);
		}

		public bool TryDequeue(out Signal signal)
			=> _queue.TryDequeue(out signal);
	}

	protected interface ISignalQueue
	{
		void Enqueue(Signal signal);
	}

	protected interface ISignalSequence : ISignalQueue
	{
		bool TryDequeue(out Signal signal);
	}

	protected interface IModuleProcessor
	{
		void EmitSignals(Signal activationSignal, ISignalQueue queue);
	}

	sealed class NullModuleProcessor : IModuleProcessor
	{
		public static readonly NullModuleProcessor Instance = new();

		private NullModuleProcessor() { }

		public void EmitSignals(Signal activationSignal, ISignalQueue signals) { }
	}

	protected class FlipFlopModuleProcessor(Module owner) : IModuleProcessor
	{
		private bool _isEnabled = false;

		public void EmitSignals(Signal activationSignal, ISignalQueue queue)
		{
			if(activationSignal.Pulse != Pulse.Low) return;
			_isEnabled = !_isEnabled;
			owner.QueuePulse(_isEnabled ? Pulse.High : Pulse.Low, queue);
		}
	}

	protected class ConjunctionModuleProcessor(Module owner) : IModuleProcessor
	{
		readonly Dictionary<Module, Pulse> _memory = [];

		const Pulse InitialRememberedState = Pulse.Low;

		private Pulse GetLast(Module from)
			=> _memory.TryGetValue(from, out var p) ? p : InitialRememberedState;

		private bool CheckIfAllHigh()
		{
			foreach(var input in owner.Inputs)
			{
				if(GetLast(input) != Pulse.High) return false;
			}
			return true;
		}

		public void EmitSignals(Signal activationSignal, ISignalQueue queue)
		{
			_memory[activationSignal.From] = activationSignal.Pulse;
			owner.QueuePulse(CheckIfAllHigh() ? Pulse.Low : Pulse.High, queue);
		}
	}

	protected class BroadcasterModuleProcessor : IModuleProcessor
	{
		public static readonly BroadcasterModuleProcessor Instance = new();

		private BroadcasterModuleProcessor() { }

		public void EmitSignals(Signal activationSignal, ISignalQueue queue)
			=> activationSignal.To.QueuePulse(activationSignal.Pulse, queue);
	}

	protected sealed class ModuleParser
	{
		private readonly Dictionary<string, Module> _lookup = new();

		public static ModuleParser Parse(TextReader reader)
		{
			var parser = new ModuleParser();
			string? line;
			while((line = reader.ReadLine()) is not null)
			{
				if(line.Length == 0) continue;
				parser.ParseModule(line);
			}
			return parser;
		}

		public Module GetOrCreateModule(string name)
		{
			if(!_lookup.TryGetValue(name, out var module))
			{
				_lookup.Add(name, module = new(name));
			}
			return module;
		}

		public Module GetModule(string name) => _lookup[name];

		private Module InitModuleProcessor(ReadOnlySpan<char> name)
		{
			if(name[0] == '%')
			{
				var module = GetOrCreateModule(new(name[1..]));
				module.Processor = new FlipFlopModuleProcessor(module);
				return module;
			}
			if(name[0] == '&')
			{
				var module = GetOrCreateModule(new(name[1..]));
				module.Processor = new ConjunctionModuleProcessor(module);
				return module;
			}
			if(name.SequenceEqual("broadcaster"))
			{
				var module = GetOrCreateModule("broadcaster");
				module.Processor = BroadcasterModuleProcessor.Instance;
				return module;
			}
			throw new InvalidDataException($"Unknown module type: {new(name)}");
		}

		public Module ParseModule(string line)
		{
			var sep = line.IndexOf("->");
			if(sep == -1) throw new InvalidDataException($"Invalid module definition: {line}");
			var module = InitModuleProcessor(line.AsSpan(0, sep).Trim());
			module.SetOutputs(Array.ConvertAll(
				line[(sep + 2)..].Split(',', StringSplitOptions.TrimEntries),
				GetOrCreateModule));
			return module;
		}
	}

	protected static Module AddButtonModule(Module buttonOutputReceiver)
	{
		var button = new Module("button");
		button.SetOutputs([buttonOutputReceiver]);
		button.Processor = BroadcasterModuleProcessor.Instance;
		return button;
	}
}

public sealed class Day20SolutionPart1 : Day20Solution
{
	sealed class CountingQueue : ObservableSignalQueue
	{
		private readonly int[] _counts = new int[2];

		protected override void OnEnqueue(Signal signal)
			=> ++_counts[(int)signal.Pulse];

		public SignalCounts Counts
			=> new(_counts[(int)Pulse.Low], _counts[(int)Pulse.High]);
	}

	static Module ParseModules(TextReader reader)
		=> AddButtonModule(ModuleParser
			.Parse(reader)
			.GetModule(@"broadcaster"));

	static int GetAnswer(SignalCounts counts)
		=> counts.Low * counts.High;

	public override string Process(TextReader reader)
	{
		var button = ParseModules(reader);
		var queue  = new CountingQueue();
		button.Emit(Pulse.Low, queue, count: 1000);
		return GetAnswer(queue.Counts).ToString();
	}
}

public sealed class Day20SolutionPart2 : Day20Solution
{
	const string OutputModuleName = @"rx";

	sealed class WatchLowPulseInputQueue(Module[] modules)
		: ObservableSignalQueue
	{
		private readonly Module[] _modules = modules;
		private readonly bool  [] _hadLow  = new bool[modules.Length];

		protected override void OnEnqueue(Signal signal)
		{
			if(signal.Pulse != Pulse.Low) return;
			var index = Array.IndexOf(_modules, signal.To);
			if(index >= 0) _hadLow[index] = true;
		}

		public bool HadLowInput(Module module)
		{
			var index = Array.IndexOf(_modules, module);
			return index >= 0 && _hadLow[index];
		}

		public void Reset() => Array.Clear(_hadLow);
	}

	struct InputState
	{
		public int PreviousCount;
		public int CycleLength;
		public int RepeatedCycles;

		public bool TryUpdate(int counter)
		{
			var cycleLength = counter - PreviousCount;
			if(RepeatedCycles > 0)
			{
				if(cycleLength != CycleLength)
				{
					return false;
				}
			}
			else
			{
				CycleLength = cycleLength;
			}
			PreviousCount = counter;
			++RepeatedCycles;
			return true;
		}
	}

	static (Module button, Module rx) ParseModules(TextReader reader)
	{
		var parser = ModuleParser.Parse(reader);
		return (
			button: AddButtonModule(parser.GetModule(@"broadcaster")),
			rx:     parser.GetModule(OutputModuleName));
	}

	static Module[] GetInputs(Module rx)
	{
		if(rx.Inputs.Count != 1)
		{
			throw new NotSupportedException($"{OutputModuleName} must have 1 input for this solution.");
		}
		if(rx.Inputs[0].Processor is not ConjunctionModuleProcessor)
		{
			throw new NotSupportedException($"{OutputModuleName} must have 1 conjunction input module for this solution.");
		}
		if(rx.Inputs[0].Inputs.Count == 0)
		{
			throw new NotSupportedException($"No inputs for the {rx.Inputs[0].Name} -> {OutputModuleName} chain.");
		}
		if(!rx.Inputs[0].Inputs.All(static i => i.Processor is ConjunctionModuleProcessor))
		{
			throw new NotSupportedException("Must be conjunction input modules.");
		}

		return [.. rx.Inputs[0].Inputs];
	}

	static long Count(Module button, Module rx)
	{
		var inputs  = GetInputs(rx);
		var state   = new InputState[inputs.Length];
		var queue   = new WatchLowPulseInputQueue(inputs);
		var counter = 0;
		do
		{
			button.Emit(Pulse.Low, queue);
			++counter;
			for(int i = 0; i < inputs.Length; ++i)
			{
				var input = inputs[i];
				if(!queue.HadLowInput(input)) continue;
				if(!state[i].TryUpdate(counter))
				{
					throw new NotSupportedException($"Cannot detect cycle for {input.Name}");
				}
			}
			queue.Reset();
		}
		while(!CyclesRepeatedAtLeastTwice(state));

		return MatchCycles(state);
	}

	static bool CyclesRepeatedAtLeastTwice(InputState[] state)
		=> Array.TrueForAll(state, static s => s.CycleLength >= 2);

	static long MatchCycles(InputState[] state)
	{
		long p = state[0].CycleLength;
		for(int i = 1; i < state.Length; ++i)
		{
			p = Mathematics.LCM(p, state[i].CycleLength);
		}
		return p;
	}

	public override string Process(TextReader reader)
	{
		var (button, rx) = ParseModules(reader);
		return Count(button, rx).ToString();
	}
}
