using System;
using System.Collections.Generic;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<int>;

public static class PartOneMap
{
    public static PartOneMap<TRows> Create<TRows>(TRows rows) where TRows : IReadOnlyList<string>
    {
        if (rows is null)
            throw new ArgumentNullException(nameof(rows));

        return CreateUnchecked(rows);
    }

    internal static PartOneMap<TRows> CreateUnchecked<TRows>(TRows rows) where TRows : IReadOnlyList<string>
    {
        int rowCount = rows.Count;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            string row = rows[rowIndex];
            int columnIndex = row.IndexOf('S', StringComparison.Ordinal);
            if (columnIndex >= 0)
                return new(rows, new(columnIndex, rowIndex));
        }

        throw new ArgumentException(null, nameof(rows));
    }
}

public sealed class PartOneMap<TRows> : IMap
    where TRows : IReadOnlyList<string>
{
    private readonly TRows _rows;

    internal PartOneMap(TRows rows, Point start)
    {
        _rows = rows;
        Start = start;
    }

    public Point Start { get; }

    public bool IsWalkable(int row, int column)
    {
        if (unchecked((uint)row >= (uint)_rows.Count))
            return false;

        string line = _rows[row];
        if (unchecked((uint)column >= (uint)line.Length))
            return false;
        return line[column] switch
        {
            '.' or 'S' => true,
            '#' => false,
            var c => throw new InvalidOperationException(c.ToString())
        };
    }
}
