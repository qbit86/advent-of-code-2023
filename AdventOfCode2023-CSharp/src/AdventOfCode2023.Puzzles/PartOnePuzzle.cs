using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<long>;

public static class PartOnePuzzle
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        var instructions = lines.Select(Parse);
        List<Segment> segments = new();
        Point current = new();
        foreach (var instruction in instructions)
        {
            var segment = instruction.CreateSegment(current);
            segments.Add(segment);
            current = segment.EndInclusive;
        }

        return Helpers.Solve(segments);
    }

    private static Instruction Parse(string line)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(line));
        var lineSpan = line.AsSpan();
        Span<Range> ranges = stackalloc Range[3];
        int count = lineSpan.Split(ranges, ' ',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (count < 2)
            throw new InvalidOperationException($"{nameof(count)}: {count}");
        char direction = lineSpan[ranges[0]][0];
        int cubeCount = int.Parse(lineSpan[ranges[1]], CultureInfo.InvariantCulture);
        return new(direction, cubeCount);
    }
}
