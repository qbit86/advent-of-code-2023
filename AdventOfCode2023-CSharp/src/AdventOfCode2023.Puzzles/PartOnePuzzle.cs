using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence;
using Arborescence.Models;
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

    private static long Solve(string[] lines)
    {
        Dictionary<Point, List<Point>> neighborsByVertex = new();
        bool startingPositionFound =
            Helpers.PopulateNeighborsByVertex(lines, neighborsByVertex, out Point startingPosition);
        if (!startingPositionFound)
            throw new ArgumentException($"{nameof(startingPosition)}: false", nameof(lines));
        ListAdjacencyGraph<Point, Dictionary<Point, List<Point>>> graph =
            ListAdjacencyGraphFactory<Point>.Create(neighborsByVertex);
        Dictionary<Point, int> distanceByPosition = new() { { startingPosition, 0 } };
        IEnumerable<Endpoints<Point>> edges =
            EnumerableBfs<Point, List<Point>.Enumerator>.EnumerateEdges(graph, startingPosition);
        foreach (Endpoints<Point> edge in edges)
        {
            if (distanceByPosition.TryGetValue(edge.Tail, out int d))
                distanceByPosition.Add(edge.Head, d + 1);
            else
                throw new UnreachableException();
        }

        int result = distanceByPosition.Max(it => it.Value);
        return result;
    }
}
