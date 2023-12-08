using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

internal abstract class HandFactory
{
    internal Hand Parse(StringSegment stringSegment)
    {
        int[] cardRanks = GC.AllocateUninitializedArray<int>(stringSegment.Length);
        int count = stringSegment.Length;
        Dictionary<int, int> countByCardRank = new();
        for (int i = 0; i < count; ++i)
        {
            int cardRank = GetCardRank(stringSegment[i]);
            cardRanks[i] = cardRank;
            ++CollectionsMarshal.GetValueRefOrAddDefault(countByCardRank, cardRank, out _);
        }

        HandType handType = GetHandType(countByCardRank);
        return new(stringSegment, cardRanks, handType);
    }

    protected abstract int GetCardRank(char card);

    protected internal abstract HandType GetHandType<TDictionary>(TDictionary countByCardRank)
        where TDictionary : IReadOnlyDictionary<int, int>;
}
