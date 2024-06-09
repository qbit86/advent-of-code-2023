using System.Drawing;

namespace AdventOfCode2023;

public interface IMap
{
    bool IsWalkable(Point tile) => IsWalkable(tile.Y, tile.X);

    bool IsWalkable(int row, int column);
}
