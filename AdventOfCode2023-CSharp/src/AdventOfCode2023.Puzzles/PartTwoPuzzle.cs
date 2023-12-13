using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
        string text = await File.ReadAllTextAsync(path, Encoding.UTF8).ConfigureAwait(false);
        string[] texts = Helpers.SplitByBlankLines(text);
        return SolveAll(texts);
    }

    private static long SolveAll(IEnumerable<string> texts)
    {
        IEnumerable<string[]> grids = texts.Select(Helpers.SplitByNewLines);
        IEnumerable<long> summaries = grids.Select(SolveSingle);
        return summaries.Sum();
    }

    private static long SolveSingle(string[] lines)
    {
        if (Helpers.TryFindHorizontalAxis(lines, out int horizontalAxisPosition))
            return FixHorizontalAxis(lines, horizontalAxisPosition);
        if (Helpers.TryFindVerticalAxis(lines, out int verticalAxisPosition))
            return FixVerticalAxis(lines, verticalAxisPosition);
        return 0L;
    }

    private static long FixHorizontalAxis(string[] lines, int oldHorizontalAxisPosition)
    {
        var originalLines = ImmutableList.Create(lines);
        int rowCount = lines.Length;
        (int offset, int count) = (0, rowCount);
        int upperBound = offset + count;
        for (int rowIndex = offset; rowIndex < upperBound; ++rowIndex)
        {
            string row = lines[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
            {
                char[] chars = row.ToCharArray();
                chars[columnIndex] = Flip(chars[columnIndex]);
                ImmutableList<string> modifiedLines = originalLines.SetItem(rowIndex, new(chars));
                if (Helpers.TryFindHorizontalAxis(modifiedLines, oldHorizontalAxisPosition, out int h))
                    return 100L * h;
                if (Helpers.TryFindVerticalAxis(modifiedLines, out int v))
                    return v;
            }
        }

        throw new UnreachableException();
    }

    private static long FixVerticalAxis(string[] lines, int oldVerticalAxisPosition)
    {
        var originalLines = ImmutableList.Create(lines);
        int rowCount = lines.Length;
        Debug.Assert(lines.Length > 0);
        int columnCount = lines[0].Length;
        (int offset, int count) = (0, columnCount);
        int upperBound = offset + count;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            string row = lines[rowIndex];
            for (int columnIndex = offset; columnIndex < upperBound; ++columnIndex)
            {
                char[] chars = row.ToCharArray();
                chars[columnIndex] = Flip(chars[columnIndex]);
                ImmutableList<string> modifiedLines = originalLines.SetItem(rowIndex, new(chars));
                if (Helpers.TryFindHorizontalAxis(modifiedLines, out int h))
                    return 100L * h;
                if (Helpers.TryFindVerticalAxis(modifiedLines, oldVerticalAxisPosition, out int v))
                    return v;
            }
        }

        throw new UnreachableException();
    }

    private static char Flip(char c) =>
        c switch { '.' => '#', '#' => '.', _ => throw new ArgumentException(c.ToString(), nameof(c)) };
}
