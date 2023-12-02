using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PuzzlePartOne
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
        IEnumerable<Game> filteredGames = games.Where(IsPossible);
        return filteredGames.Select(game => game.Id).Sum();
    }

    private static bool IsPossible(Game game) => game.Picks
        .All(pick => pick is { RedCubeCount: <= 12, GreenCubeCount: <= 13, BlueCubeCount: <= 14 });
}
