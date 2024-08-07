using EuclideanSpace;

namespace AdventOfCode2023;

using V2 = Vector2<int>;

internal static class SizeExtensions
{
    internal static V2 RotateRight(this V2 direction) => new(direction.Y, -direction.X);

    internal static V2 RotateLeft(this V2 direction) => new(-direction.Y, direction.X);
}
