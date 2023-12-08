using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public sealed class Hand
{
    private readonly int[] _cardRanks;

    internal Hand(StringSegment originalSegment, int[] cardRanks, HandType handType)
    {
        _cardRanks = cardRanks;
        OriginalSegment = originalSegment;
        HandType = handType;
    }

    private StringSegment OriginalSegment { get; }
    public HandType HandType { get; }

    internal IReadOnlyList<int> CardRanks => _cardRanks;

    public override string ToString() => OriginalSegment.ToString();
}
