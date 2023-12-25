namespace AoC.Year2022;

public static class SnafuConverter
{
	const int Base = 5;

	private static int ConvertDigit(char value)
		=> value switch
		{
			'2' =>  2,
			'1' =>  1,
			'0' =>  0,
			'-' => -1,
			'=' => -2,
			 _  => throw new ArgumentException($"Invalid digit: {value}", nameof(value)),
		};

	private static char ConvertDigit(int value)
		=> value switch
		{
			-2 => '=',
			-1 => '-',
			 0 => '0',
			 1 => '1',
			 2 => '2',
			 _ => throw new ArgumentException($"Invalid digit: {value}", nameof(value)),
		};

	public static long Convert(string value)
	{
		var sum = 0L;
		var mul = 1L;
		for(int i = value.Length - 1; i >= 0; --i)
		{
			var digit = ConvertDigit(value[i]);
			sum += digit * mul;
			mul *= Base;
		}
		return sum;
	}

	private static string Concat(Stack<char> num)
		=> string.Create(num.Count, num, static (span, state) =>
		{
			for(int i = 0; i < span.Length; ++i)
			{
				span[i] = state.Pop();
			}
		});

	public static string Convert(long value)
	{
		var num = new Stack<char>();
		do
		{
			var mod = (int)(value % Base);
			value /= Base;
			if(mod <= 2)
			{
				num.Push(ConvertDigit(mod));
			}
			else
			{
				num.Push(ConvertDigit(mod - Base));
				++value;
			}
		}
		while(value != 0);
		return Concat(num);
	}
}
