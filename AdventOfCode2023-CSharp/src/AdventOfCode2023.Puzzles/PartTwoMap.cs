using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace AdventOfCode2023;

internal static class PartTwoMap
{
    internal static PartTwoMap<TRows> Create<TRows>(TRows rows, int radius) where TRows : IReadOnlyList<string>
    {
        int rowCount = rows.Count;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            string row = rows[rowIndex];
            int columnIndex = row.IndexOf('S', StringComparison.Ordinal);
            if (columnIndex >= 0)
                return new(rows, new(columnIndex, rowIndex), radius);
        }

        throw new ArgumentException(null, nameof(rows));
    }
}

internal sealed class PartTwoMap<TRows> : IMap
    where TRows : IReadOnlyList<string>
{
    private readonly int _radius;
    private readonly TRows _rows;

    internal PartTwoMap(TRows rows, Point start, int radius)
    {
        _rows = rows;
        _radius = radius;
        Start = start;
    }

    internal Point Start { get; }

    public bool IsWalkable(int row, int column)
    {
        if (new Point(column, row).ManhattanDistance(Start) > _radius)
            return false;
        int rowIndex = Mod(row, _rows.Count);
        string line = _rows[rowIndex];
        int columnIndex = Mod(column, line.Length);
        return line[columnIndex] switch
        {
            '.' or 'S' => true,
            '#' => false,
            var c => throw new InvalidOperationException(c.ToString())
        };
    }

    private static T Mod<T>(T dividend, T divisor)
        where T : IAdditionOperators<T, T, T>, IModulusOperators<T, T, T> =>
        (dividend % divisor + divisor) % divisor;
}
