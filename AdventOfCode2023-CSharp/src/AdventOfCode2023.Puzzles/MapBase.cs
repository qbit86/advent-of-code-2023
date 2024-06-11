using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2023;

internal abstract class MapBase<TRows>
    where TRows : IReadOnlyList<string>
{
    private TRows _rows;

    internal MapBase(TRows rows)
    {
        Debug.Assert(rows is not null);
        _rows = rows;
    }

    internal int RowCount => _rows.Count;

    internal int GetColumnCount(int rowIndex)
    {
        Debug.Assert((uint)rowIndex < (uint)_rows.Count);
        return _rows[rowIndex].Length;
    }

    internal abstract IEnumerable<Point> GetOutNeighbors(Point point);

    protected IEnumerable<Point> GetOutNeighborsIterator(Point point) =>
        Helpers.Directions.Select(direction => point + direction).Where(IsWithinBoundsAndWalkable);

    internal char GetTile(Point point) => GetTile(point.Y, point.X);

    internal char GetTile(int rowIndex, int columnIndex)
    {
        Debug.Assert((uint)rowIndex < (uint)_rows.Count);
        Debug.Assert(columnIndex >= 0);
        string row = _rows[rowIndex];
        Debug.Assert(columnIndex < row.Length);
        return row[columnIndex];
    }

    protected bool TryGetTile(Point point, out char tile) => TryGetTile(point.Y, point.X, out tile);

    private bool TryGetTile(int rowIndex, int columnIndex, out char tile)
    {
        if (unchecked((uint)rowIndex >= (uint)_rows.Count))
            return None(out tile);

        string row = _rows[rowIndex];
        if (unchecked((uint)columnIndex >= (uint)row.Length))
            return None(out tile);

        tile = row[columnIndex];
        return true;
    }

    protected bool IsWithinBoundsAndWalkable(Point point) => IsWithinBoundsAndWalkable(point.Y, point.X);

    private bool IsWithinBoundsAndWalkable(int rowIndex, int columnIndex) =>
        TryGetTile(rowIndex, columnIndex, out char tile) && tile is not '#';

    private static bool None(out char value)
    {
        value = default;
        return false;
    }
}
