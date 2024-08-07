using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2023;

internal sealed class Output
{
    private readonly char[][] _rows;
    private string _stringRepresentation = string.Empty;

    private Output(char[][] rows)
    {
        Debug.Assert(rows is not null);
        _rows = rows;
    }

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(_stringRepresentation))
            return _stringRepresentation;
        var lines = _rows.Select(it => new string(it));
        return _stringRepresentation = string.Join(Environment.NewLine, lines);
    }

    internal static Output Create<TRows>(TRows lines)
        where TRows : IReadOnlyList<string>
    {
        Debug.Assert(lines is not null);
        int rowCount = lines.Count;
        char[][] rows = new char[rowCount][];
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
            rows[rowIndex] = lines[rowIndex].ToCharArray();
        return new(rows);
    }

    internal void SetUnchecked(Point position, char value)
    {
        _rows[position.Y][position.X] = value;
        _stringRepresentation = string.Empty;
    }

    internal char Get(Point position) => _rows[position.Y][position.X];
}
