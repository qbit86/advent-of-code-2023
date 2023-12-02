using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

internal sealed class Game
{
    private static readonly char[] s_separators = { ';' };

    private Game(int id, IReadOnlyList<Pick> picks)
    {
        Id = id;
        Picks = picks;
    }

    internal int Id { get; }

    internal IReadOnlyList<Pick> Picks { get; }

    internal static Game Parse(string line)
    {
        int indexOfSemicolon = line.IndexOf(':', StringComparison.InvariantCulture);
        if (indexOfSemicolon < 6)
        {
            throw new ArgumentException(
                $"{nameof(indexOfSemicolon)} should be at least 6, but was {indexOfSemicolon} for `{line}`.",
                nameof(line));
        }

        Range idRange = new(5, indexOfSemicolon);
        int id = int.Parse(line[idRange], CultureInfo.InvariantCulture);

        List<Pick> picks = new();
        StringSegment picksSegment = new(line, indexOfSemicolon + 1, line.Length - indexOfSemicolon - 1);
        StringTokenizer tokenizer = new(picksSegment, s_separators);
        foreach (StringSegment segment in tokenizer)
            picks.Add(Pick.Parse(segment));

        return new(id, picks);
    }
}
