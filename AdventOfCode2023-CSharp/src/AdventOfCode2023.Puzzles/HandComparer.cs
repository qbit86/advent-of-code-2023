using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal sealed class HandComparer : IComparer<Hand>
{
    private HandComparer() { }

    internal static HandComparer Instance { get; } = new();

    public int Compare(Hand? x, Hand? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        int handTypeComparison = x.HandType.CompareTo(y.HandType);
        if (handTypeComparison is not 0)
            return handTypeComparison;

        int count = Math.Max(x.CardRanks.Count, y.CardRanks.Count);
        for (int i = 0; i < count; ++i)
        {
            int comparison = x.CardRanks[i].CompareTo(y.CardRanks[i]);
            if (comparison is not 0)
                return comparison;
        }

        return x.CardRanks.Count.CompareTo(y.CardRanks.Count);
    }
}
