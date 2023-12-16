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
        string line = await File.ReadAllTextAsync(path, Encoding.UTF8).ConfigureAwait(false);
        string[] steps = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return Solve(steps);
    }

    private static long Solve(IEnumerable<string> steps) => steps.Select(Helpers.Hash).Select(long.CreateChecked).Sum();
}
