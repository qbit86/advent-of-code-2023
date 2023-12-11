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
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IReadOnlyList<string> lines)
    {
        List<Point> galaxies = Helpers.CreateGalaxies(lines);
        var transformedGalaxies = Helpers.Expand(lines, 2, galaxies).ToList();
        return Helpers.ComputeSumOfDistances(transformedGalaxies);
    }
}
