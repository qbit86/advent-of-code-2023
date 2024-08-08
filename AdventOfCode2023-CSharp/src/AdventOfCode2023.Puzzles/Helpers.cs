using System.Collections.Generic;
using EuclideanSpace;

namespace AdventOfCode2023;

using Vector = Vector2<int>;

public static class Helpers
{
    private static readonly Vector[] s_directions = [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];

    internal static IReadOnlyList<Vector> Directions => s_directions;
}
