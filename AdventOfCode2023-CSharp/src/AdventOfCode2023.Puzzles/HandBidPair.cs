using System;

namespace AdventOfCode2023;

public sealed class HandBidPair
{
    internal HandBidPair(Hand hand, int bid)
    {
        Hand = hand ?? throw new ArgumentNullException(nameof(hand));
        Bid = bid;
    }

    internal Hand Hand { get; }
    internal int Bid { get; }
}
