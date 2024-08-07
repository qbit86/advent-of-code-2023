using EuclideanSpace;

namespace AdventOfCode2023;

using P2 = Point2<int>;
using V2 = Vector2<int>;

internal readonly record struct Node(P2 Position, V2 Direction, int MoveCount)
{
    public override string ToString() =>
        $"{{Row: {Position.Y}, Column: {Position.X}, {nameof(Direction)}: {GetDirectionString()}, {nameof(MoveCount)}: {MoveCount}}}";

    private string GetDirectionString() =>
        Direction switch
        {
            { X: 0, Y: 0 } => "·",
            { X: > 0, Y: 0 } => "\u2192",
            { X: 0, Y: > 0 } => "\u2193",
            { X: < 0, Y: 0 } => "\u2190",
            { X: 0, Y: < 0 } => "\u2191",
            _ => Direction.ToString()
        };
}
