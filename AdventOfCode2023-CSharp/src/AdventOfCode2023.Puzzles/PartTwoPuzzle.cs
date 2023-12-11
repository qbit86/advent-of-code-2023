using System;
using System.Collections.Generic;
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
        return Solve(lines, 1000000);
    }

    public static async Task<long> SolveAsync(string path, int factor)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(factor);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines, factor);
    }

    private static long Solve(IReadOnlyList<string> lines, int factor)
    {
        List<Point> galaxies = Helpers.CreateGalaxies(lines);
        var transformedGalaxies = Helpers.Expand(lines, factor, galaxies).ToList();
        return Helpers.ComputeSumOfDistances(transformedGalaxies);
    }
}
