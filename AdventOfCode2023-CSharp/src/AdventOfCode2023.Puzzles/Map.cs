using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

internal sealed class Map
{
    private static readonly char[] s_separators = { '\n' };
    private readonly List<RangeMapping> _rangeMappings;

    private Map(StringSegment title, List<RangeMapping> rangeMappings)
    {
        Title = title;
        _rangeMappings = rangeMappings;
    }

    internal StringSegment Title { get; }

    internal IReadOnlyList<RangeMapping> RangeMappings => _rangeMappings;

    public override string ToString() => Title.ToString();

    internal static Map Parse(string s)
    {
        StringTokenizer tokenizer = new(s, s_separators);
        using StringTokenizer.Enumerator enumerator = tokenizer.GetEnumerator();
        if (!enumerator.MoveNext())
            throw new ArgumentException("title", nameof(s));
        StringSegment title = enumerator.Current.Trim();
        List<RangeMapping> rangeMappings = new();
        while (enumerator.MoveNext())
        {
            StringSegment segment = enumerator.Current.Trim();
            if (segment.Length is 0)
                continue;
            var rangeMapping = RangeMapping.Parse(segment);
            rangeMappings.Add(rangeMapping);
        }

        return new(title, rangeMappings);
    }
}
