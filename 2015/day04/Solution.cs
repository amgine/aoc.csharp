using System;
using System.Security.Cryptography;
using System.Text;

namespace AoC.Year2015;

#pragma warning disable CA1850

/// <remarks><a href="https://adventofcode.com/2015/day/4"/></remarks>
[Name(@"The Ideal Stocking Stuffer")]
public abstract class Day04Solution : Solution
{
	protected abstract bool IsTerminal(ReadOnlySpan<byte> md5);

	public override string Process(TextReader reader)
	{
		var line = reader.ReadLine() ?? throw new InvalidDataException();
		Span<byte> data = stackalloc byte[line.Length + 11];
		Span<byte> md5  = stackalloc byte[MD5.HashSizeInBytes];
		var symbolBytes    = Encoding.ASCII.GetBytes(line, data);
		var dataForInteger = data[symbolBytes..];
		using var alg = MD5.Create();
		for(int i = 0; i is >= 0 and < int.MaxValue; ++i)
		{
			if(!i.TryFormat(dataForInteger, out int integerBytes))
				throw new ApplicationException("Failed to format integer value");
			if(!alg.TryComputeHash(data[..(symbolBytes + integerBytes)], md5, out _))
				throw new ApplicationException("Failed to calculate MD5.");
			if(IsTerminal(md5)) return i.ToString();
		}
		throw new InvalidDataException();
	}
}

public sealed class Day04SolutionPart1 : Day04Solution
{
	protected override bool IsTerminal(ReadOnlySpan<byte> md5)
		=> md5[0] == 0 && md5[1] == 0 && (md5[2] & 0xf0) == 0;
}

public sealed class Day04SolutionPart2 : Day04Solution
{
	protected override bool IsTerminal(ReadOnlySpan<byte> md5)
		=> md5[0] == 0 && md5[1] == 0 && md5[2] == 0;
}
