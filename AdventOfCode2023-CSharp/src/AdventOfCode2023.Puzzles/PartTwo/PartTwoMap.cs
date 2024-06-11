using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode2023;

internal static class PartTwoMap
{
    internal static PartTwoMap<TRows> Create<TRows>(TRows rows)
        where TRows : IReadOnlyList<string>
    {
        Debug.Assert(rows is not null);
        return new(rows);
    }
}

internal sealed class PartTwoMap<TRows> : MapBase<TRows>
    where TRows : IReadOnlyList<string>
{
    internal PartTwoMap(TRows rows) : base(rows) { }

    internal override IEnumerable<Point> GetOutNeighbors(Point point) => GetOutNeighborsIterator(point);
}
