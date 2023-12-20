using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal sealed class NameComparer : IEqualityComparer<ReadOnlyMemory<char>>
{
    private NameComparer() { }

    internal static NameComparer Instance { get; } = new();

    public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y) => x.Span.SequenceEqual(y.Span);

    public int GetHashCode(ReadOnlyMemory<char> obj)
    {
        HashCode hashCode = new();
        foreach (char c in obj.Span)
            hashCode.Add(c);
        return hashCode.ToHashCode();
    }
}
