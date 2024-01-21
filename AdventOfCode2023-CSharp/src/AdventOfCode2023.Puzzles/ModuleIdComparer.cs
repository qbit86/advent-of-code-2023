using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal sealed class ModuleIdComparer : IComparer<string>
{
    private readonly IReadOnlyDictionary<string, int> _rankById;

    internal ModuleIdComparer(IReadOnlyDictionary<string, int> rankById) => _rankById = rankById;

    public int Compare(string? x, string? y)
    {
        if (ReferenceEquals(x, y))
            return 0;
        if (x is null)
            return -1;
        if (y is null)
            return 1;

        int left = _rankById.GetValueOrDefault(x);
        int right = _rankById.GetValueOrDefault(y);
        int comparison = left.CompareTo(right);
        if (comparison is not 0)
            return comparison;

        return StringComparer.Ordinal.Compare(x, y);
    }
}
