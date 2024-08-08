using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<int>;

public interface IMap
{
    bool IsWalkable(Point tile) => IsWalkable(tile.Y, tile.X);

    bool IsWalkable(int row, int column);
}
