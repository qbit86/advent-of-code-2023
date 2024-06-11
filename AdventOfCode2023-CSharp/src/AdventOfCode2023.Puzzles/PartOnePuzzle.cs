using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence;
using Arborescence.Traversal;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2023;

public static class PartOnePuzzle
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

        int count = 0;
        foreach (Brick brick in topologicallyOrderedBricks)
        {
            if (bricksAboveById[brick.Id] is not { Count: > 0 } bricksAbove)
            {
                count += 1;
                continue;
            }

            bool canDisintegrate = true;
            foreach (Brick brickAbove in bricksAbove)
            {
                IEnumerable<Brick> siblings = bricksBelowById[brickAbove.Id]!.Where(b => b.Id != brick.Id);
                bool hasOtherSupporters = siblings.Any(b => b.UpperBoundZInclusive >= brick.UpperBoundZInclusive);
                if (!hasOtherSupporters)
                {
                    canDisintegrate = false;
                    break;
                }
            }

            if (canDisintegrate)
                count += 1;
        }

        return count;
    }
}
