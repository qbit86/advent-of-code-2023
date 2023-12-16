using System;

namespace AdventOfCode2023;

public static class Helpers
{
    public static byte Hash(string s) => Hash(s.AsSpan());

    public static byte Hash(ReadOnlySpan<char> chars)
    {
        nint currentValue = 0;
        foreach (char c in chars)
        {
            if (!char.IsAscii(c))
                throw new ArgumentException(c.ToString(), nameof(chars));
            nint asciiCode = c;
            currentValue += asciiCode;
            currentValue *= 17;
            currentValue %= 256;
        }

        return byte.CreateChecked(currentValue);
    }
}
