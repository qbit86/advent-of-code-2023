using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public abstract class Puzzle
{
    private static readonly char[] s_separators = { ' ' };

    public async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private long Solve(IEnumerable<string> lines)
    {
        IEnumerable<HandBidPair> handBidPairs = lines.Select(line => ParseHandBidPair(line));
        IOrderedEnumerable<HandBidPair> orderedPairs = handBidPairs.Order(HandBidPairComparer.Instance);
        IEnumerable<long> values = orderedPairs.Select((p, i) => p.Bid * (i + 1L));
        return values.Sum();
    }

    private HandBidPair ParseHandBidPair(StringSegment stringSegment)
    {
        StringTokenizer tokenizer = new(stringSegment, s_separators);
        using StringTokenizer.Enumerator enumerator = tokenizer.GetEnumerator();
        if (!enumerator.MoveNext())
            throw new ArgumentException(nameof(Hand), nameof(stringSegment));
        Hand hand = ParseHand(enumerator.Current);
        if (!enumerator.MoveNext())
            throw new ArgumentException("Bid", nameof(stringSegment));
        int bid = int.Parse(enumerator.Current, CultureInfo.InvariantCulture);
        return new(hand, bid);
    }

    protected abstract Hand ParseHand(StringSegment stringSegment);
}
