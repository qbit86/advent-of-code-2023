using System;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<long>;
using Vector = Vector2<long>;

internal readonly record struct Segment(Point Start, Point EndInclusive)
{
    internal static Segment CreateHorizontal(Point start, long length)
    {
        var endInclusive = start + new Vector(length, 0L);
        return new(start, endInclusive);
    }

    internal static Segment CreateVertical(Point start, long length)
    {
        var endInclusive = start + new Vector(0L, length);
        return new(start, endInclusive);
    }

    internal long Length()
    {
        long h = Math.Abs(EndInclusive.X - Start.X);
        long v = Math.Abs(EndInclusive.Y - Start.Y);
        return Math.Max(h, v);
    }
}
