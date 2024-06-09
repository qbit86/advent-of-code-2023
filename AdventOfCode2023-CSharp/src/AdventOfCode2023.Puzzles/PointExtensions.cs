using System;
using System.Drawing;

namespace AdventOfCode2023;

internal static class PointExtensions
{
    internal static int ManhattanDistance(this Point self, Point other) =>
        Math.Abs(self.X - other.X) + Math.Abs(self.Y - other.Y);
}
