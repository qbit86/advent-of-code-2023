using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
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
        Span<Range> ranges = stackalloc Range[4];
        int count = lineSpan.SplitAny(ranges, " (#)",
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (count is 0)
            throw new InvalidOperationException($"{nameof(count)}: 0");
        var colorCode = lineSpan[ranges[2]];
        if (colorCode.Length is not 6)
            throw new InvalidOperationException($"{nameof(colorCode)}: {new string(colorCode)}");

        char direction = DecodeDirection(colorCode[^1]);
        long cubeCount = long.Parse(colorCode[..^1], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        return new(direction, cubeCount);
    }

    private static char DecodeDirection(char c) => c switch
    {
        '0' => 'R',
        '1' => 'D',
        '2' => 'L',
        '3' => 'U',
        _ => c
    };
}
