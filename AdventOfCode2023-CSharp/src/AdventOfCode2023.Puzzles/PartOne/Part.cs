using System;
using System.Diagnostics;
using System.Globalization;

namespace AdventOfCode2023;

internal readonly record struct Part(short X, short M, short A, short S)
{
    public override string ToString() => $"[{X}, {M}, {A}, {S}]";

    internal static Part Parse(string line)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(line));
        ReadOnlySpan<char> lineSpan = line.AsSpan();
        Span<Range> ranges = stackalloc Range[5];
        int count = lineSpan.SplitAny(ranges, "{},xmas=",
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (count is not 4)
            throw new ArgumentException($"{nameof(count)}: {count}", nameof(line));
        short x = short.Parse(lineSpan[ranges[0]], CultureInfo.InvariantCulture);
        short m = short.Parse(lineSpan[ranges[1]], CultureInfo.InvariantCulture);
        short a = short.Parse(lineSpan[ranges[2]], CultureInfo.InvariantCulture);
        short s = short.Parse(lineSpan[ranges[3]], CultureInfo.InvariantCulture);
        return new(x, m, a, s);
    }

    internal int GetValue(int key) => key switch
    {
        0 => X,
        1 => M,
        2 => A,
        3 => S,
        _ => throw new ArgumentOutOfRangeException(nameof(key))
    };
}
