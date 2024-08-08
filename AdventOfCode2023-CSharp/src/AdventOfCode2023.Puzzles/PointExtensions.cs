using System;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<int>;

internal static class PointExtensions
{
    internal static int ManhattanDistance(this Point self, Point other) =>
        Math.Abs(self.X - other.X) + Math.Abs(self.Y - other.Y);
}
