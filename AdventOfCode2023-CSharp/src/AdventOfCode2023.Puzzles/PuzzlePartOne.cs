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
        IEnumerable<Card> cards = lines.Select(Card.Parse);
        IEnumerable<IEnumerable<int>> intersections =
            cards.Select(card => card.WinningNumbers.Intersect(card.ActualNumbers));
        IEnumerable<int> winningCounts = intersections.Select(it => it.Count()).Where(it => it > 0);
        IEnumerable<long> scores = winningCounts.Select(it => 1L << (it - 1));
        return scores.Sum();
    }
}
