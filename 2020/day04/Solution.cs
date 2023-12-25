using System.Globalization;

namespace AoC.Year2020;

/// <remarks><a href="https://adventofcode.com/2020/day/4"/></remarks>
[Name(@"Passport Processing")]
public abstract class Day04Solution : Solution
{
	protected record class Passport(
	    string? BirthYear,
	    string? IssueYear,
	    string? ExpirationYear,
	    string? Height,
	    string? HairColor,
	    string? EyeColor,
	    string? PassportID,
	    string? CountryID);

	private static Passport CreatePassport(Dictionary<string, string> values) => new(
		BirthYear:      values.GetValueOrDefault(@"byr"),
		IssueYear:      values.GetValueOrDefault(@"iyr"),
		ExpirationYear: values.GetValueOrDefault(@"eyr"),
		Height:         values.GetValueOrDefault(@"hgt"),
		HairColor:      values.GetValueOrDefault(@"hcl"),
		EyeColor:       values.GetValueOrDefault(@"ecl"),
		PassportID:     values.GetValueOrDefault(@"pid"),
		CountryID:      values.GetValueOrDefault(@"cid"));

	static void UpdateDictionary(Dictionary<string, string> d, string line)
	{
		foreach(var kvp in line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
		{
			var sep   = kvp.IndexOf(':');
			var key   = kvp[..sep];
			var value = kvp[(sep + 1)..];
			d.Add(key, value);
		}
	}

	protected static IEnumerable<Passport> ParsePassports(TextReader reader)
	{
		var d = new Dictionary<string, string>();
		string? line;
		while((line = reader.ReadLine()) is not null)
		{
			if(line.Length == 0)
			{
				if(d.Count != 0)
				{
					var passport = CreatePassport(d);
					d.Clear();
					yield return passport;
				}
				continue;
			}
			UpdateDictionary(d, line);
		}

		if(d.Count != 0)
		{
			var passport = CreatePassport(d);
			d.Clear();
			yield return passport;
		}
	}

	protected abstract bool IsValid(Passport passport);

	public override string Process(TextReader reader)
		=> ParsePassports(reader).Count(IsValid).ToString();
}

public sealed class Day04SolutionPart1 : Day04Solution
{
	protected override bool IsValid(Passport passport)
	{
		if(passport.BirthYear      is null) return false;
		if(passport.IssueYear      is null) return false;
		if(passport.ExpirationYear is null) return false;
		if(passport.Height         is null) return false;
		if(passport.HairColor      is null) return false;
		if(passport.EyeColor       is null) return false;
		if(passport.PassportID     is null) return false;
		return true;
	}
}

public sealed class Day04SolutionPart2 : Day04Solution
{
	static bool IsHexColorValue(ReadOnlySpan<char> text)
	{
		for(int i = 0; i < text.Length; ++i)
		{
			var c = text[i];
			if(c is >= '0' and <= '9') continue;
			if(c is >= 'a' and <= 'f') continue;
			return false;
		}
		return true;
	}

	static bool IsNumberBetween(ReadOnlySpan<char> text, int min, int max)
		=> int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out var value)
		&& value >= min
		&& value <= max;

	static bool ValidateBirthYear(string? byr)
		=> byr is { Length: 4 }
		&& IsNumberBetween(byr, 1920, 2002);

	static bool ValidateIssueYear(string? iyr)
		=> iyr is { Length: 4 }
		&& IsNumberBetween(iyr, 2010, 2020);

	static bool ValidateExpirationYear(string? eyr)
		=> eyr is { Length: 4 }
		&& IsNumberBetween(eyr, 2020, 2030);

	static bool ValidateHeight(string? hgt)
		=> hgt is { Length: > 3 }
		&& ((hgt.EndsWith("cm") && IsNumberBetween(hgt.AsSpan(0, hgt.Length - 2), 150, 193))
		||  (hgt.EndsWith("in") && IsNumberBetween(hgt.AsSpan(0, hgt.Length - 2),  59,  76)));

	static bool ValidateHairColor(string? hcl)
		=> hcl is { Length: 7 }
		&& hcl[0] == '#'
		&& IsHexColorValue(hcl.AsSpan(1));

	static bool ValidateEyeColor(string? ecl)
		=> ecl is @"amb" or @"blu" or @"brn" or @"gry" or @"grn" or @"hzl" or @"oth";

	static bool ValidatePassportID(string? pid)
		=> pid is { Length: 9 }
		&& pid.All(char.IsAsciiDigit);

	protected override bool IsValid(Passport passport)
		=> ValidateBirthYear(passport.BirthYear)
		&& ValidateIssueYear(passport.IssueYear)
		&& ValidateExpirationYear(passport.ExpirationYear)
		&& ValidateHeight(passport.Height)
		&& ValidateHairColor(passport.HairColor)
		&& ValidateEyeColor(passport.EyeColor)
		&& ValidatePassportID(passport.PassportID);
}
