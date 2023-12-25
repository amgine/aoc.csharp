namespace day6;

internal class Program
{
    static bool AreUnique(ReadOnlySpan<char> chars)
    {
        for(int i = 0; i < chars.Length - 1; ++i)
        {
            for(int j = i + 1; j < chars.Length; ++j)
            {
                if(chars[i] == chars[j]) return false;
            }
        }
        return true;
    }

    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");

        const int size = 14;

        Span<char> chars = stackalloc char[size];
        for(int i = 0; i < size - 1; ++i)
        {
            chars[i] = input[i];
        }
        for(int i = 3; i < input.Length; ++i)
        {
            chars[i % size] = input[i];
            if(AreUnique(chars))
            {
                Console.WriteLine(i + 1);
                break;
            }
        }
    }
}
