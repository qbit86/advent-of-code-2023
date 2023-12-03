using System;
using System.Buffers;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public static class PuzzlePartOne
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLineCollection>(TLineCollection lines)
        where TLineCollection : IReadOnlyList<string>
    {
        FrozenSet<Point> symbolsNeighbors = GetSymbolsNeighbors(lines);
        char[] separators = ParsingHelpers.Separators.ToCharArray();
        List<StringSegment> segments = new();
        for (int rowIndex = 0; rowIndex < lines.Count; rowIndex++)
        {
            string row = lines[rowIndex];
            StringTokenizer tokenizer = new(row, separators);
            foreach (StringSegment segment in tokenizer)
            {
                if (segment.Length is 0)
                    continue;
                int offset = segment.Offset;
                int upperBound = offset + segment.Length;
                for (int columnIndex = offset; columnIndex < upperBound; ++columnIndex)
                {
                    Point currentPosition = new(rowIndex, columnIndex);
                    if (symbolsNeighbors.Contains(currentPosition))
                    {
                        segments.Add(segment);
                        break;
                    }
                }
            }
        }

        IEnumerable<long> partNumbers = segments.Select(segment => long.Parse(segment, CultureInfo.InvariantCulture));
        return partNumbers.Sum();
    }

    private static FrozenSet<Point> GetSymbolsNeighbors<TRowCollection>(TRowCollection rows)
        where TRowCollection : IReadOnlyList<string>
    {
        var symbolSearchValues = SearchValues.Create(ParsingHelpers.Symbols);
        List<Point> neighbors = new();
        int rowCount = rows.Count;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            string row = rows[rowIndex];
            int columnCount = row.Length;
            for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
            {
                char c = rows[rowIndex][columnIndex];
                if (!symbolSearchValues.Contains(c))
                    continue;

                for (int i = -1; i < 2; ++i)
                {
                    int neighborRowIndex = rowIndex + i;
                    if (unchecked((uint)neighborRowIndex >= rowCount))
                        continue;
                    for (int j = -1; j < 2; ++j)
                    {
                        int neighborColumnIndex = columnIndex + j;
                        if (unchecked((uint)neighborColumnIndex >= columnCount))
                            continue;

                        if (i is 0 && j is 0)
                            continue;

                        neighbors.Add(new(neighborRowIndex, neighborColumnIndex));
                    }
                }
            }
        }

        return neighbors.ToFrozenSet();
    }
}
