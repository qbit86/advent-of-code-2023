using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal sealed class InverseComparer<T> : IComparer<T>
    where T : IComparable<T>
{
    private InverseComparer() { }

    internal static InverseComparer<T> Instance { get; } = new();

    public int Compare(T? x, T? y) => y!.CompareTo(x);
}
