using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public static class PuzzlePartTwo
{
    private static readonly char[] s_separators = { ' ' };

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string allText = await File.ReadAllTextAsync(path, Encoding.UTF8).ConfigureAwait(false);
        string[] separators = { "\n\n", "\r\n\r\n" };
        string[] parts = allText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return Solve(parts);
    }

    private static long Solve<TPartCollection>(TPartCollection parts)
        where TPartCollection : IReadOnlyList<string>
    {
        List<LongRange> seedRanges = ParseSeeds(parts[0]);
        List<Map> maps = new();
        for (int i = 1; i < parts.Count; ++i)
        {
            var map = Map.Parse(parts[i]);
            maps.Add(map);
        }

        IEnumerable<LongRange> locationRanges =
            maps.Aggregate(seedRanges, (current, map) => ComputeValues(map, current));
        return locationRanges.Min(it => it.Start);
    }

    private static List<LongRange> ParseSeeds(string line)
    {
        List<LongRange> seedRanges = new();
        StringSegment seedsSegment = new(line, 7, line.Length - 7);
        StringTokenizer tokenizer = new(seedsSegment, s_separators);
        using StringTokenizer.Enumerator enumerator = tokenizer.GetEnumerator();
        while (enumerator.MoveNext())
        {
            long start = long.Parse(enumerator.Current, CultureInfo.InvariantCulture);
            if (!enumerator.MoveNext())
                throw new ArgumentException("MoveNext: false", nameof(line));
            long length = long.Parse(enumerator.Current, CultureInfo.InvariantCulture);
            LongRange range = new(start, start + length);
            seedRanges.Add(range);
        }

        return seedRanges;
    }

    private static List<LongRange> ComputeValues(Map map, IEnumerable<LongRange> sourceRanges)
    {
        List<LongRange> destinationRanges = new();
        List<LongRange> unprocessedRanges = new();
        foreach (RangeMapping rangeMapping in map.RangeMappings)
        {
            unprocessedRanges = new();
            ProcessRangeMapping(rangeMapping, sourceRanges, destinationRanges, unprocessedRanges);
            sourceRanges = unprocessedRanges;
        }

        destinationRanges.AddRange(unprocessedRanges);

        return destinationRanges;
    }

    private static void ProcessRangeMapping<TRangeCollection>(
        RangeMapping rangeMapping, IEnumerable<LongRange> sourceRanges,
        TRangeCollection destinationRanges, TRangeCollection unprocessedRanges)
        where TRangeCollection : ICollection<LongRange>
    {
        foreach (LongRange sourceRange in sourceRanges)
        {
            long newSourceStart = Math.Max(sourceRange.Start, rangeMapping.SourceStart);
            long newSourceEnd = Math.Min(sourceRange.End, rangeMapping.SourceEnd);

            if (newSourceStart >= newSourceEnd)
            {
                unprocessedRanges.Add(sourceRange);
                continue;
            }

            long offset = newSourceStart - rangeMapping.SourceStart;
            long destinationStart = rangeMapping.DestinationStart + offset;
            long newLength = newSourceEnd - newSourceStart;
            long destinationEnd = destinationStart + newLength;
            LongRange destinationRange = new(destinationStart, destinationEnd);
            destinationRanges.Add(destinationRange);

            if (sourceRange.Start < newSourceStart)
                unprocessedRanges.Add(sourceRange with { End = newSourceStart });

            if (newSourceEnd < sourceRange.End)
                unprocessedRanges.Add(sourceRange with { Start = newSourceEnd });
        }
    }
}
