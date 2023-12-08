using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2023;

internal sealed class PartOneHandFactory : HandFactory
{
    private PartOneHandFactory() { }

    internal static PartOneHandFactory Instance { get; } = new();

    protected override int GetCardRank(char card) => card switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        _ when char.IsDigit(card) => card - '0',
        _ => throw new ArgumentOutOfRangeException(nameof(card), card.ToString())
    };

    protected internal override HandType GetHandType<TDictionary>(TDictionary countByCardRank)
    {
        if (countByCardRank.Count is 1)
            return HandType.FiveOfKind;

        if (countByCardRank.Count is 4)
            return HandType.OnePair;

        if (countByCardRank.Count is 5)
            return HandType.HighCard;

        if (countByCardRank.Count is 2)
            return countByCardRank.Values.First() is 1 or 4 ? HandType.FourOfKind : HandType.FullHouse;

        Debug.Assert(countByCardRank.Count is 3);
        int maxCount = countByCardRank.Values.Max();
        if (maxCount is 3)
            return HandType.ThreeOfKind;

        Debug.Assert(maxCount is 2);
        return HandType.TwoPair;
    }
}
