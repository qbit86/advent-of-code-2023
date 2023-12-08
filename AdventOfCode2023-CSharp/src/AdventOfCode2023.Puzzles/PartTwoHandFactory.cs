using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023;

internal sealed class PartTwoHandFactory : HandFactory
{
    private const int JokerRank = -1;

    private PartTwoHandFactory() { }

    internal static PartTwoHandFactory Instance { get; } = new();

    protected override int GetCardRank(char card) => card switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => JokerRank,
        'T' => 10,
        _ when char.IsDigit(card) => card - '0',
        _ => throw new ArgumentOutOfRangeException(nameof(card), card.ToString())
    };

    protected internal override HandType GetHandType<TDictionary>(TDictionary countByCardRank)
    {
        if (countByCardRank.Count is 1)
            return HandType.FiveOfKind;

        if (!countByCardRank.TryGetValue(JokerRank, out int jokerCount))
            return PartOneHandFactory.Instance.GetHandType(countByCardRank);

        Dictionary<int, int> newCountByCardRank = new(countByCardRank);
        newCountByCardRank.Remove(JokerRank);

        (int argmax, int max) = newCountByCardRank.MaxBy(it => it.Value);
        newCountByCardRank[argmax] = max + jokerCount;

        return PartOneHandFactory.Instance.GetHandType(newCountByCardRank);
    }
}
