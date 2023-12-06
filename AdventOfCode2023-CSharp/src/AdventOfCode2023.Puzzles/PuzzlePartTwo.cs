using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using static AdventOfCode2023.Helpers;

namespace AdventOfCode2023;

public static class PuzzlePartTwo
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
        long time = Parse(lines[0]);
        long distance = Parse(lines[1]);
        return CountWays(time, distance);
    }

    private static long Parse(string line)
    {
        StringBuilder builder = new();
        const int offset = 11;
        StringSegment valuesSegment = new(line, offset, line.Length - offset);
        StringTokenizer tokenizer = new(valuesSegment, s_separators);
        foreach (StringSegment segment in tokenizer)
        {
            if (segment is { Length: > 0 })
                builder.Append(segment.AsSpan());
        }

        return long.Parse(builder.ToString(), CultureInfo.InvariantCulture);
    }
}
