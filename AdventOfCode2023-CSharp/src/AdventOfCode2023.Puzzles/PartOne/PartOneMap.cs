using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<int>;

internal static class PartOneMap
{
    internal static PartOneMap<TRows> Create<TRows>(TRows rows)
        where TRows : IReadOnlyList<string>
    {
        Debug.Assert(rows is not null);
        return new(rows);
    }
}

internal sealed class PartOneMap<TRows> : MapBase<TRows>
    where TRows : IReadOnlyList<string>
{
    internal PartOneMap(TRows rows) : base(rows) { }

    internal override IEnumerable<Point> GetOutNeighbors(Point point)
    {
        if (!TryGetTile(point, out char tile))
            return GetOutNeighborsIterator(point);
        return tile switch
        {
            '>' => new[] { point + Vector2.UnitX<int>() }.Where(IsWithinBoundsAndWalkable),
            'v' => new[] { point + Vector2.UnitY<int>() }.Where(IsWithinBoundsAndWalkable),
            '<' => new[] { point + -Vector2.UnitX<int>() }.Where(IsWithinBoundsAndWalkable),
            '^' => new[] { point + -Vector2.UnitY<int>() }.Where(IsWithinBoundsAndWalkable),
            _ => GetOutNeighborsIterator(point)
        };
    }
}
