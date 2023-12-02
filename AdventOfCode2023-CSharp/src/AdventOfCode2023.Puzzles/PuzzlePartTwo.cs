using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PuzzlePartTwo
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IEnumerable<string> lines)
    {
        IEnumerable<Game> games = lines.Select(Game.Parse);
        IEnumerable<Pick> fewestNumbers = games.Select(GetFewest);
        IEnumerable<long> powers =
            fewestNumbers.Select(it => (long)it.RedCubeCount * it.GreenCubeCount * it.BlueCubeCount);
        return powers.Sum();
    }

    private static Pick GetFewest(Game game) => game.Picks.Aggregate(Combine);

    private static Pick Combine(Pick left, Pick right) => new(
        Math.Max(left.RedCubeCount, right.RedCubeCount),
        Math.Max(left.GreenCubeCount, right.GreenCubeCount),
        Math.Max(left.BlueCubeCount, right.BlueCubeCount));
}
