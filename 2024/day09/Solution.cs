namespace AoC.Year2024;

/// <remarks><a href="https://adventofcode.com/2024/day/9"/></remarks>
[Name(@"Disk Fragmenter")]
public abstract class Day09Solution : Solution
{
	protected readonly record struct Entry(int? FileId, int Length);

	static LinkedList<Entry> ParseInput(string line)
	{
		var entries = new LinkedList<Entry>();
		var fileId = 0;
		for(int i = 0; i < line.Length; ++i)
		{
			var len = line[i] - '0';
			if(len == 0) continue;
			if((i & 1) == 0)
			{
				entries.AddLast(new Entry(fileId++, len));
			}
			else
			{
				if(entries.Last is { Value: { FileId: null } e} last)
				{
					last.Value = new(null, e.Length + len);
				}
				else
				{
					entries.AddLast(new Entry(null, len));
				}
			}
		}
		while(entries.Last is { Value.FileId: null })
		{
			entries.RemoveLast();
		}
		return entries;
	}

	protected abstract void Defragment(LinkedList<Entry> entries);

	static long CalcChecksum(LinkedList<Entry> entries)
	{
		static void UpdateChecksum(ref long checksum, long offset, int file, int len)
			=> checksum += file * (len * offset + ((len - 1) * len / 2));

		var checksum = 0L;
		var offset   = 0L;
		foreach(var entry in entries)
		{
			if(entry.FileId.HasValue)
			{
				UpdateChecksum(ref checksum, offset, entry.FileId.Value, entry.Length);
			}
			offset += entry.Length;
		}
		return checksum;
	}

	public sealed override string Process(TextReader reader)
	{
		var entries = ParseInput(reader.ReadLine() ?? throw new InvalidDataException());
		Defragment(entries);
		return CalcChecksum(entries).ToString();
	}
}

public sealed class Day09SolutionPart1 : Day09Solution
{
	private static LinkedListNode<Entry>? FindNextFreeSpace(LinkedListNode<Entry>? scan, LinkedListNode<Entry>? upTo = default)
	{
		while(scan is not null)
		{
			if(scan == upTo) return default;
			if(!scan.Value.FileId.HasValue) return scan;
			scan = scan.Next;
		}
		return default;
	}

	protected override void Defragment(LinkedList<Entry> entries)
	{
		var scan = FindNextFreeSpace(entries.First);
		var node = entries.Last;
		while(node is not null && scan is not null && node != scan)
		{
			var prev  = node.Previous;
			var entry = node.Value;
			if(!entry.FileId.HasValue)
			{
				node = prev;
				continue;
			}
			var free = scan.Value;
			if(free.Length < entry.Length)
			{
				scan.Value = new(entry.FileId, free.Length);
				node.Value = new(entry.FileId, entry.Length - free.Length);
				scan = FindNextFreeSpace(scan, node);
				continue;
			}
			else if(free.Length > entry.Length)
			{
				scan.Value = new(entry.FileId, entry.Length);
				var prevWasScan = prev == scan;
				scan = entries.AddAfter(scan, new Entry(null, free.Length - entry.Length));
				if(prevWasScan) prev = scan;
			}
			else
			{
				scan.Value = entry;
				scan = FindNextFreeSpace(scan, node);
			}
			entries.Remove(node);
			node = prev;
		}
	}
}

public sealed class Day09SolutionPart2 : Day09Solution
{
	static void FreeSpace(LinkedList<Entry> entries, LinkedListNode<Entry> node)
	{
		var entry = node.Value;
		if(node.Next is { Value: { FileId: null } nv } next)
		{
			if(node.Previous is { Value: { FileId: null } pv } prev)
			{
				prev.Value = new(null, pv.Length + nv.Length + entry.Length);
				entries.Remove(node);
				entries.Remove(next);
			}
			else
			{
				node.Value = new(null, nv.Length + entry.Length);
				entries.Remove(next);
			}
		}
		else if(node.Previous is { Value: { FileId: null } pv } prev)
		{
			prev.Value = new(null, pv.Length + entry.Length);
			entries.Remove(node);
		}
		else
		{
			node.Value = new(null, entry.Length);
		}
	}

	static LinkedListNode<Entry>? FindFreeSpace(LinkedList<Entry> entries, LinkedListNode<Entry>? upTo, int atLeast)
	{
		var node = entries.First;
		while(node is not null && node != upTo)
		{
			var entry = node.Value;
			if(!entry.FileId.HasValue && entry.Length >= atLeast)
			{
				return node;
			}
			node = node.Next;
		}
		return default;
	}

	static void TakeSpace(LinkedList<Entry> entries, LinkedListNode<Entry> free, Entry entry)
	{
		var freeEntry = free.Value;
		free.Value = new(entry.FileId, entry.Length);
		if(freeEntry.Length > entry.Length)
		{
			entries.AddAfter(free, new Entry(null, freeEntry.Length - entry.Length));
		}
	}

	static void TryMove(LinkedList<Entry> entries, LinkedListNode<Entry> node)
	{
		var entry = node.Value;
		if(!entry.FileId.HasValue) return;
		var freeNode = FindFreeSpace(entries, node, entry.Length);
		if(freeNode is null) return;
		TakeSpace(entries, freeNode, entry);
		FreeSpace(entries, node);
	}

	protected override void Defragment(LinkedList<Entry> entries)
	{
		var node = entries.Last;
		while(node is not null)
		{
			var prev = node.Previous;
			TryMove(entries, node);
			node = prev;
		}
	}
}
