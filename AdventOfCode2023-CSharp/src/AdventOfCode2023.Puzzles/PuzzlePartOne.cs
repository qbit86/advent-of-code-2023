using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public static class PuzzlePartOne
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
        List<long> seeds = ParseSeeds(parts[0]);
        List<Map> maps = new();
        for (int i = 1; i < parts.Count; ++i)
        {
            var map = Map.Parse(parts[i]);
            maps.Add(map);
        }

        return seeds.Select(ComputeLocation).Min();

        long ComputeLocation(long seed)
        {
            return maps.Aggregate(seed, (current, map) => ComputeValue(map, current));
        }
    }

    private static List<long> ParseSeeds(string line)
    {
        List<long> seeds = new();
        StringSegment seedsSegment = new(line, 7, line.Length - 7);
        StringTokenizer tokenizer = new(seedsSegment, s_separators);
        foreach (StringSegment segment in tokenizer)
            seeds.Add(long.Parse(segment, CultureInfo.InvariantCulture));
        return seeds;
    }

    private static long ComputeValue(Map map, long argument)
    {
        foreach (RangeMapping r in map.RangeMappings)
        {
            if (argument < r.SourceStart || argument >= r.SourceEnd)
                continue;

            long offset = argument - r.SourceStart;
            return r.DestinationStart + offset;
        }

        return argument;
    }
}
