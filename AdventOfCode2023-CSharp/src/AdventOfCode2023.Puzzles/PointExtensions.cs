using System;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<int>;

internal static class PointExtensions
{
    internal static int ManhattanDistance(this Point point, Point other) =>
        Math.Abs(other.X - point.X) + Math.Abs(other.Y - point.Y);
}
