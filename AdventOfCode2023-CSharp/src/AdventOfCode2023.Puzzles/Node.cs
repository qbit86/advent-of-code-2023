using System.Drawing;

namespace AdventOfCode2023;

internal readonly record struct Node(Point Position, Size Direction, int MoveCount)
{
    public override string ToString() =>
        $"{{Row: {Position.Y}, Column: {Position.X}, {nameof(Direction)}: {GetDirectionString()}, {nameof(MoveCount)}: {MoveCount}}}";

    private string GetDirectionString() =>
        Direction switch
        {
            { IsEmpty: true } => "·",
            { Width: > 0, Height: 0 } => "\u2192",
            { Width: 0, Height: > 0 } => "\u2193",
            { Width: < 0, Height: 0 } => "\u2190",
            { Width: 0, Height: < 0 } => "\u2191",
            _ => Direction.ToString()
        };
}
