using System.Collections.Generic;
using System.Diagnostics;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<int>;

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
