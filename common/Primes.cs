namespace AoC;

public static class Primes
{
	static readonly List<int> _cachedPrimes = [2, 3];

	static int NextPrime(int prime)
	{
		while(true)
		{
			prime += 2;
			bool isPrime = true;
			foreach(var p in _cachedPrimes)
			{
				if((prime % p) == 0)
				{
					isPrime = false;
					break;
				}
			}
			if(isPrime) return prime;
		}
	}

	public static IEnumerable<int> Get()
	{
		var last = 0;
		foreach(var p in _cachedPrimes)
		{
			yield return p;
			last = p;
		}
		while(true)
		{
			var p = NextPrime(last);
			_cachedPrimes.Add(p);
			last = p;
			yield return p;
		}
	}
}
