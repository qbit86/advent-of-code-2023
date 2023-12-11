using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace AdventOfCode2023;

internal static class Helpers
{
    internal static List<Point> CreateGalaxies<TRows>(TRows rows)
        where TRows : IReadOnlyList<string>
    {
        List<Point> galaxies = new();
        int rowCount = rows.Count;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            string row = rows[rowIndex];
            int columnCount = row.Length;
            int localRowIndex = rowIndex;
            IEnumerable<Point> points = Enumerable.Range(0, columnCount)
                .Where(columnIndex => row[columnIndex] is '#')
                .Select(columnIndex => new Point(localRowIndex, columnIndex));
            galaxies.AddRange(points);
        }

        return galaxies;
    }

    internal static IEnumerable<Point> Expand<TRows>(TRows rows, int factor, ICollection<Point> galaxies)
        where TRows : IReadOnlyList<string>
    {
        int columnCount = rows[0].Length;
        var emptyColumnIndices = Enumerable.Range(0, columnCount)
            .Except(galaxies.Select(it => it.Column)).ToList();

        int rowCount = rows.Count;
        var emptyRowIndices = Enumerable.Range(0, rowCount)
            .Except(galaxies.Select(it => it.Row)).ToList();

        return galaxies.Select(TransformGalaxy);

        Point TransformGalaxy(Point galaxy)
        {
            int rowSearchResult = CollectionsMarshal.AsSpan(emptyRowIndices).BinarySearch(galaxy.Row);
            Debug.Assert(rowSearchResult < 0);
            int bottom = ~rowSearchResult;
            int newRowIndex = galaxy.Row + (bottom - 1) * (factor - 1);

            int columnSearchResult = CollectionsMarshal.AsSpan(emptyColumnIndices).BinarySearch(galaxy.Column);
            Debug.Assert(columnSearchResult < 0);
            int right = ~columnSearchResult;
            int newColumnIndex = galaxy.Column + (right - 1) * (factor - 1);

            return new(newRowIndex, newColumnIndex);
        }
    }

    internal static long ComputeSumOfDistances<TGalaxies>(TGalaxies galaxies)
        where TGalaxies : IReadOnlyList<Point>
    {
        List<int> distances = new();
        int galaxyCount = galaxies.Count;
        for (int i = 0; i < galaxyCount; ++i)
        {
            for (int j = i + 1; j < galaxyCount; ++j)
                distances.Add(Distance(galaxies[i], galaxies[j]));
        }

        return distances.Sum(Convert.ToInt64);
    }

    private static int Distance(Point left, Point right) =>
        Math.Abs(left.Row - right.Row) + Math.Abs(left.Column - right.Column);
}
