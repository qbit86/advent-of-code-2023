using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2023;

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
        IEnumerable<long> lengths = segments.Select(segment => segment.Length());
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
}
