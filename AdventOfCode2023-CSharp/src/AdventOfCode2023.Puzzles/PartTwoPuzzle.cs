using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence;
using Arborescence.Traversal;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        var brickById = new Brick[lines.Count];
        for (int i = 0; i < lines.Count; ++i)
            brickById[i] = Brick.Parse(i, lines[i]);

        (List<Brick>?[] bricksBelowById, List<Brick>?[] bricksAboveById) = BrickHelpers.ComputeBelowAbove(brickById);
        var aboveGraph = AboveGraph.Create(bricksAboveById);

        List<Brick> topologicallyOrderedBricks = new(brickById.Length);
        DfsHandler<Brick, Endpoints<Brick>, AboveGraph<List<Brick>?[]>> handler = new();
        handler.FinishVertex += (_, v) => topologicallyOrderedBricks.Add(v);
        EagerDfs<Brick, List<Brick>.Enumerator>.Traverse(aboveGraph, brickById, handler);
        topologicallyOrderedBricks.Reverse();
        foreach (Brick brick in topologicallyOrderedBricks)
        {
            if (bricksBelowById[brick.Id] is not { Count: > 0 } bricksBelow)
            {
                brick.LowerBoundZ = 1;
                continue;
            }

            int upperBoundZInclusive = bricksBelow.Max(b => b.UpperBoundZInclusive);
            brick.LowerBoundZ = upperBoundZInclusive + 1;
        }

        return Enumerable.Range(0, topologicallyOrderedBricks.Count).Sum(CountFallingBricks);

        int CountFallingBricks(int disintegratingBrickIndex)
        {
            int[] lowerBoundZById = ArrayPool<int>.Shared.Rent(topologicallyOrderedBricks.Count);
            try
            {
                Array.Fill(lowerBoundZById, short.MinValue);
                foreach (Brick brick in topologicallyOrderedBricks)
                    lowerBoundZById[brick.Id] = brick.LowerBoundZ;

                Brick disintegratingBrick = topologicallyOrderedBricks[disintegratingBrickIndex];
                int count = 0;
                for (int j = disintegratingBrickIndex + 1; j < topologicallyOrderedBricks.Count; ++j)
                {
                    Brick brick = topologicallyOrderedBricks[j];
                    if (bricksBelowById[brick.Id] is not { Count: > 0 } bricksBelow
                        || bricksBelow is { Count: 1 } && bricksBelow[0] == disintegratingBrick)
                    {
                        if (lowerBoundZById[brick.Id] is not 1)
                        {
                            lowerBoundZById[brick.Id] = 1;
                            count += 1;
                        }

                        continue;
                    }

                    int upperBoundZInclusive = bricksBelow.Where(b => b != disintegratingBrick)
                        .Max(b => lowerBoundZById[b.Id] + b.LengthZInclusive);
                    if (lowerBoundZById[brick.Id] != upperBoundZInclusive + 1)
                    {
                        lowerBoundZById[brick.Id] = upperBoundZInclusive + 1;
                        count += 1;
                    }
                }

                return count;
            }
            finally
            {
                ArrayPool<int>.Shared.Return(lowerBoundZById);
            }
        }
    }
}
