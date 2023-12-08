using System.Collections.Generic;

namespace AdventOfCode2023;

internal sealed class HandBidPairComparer : IComparer<HandBidPair>
{
    private HandBidPairComparer() { }

    internal static HandBidPairComparer Instance { get; } = new();

    public int Compare(HandBidPair? x, HandBidPair? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        return HandComparer.Instance.Compare(x.Hand, y.Hand);
    }
}
