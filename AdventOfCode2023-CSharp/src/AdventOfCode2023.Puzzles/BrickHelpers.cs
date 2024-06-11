using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal static class BrickHelpers
{
    internal static (
        List<Brick>?[] BricksBelowById, List<Brick>?[] BricksAboveById) ComputeBelowAbove<TBricks>(TBricks bricks)
        where TBricks : IReadOnlyList<Brick>
    {
        var bricksBelowById = new List<Brick>?[bricks.Count];
        var bricksAboveById = new List<Brick>?[bricks.Count];
        for (int i = 0; i < bricks.Count; ++i)
        {
            Brick left = bricks[i];
            for (int j = i + 1; j < bricks.Count; ++j)
            {
                Brick right = bricks[j];
                if (!Intersect(left, right))
                    continue;

                switch (left.LowerBoundZ.CompareTo(right.LowerBoundZ))
                {
                    case -1:
                    {
                        if (bricksBelowById[right.Id] is { } list)
                            list.Add(left);
                        else
                            bricksBelowById[right.Id] = new List<Brick> { left };
                    }
                    {
                        if (bricksAboveById[left.Id] is { } list)
                            list.Add(right);
                        else
                            bricksAboveById[left.Id] = new List<Brick> { right };
                    }
                        break;
                    case 1:
                    {
                        if (bricksBelowById[left.Id] is { } list)
                            list.Add(right);
                        else
                            bricksBelowById[left.Id] = new List<Brick> { right };
                    }
                    {
                        if (bricksAboveById[right.Id] is { } list)
                            list.Add(left);
                        else
                            bricksAboveById[right.Id] = new List<Brick> { left };
                    }
                        break;
                    default:
                        throw new ArgumentException(null, nameof(bricks));
                }
            }
        }

        return (bricksBelowById, bricksAboveById);
    }

    private static bool Intersect(Brick left, Brick right)
    {
        if (left.UpperBoundXInclusive < right.LowerBoundX)
            return false;
        if (left.UpperBoundYInclusive < right.LowerBoundY)
            return false;
        if (right.UpperBoundXInclusive < left.LowerBoundX)
            return false;
        if (right.UpperBoundYInclusive < left.LowerBoundY)
            return false;
        return true;
    }
}
