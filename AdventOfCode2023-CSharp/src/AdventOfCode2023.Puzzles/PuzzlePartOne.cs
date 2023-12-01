using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PuzzlePartOne
{
    private static readonly SearchValues<char> s_digits = SearchValues.Create("0123456789");

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IEnumerable<string> lines)
    {
        IEnumerable<long> calibrationValues = lines.Select(Decode);
        return calibrationValues.Sum();
    }

    private static long Decode(string line)
    {
        ReadOnlySpan<char> span = line.AsSpan();
        int firstIndex = span.IndexOfAny(s_digits);
        int leftDigit = line[firstIndex] - '0';
        int lastIndex = span.LastIndexOfAny(s_digits);
        int rightDigit = line[lastIndex] - '0';
        return leftDigit * 10 + rightDigit;
    }
}
