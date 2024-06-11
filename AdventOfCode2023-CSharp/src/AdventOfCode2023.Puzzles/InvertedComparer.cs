using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal readonly struct InvertedComparer<T> : IComparer<T>
    where T : IComparable<T>
{
    public int Compare(T? x, T? y) => y!.CompareTo(x);
}
