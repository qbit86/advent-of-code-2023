using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<long>;

internal static class Helpers
{
    internal static long Solve(ICollection<Segment> segments)
    {
        long boundaryPoints = ComputeBoundaryPoints(segments);
        long interiorPoints = ComputeInteriorPoints(segments, boundaryPoints);
        return boundaryPoints + interiorPoints;
    }

    private static long ComputeBoundaryPoints(IEnumerable<Segment> segments)
    {
        var lengths = segments.Select(segment => segment.Length());
        return lengths.Sum();
    }

    private static long ComputeInteriorPoints(IEnumerable<Segment> segments, long boundaryPoints)
    {
        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        Debug.Assert(long.IsEvenInteger(boundaryPoints));
        long area = ComputeArea(segments);
        return area - boundaryPoints / 2 + 1;
    }

    private static long ComputeArea(IEnumerable<Segment> segments)
    {
        // https://en.wikipedia.org/wiki/Shoelace_formula
        long sum = segments.Select(segment => segment.Start.Cross(segment.EndInclusive)).Sum();
        Debug.Assert(long.IsEvenInteger(sum));
        return Math.Abs(sum) / 2;
    }

    private static long Cross(this Point left, Point right) =>
        Vector2.Cross(left.AsVector2(), right.AsVector2());
}
