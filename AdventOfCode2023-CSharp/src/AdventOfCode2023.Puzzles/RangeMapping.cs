using System;
using System.Globalization;

namespace AdventOfCode2023;

internal readonly struct RangeMapping
{
    private RangeMapping(long sourceStart, long sourceEnd, long destinationStart)
    {
        SourceStart = sourceStart;
        SourceEnd = sourceEnd;
        DestinationStart = destinationStart;
    }

    internal long SourceStart { get; }
    internal long SourceEnd { get; }
    internal long DestinationStart { get; }

    public override string ToString() => $"[{SourceStart}..{SourceEnd}) \u2192 {DestinationStart}";

    internal static RangeMapping Parse(ReadOnlySpan<char> line)
    {
        Span<Range> ranges = stackalloc Range[3];
        int rangeCount = line.Split(ranges, ' ', StringSplitOptions.TrimEntries);
        if (rangeCount is not 3)
            throw new ArgumentException($"{nameof(rangeCount)} is not 3", nameof(line));
        long destinationStart = long.Parse(line[ranges[0]], CultureInfo.InvariantCulture);
        long sourceStart = long.Parse(line[ranges[1]], CultureInfo.InvariantCulture);
        long length = long.Parse(line[ranges[2]], CultureInfo.InvariantCulture);
        if (length < 0)
            throw new ArgumentException($"{nameof(length)} < 0", nameof(line));
        return new(sourceStart, sourceStart + length, destinationStart);
    }
}
