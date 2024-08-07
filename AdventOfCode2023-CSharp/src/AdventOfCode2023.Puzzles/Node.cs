using EuclideanSpace;

namespace AdventOfCode2023;

internal readonly record struct Node(Point2<int> Position, Vector2<int> Direction)
{
    public override string ToString() =>
        $"{{Row: {Position.Y}, Column: {Position.X}, {nameof(Direction)}: {GetDirectionString()}}}";

    private string GetDirectionString()
    {
        if (Direction == Directions.Right)
            return nameof(Directions.Right);
        if (Direction == Directions.Down)
            return nameof(Directions.Down);
        if (Direction == Directions.Left)
            return nameof(Directions.Left);
        if (Direction == Directions.Up)
            return nameof(Directions.Up);
        return Direction.ToString();
    }
}
