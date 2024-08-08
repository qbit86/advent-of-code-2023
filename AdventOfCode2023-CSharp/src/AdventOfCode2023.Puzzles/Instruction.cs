using System.Diagnostics;
using EuclideanSpace;

namespace AdventOfCode2023;

using Point = Point2<long>;

internal sealed class Instruction
{
    internal Instruction(char direction, long cubeCount)
    {
        Debug.Assert(cubeCount > 0);
        Direction = direction;
        CubeCount = cubeCount;
    }

    private char Direction { get; }
    private long CubeCount { get; }

    internal Segment CreateSegment(Point start) =>
        Direction switch
        {
            'R' => Segment.CreateHorizontal(start, CubeCount),
            'U' => Segment.CreateVertical(start, CubeCount),
            'L' => Segment.CreateHorizontal(start, -CubeCount),
            'D' => Segment.CreateVertical(start, -CubeCount),
            _ => default
        };
}
