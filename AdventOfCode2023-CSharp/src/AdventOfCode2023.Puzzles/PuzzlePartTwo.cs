using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence;

namespace AdventOfCode2023;

using CardCountById = Int32Dictionary<long, long[], EqualityComparer<long>>;

public static class PuzzlePartTwo
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(ICollection<string> lines)
    {
        long[] arrayFromPool = ArrayPool<long>.Shared.Rent(lines.Count);
        arrayFromPool.AsSpan(0, lines.Count).Fill(int.MinValue);
        CardCountById cardCountById = Int32DictionaryFactory<long>.CreateWithAbsence(arrayFromPool, int.MinValue);

        IEnumerable<Card> cards = lines.Select(Card.Parse);
        IEnumerable<IEnumerable<int>> intersections =
            cards.Select(card => card.WinningNumbers.Intersect(card.ActualNumbers));
        var winningNumberCountById = intersections.Select(it => it.Count()).ToList();
        Graph graph = new(winningNumberCountById);
        long accumulator = 0L;
        for (int i = 0; i < winningNumberCountById.Count; ++i)
            accumulator += GetCardCount(cardCountById, graph, i);

        ArrayPool<long>.Shared.Return(arrayFromPool);
        return accumulator;
    }

    private static long GetCardCount(CardCountById cardCountById, Graph graph, int cardId)
    {
        if (cardCountById.TryGetValue(cardId, out long cardCount))
            return cardCount;
        cardCount = 1L + graph.EnumerateOutNeighbors(cardId).Sum(neighbor => GetCardCount(cardCountById, graph, neighbor));
        cardCountById[cardId] = cardCount;
        return cardCount;
    }
}
