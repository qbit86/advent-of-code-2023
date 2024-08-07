using System;

namespace AdventOfCode2023;

internal readonly record struct Segment(Point Start, Point EndInclusive)
{
    internal static Segment CreateHorizontal(Point start, long length)
    {
        var endInclusive = start with { X = start.X + length };
        return new(start, endInclusive);
    }

    internal static Segment CreateVertical(Point start, long length)
    {
        var endInclusive = start with { Y = start.Y + length };
        return new(start, endInclusive);
    }

    internal long Length()
    {
        long h = Math.Abs(EndInclusive.X - Start.X);
        long v = Math.Abs(EndInclusive.Y - Start.Y);
        return Math.Max(h, v);
    }
}
