using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2023;

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
            '>' => new[] { point with { X = point.X + 1 } }.Where(IsWithinBoundsAndWalkable),
            'v' => new[] { point with { Y = point.Y + 1 } }.Where(IsWithinBoundsAndWalkable),
            '<' => new[] { point with { X = point.X - 1 } }.Where(IsWithinBoundsAndWalkable),
            '^' => new[] { point with { Y = point.Y - 1 } }.Where(IsWithinBoundsAndWalkable),
            _ => GetOutNeighborsIterator(point)
        };
    }
}
