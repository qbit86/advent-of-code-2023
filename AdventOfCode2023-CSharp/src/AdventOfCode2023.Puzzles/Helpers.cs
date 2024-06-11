using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2023;

public static class Helpers
{
    private static readonly Size[] s_directions = [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];

    internal static IReadOnlyList<Size> Directions => s_directions;
}
