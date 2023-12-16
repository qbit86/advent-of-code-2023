using System.Drawing;

namespace AdventOfCode2023;

internal static class Directions
{
    internal static Size Right { get; } = new(1, 0);
    internal static Size Down { get; } = new(0, 1);
    internal static Size Left { get; } = new(-1, 0);
    internal static Size Up { get; } = new(0, -1);

    internal static bool IsHorizontal(Size direction) => direction.Width is not 0 && direction.Height is 0;

    internal static bool IsVertical(Size direction) => direction.Width is 0 && direction.Height is not 0;

    internal static Size GetNewDirectionForForwardMirror(Size direction)
    {
        if (IsHorizontal(direction))
            return new(direction.Height, -direction.Width);
        if (IsVertical(direction))
            return new(-direction.Height, direction.Width);
        return default;
    }

    internal static Size GetNewDirectionForBackwardMirror(Size direction)
    {
        if (IsHorizontal(direction))
            return new(-direction.Height, direction.Width);
        if (IsVertical(direction))
            return new(direction.Height, -direction.Width);
        return default;
    }
}
