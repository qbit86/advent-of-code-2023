using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public static class PuzzlePartTwo
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
        FrozenDictionary<Point, FrozenSet<Point>> asterisksByNeighbor = GetAsterisksByNeighbor(lines);
        char[] separators = ParsingHelpers.Separators.ToCharArray();
        Dictionary<Point, HashSet<StringSegment>> numbersByAsterisk = new();
        for (int rowIndex = 0; rowIndex < lines.Count; rowIndex++)
        {
            StringTokenizer tokenizer = new(lines[rowIndex], separators);
            foreach (StringSegment segment in tokenizer)
            {
                if (segment.Length is 0)
                    continue;
                int offset = segment.Offset;
                int upperBound = offset + segment.Length;
                for (int columnIndex = offset; columnIndex < upperBound; ++columnIndex)
                {
                    Point currentPosition = new(rowIndex, columnIndex);
                    if (!asterisksByNeighbor.TryGetValue(currentPosition, out FrozenSet<Point>? asterisks))
                        continue;
                    foreach (Point asterisk in asterisks)
                    {
                        if (numbersByAsterisk.TryGetValue(asterisk, out HashSet<StringSegment>? numbers))
                            numbers.Add(segment);
                        else
                            numbersByAsterisk.Add(asterisk, new() { segment });
                    }
                }
            }
        }

        IEnumerable<KeyValuePair<Point, HashSet<StringSegment>>> gearNumbersPair =
            numbersByAsterisk.Where(kv => kv.Value.Count is 2);

        IEnumerable<long> ratios = gearNumbersPair.Select(kv => kv.Value.Aggregate(1L, Fold));
        return ratios.Sum();

        static long Fold(long left, StringSegment right)
        {
            return left * long.Parse(right, CultureInfo.InvariantCulture);
        }
    }

    private static FrozenDictionary<Point, FrozenSet<Point>> GetAsterisksByNeighbor<TRowCollection>(
        TRowCollection rows)
        where TRowCollection : IReadOnlyList<string>
    {
        List<KeyValuePair<Point, Point>> neighborAsteriskPairs = new();
        int rowCount = rows.Count;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            string row = rows[rowIndex];
            int columnCount = row.Length;
            for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
            {
                if (rows[rowIndex][columnIndex] is not '*')
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

                        Point neighbor = new(neighborRowIndex, neighborColumnIndex);
                        Point asterisk = new(rowIndex, columnIndex);
                        neighborAsteriskPairs.Add(new(neighbor, asterisk));
                    }
                }
            }
        }

        ILookup<Point, Point> asterisksByNeighbor = neighborAsteriskPairs.ToLookup(kv => kv.Key, kv => kv.Value);
        return asterisksByNeighbor.ToFrozenDictionary(grouping => grouping.Key, grouping => grouping.ToFrozenSet());
    }
}
