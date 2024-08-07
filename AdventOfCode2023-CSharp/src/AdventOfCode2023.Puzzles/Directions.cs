using EuclideanSpace;

namespace AdventOfCode2023;

using V2 = Vector2<int>;

internal static class Directions
{
    internal static V2 Right { get; } = Vector2.UnitX<int>();
    internal static V2 Down { get; } = Vector2.UnitY<int>();
    internal static V2 Left { get; } = -Vector2.UnitX<int>();
    internal static V2 Up { get; } = -Vector2.UnitY<int>();

    internal static bool IsHorizontal(V2 direction) => direction.X is not 0 && direction.Y is 0;

    internal static bool IsVertical(V2 direction) => direction.X is 0 && direction.Y is not 0;

    internal static V2 GetNewDirectionForForwardMirror(V2 direction)
    {
        if (IsHorizontal(direction))
            return new(direction.Y, -direction.X);
        if (IsVertical(direction))
            return new(-direction.Y, direction.X);
        return default;
    }

    internal static V2 GetNewDirectionForBackwardMirror(V2 direction)
    {
        if (IsHorizontal(direction))
            return new(-direction.Y, direction.X);
        if (IsVertical(direction))
            return new(direction.Y, -direction.X);
        return default;
    }
}
