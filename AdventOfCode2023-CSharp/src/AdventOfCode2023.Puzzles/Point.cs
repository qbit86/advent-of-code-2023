namespace AdventOfCode2023;

internal readonly record struct Point(long X, long Y);

internal static class PointExtensions
{
    internal static long Cross(this Point left, Point right) => left.X * right.Y - left.Y * right.X;
}
