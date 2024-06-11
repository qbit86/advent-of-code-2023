using System;
using System.Drawing;

namespace AdventOfCode2023;

internal static class PointExtensions
{
    internal static int ManhattanDistance(this Point point, Point other) =>
        Math.Abs(other.X - point.X) + Math.Abs(other.Y - point.Y);
}
