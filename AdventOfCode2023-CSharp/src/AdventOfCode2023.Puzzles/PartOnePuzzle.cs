using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartOnePuzzle
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

    private static long SolveSingle<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        if (Helpers.TryFindHorizontalAxis(lines, out int horizontalAxisPosition))
            return 100L * horizontalAxisPosition;
        if (Helpers.TryFindVerticalAxis(lines, out int verticalAxisPosition))
            return verticalAxisPosition;
        return 0L;
    }
}
