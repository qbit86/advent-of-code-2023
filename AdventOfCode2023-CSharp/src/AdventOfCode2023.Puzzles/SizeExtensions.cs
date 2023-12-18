using System.Drawing;

namespace AdventOfCode2023;

internal static class SizeExtensions
{
    internal static Size RotateRight(this Size direction) => new(direction.Height, -direction.Width);

    internal static Size RotateLeft(this Size direction) => new(-direction.Height, direction.Width);
}
