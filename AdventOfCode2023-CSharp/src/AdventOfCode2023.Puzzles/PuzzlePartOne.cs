using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using static AdventOfCode2023.Helpers;

namespace AdventOfCode2023;

public static class PuzzlePartOne
{
    private static readonly char[] s_separators = { ' ' };

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IReadOnlyList<string> lines)
    {
        List<int> times = Parse(lines[0]);
        List<int> distances = Parse(lines[1]);
        IEnumerable<(int Time, int Distance)> timeDistancePairs = times.Zip(distances);
        IEnumerable<int> wayCounts = timeDistancePairs.Select(it => CountWays(it.Time, it.Distance));
        return wayCounts.Aggregate(1L, (left, right) => left * right);
    }

    private static List<int> Parse(string line)
    {
        List<int> result = new();
        const int offset = 11;
        StringSegment valuesSegment = new(line, offset, line.Length - offset);
        StringTokenizer tokenizer = new(valuesSegment, s_separators);
        foreach (StringSegment segment in tokenizer)
        {
            if (segment is { Length: > 0 })
                result.Add(int.Parse(segment, CultureInfo.InvariantCulture));
        }

        return result;
    }
}
