using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence.Models;
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

    private static long Solve(string[] lines)
    {
        Dictionary<Point, List<Point>> neighborsByVertex = new();
        bool startingPositionFound =
            Helpers.PopulateNeighborsByVertex(lines, neighborsByVertex, out Point startingPosition);
        if (!startingPositionFound)
            throw new ArgumentException($"{nameof(startingPosition)}: false", nameof(lines));
        var graph = ListAdjacencyGraph<Point>.Create(neighborsByVertex);
        var edges =
            EnumerableDfs<Point, List<Point>.Enumerator>.EnumerateEdges(graph, startingPosition).ToList();
        edges.Add(new(edges.Last().Head, startingPosition));
        // https://en.wikipedia.org/wiki/Shoelace_formula
        int sum = edges.Select(it => Cross(it.Tail, it.Head)).Sum();
        int area = Math.Abs(sum) / 2;
        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        int boundaryPoints = edges.Count + 1;
        int interiorPoints = area - boundaryPoints / 2 + 1;
        return interiorPoints;
    }

    private static int Cross(Point left, Point right) => left.Row * right.Column - left.Column * right.Row;
}
