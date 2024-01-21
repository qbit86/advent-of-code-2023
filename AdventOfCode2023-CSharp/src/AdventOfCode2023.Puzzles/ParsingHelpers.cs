using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal static class ParsingHelpers
{
    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    internal static Module ParseModule(string line)
    {
        KeyValuePair<ReadOnlyMemory<char>, string[]> kv = ParseRaw(line);
        return ParseModule(kv.Key.Span, kv.Value);
    }

    internal static KeyValuePair<ReadOnlyMemory<char>, string[]> ParseRaw(string line)
    {
        Span<Range> ranges = stackalloc Range[3];
        ReadOnlySpan<char> lineSpan = line.AsSpan();
        int partCount = lineSpan.Split(ranges, "->", SplitOptions);
        if (partCount is not 2)
            throw new InvalidOperationException($"{nameof(partCount)}: {partCount}");
        ReadOnlySpan<char> rightPart = lineSpan[ranges[1]];
        string[] destinationIds = rightPart.ToString().Split(',', SplitOptions);
        ReadOnlyMemory<char> typedId = line.AsMemory(ranges[0]);
        return new(typedId, destinationIds);
    }

    internal static Module ParseModule(ReadOnlySpan<char> typedId, string[] destinationIds)
    {
        if (typedId is BroadcastModule.BroadcasterId)
            return new BroadcastModule(destinationIds);

        if (typedId[0] is '%')
            return new FlipFlopModule(typedId[1..].ToString(), destinationIds);

        if (typedId[0] is '&')
            return ConjunctionModule.Create(typedId[1..].ToString(), destinationIds);

        throw new ArgumentException(typedId.ToString(), nameof(typedId));
    }
}
