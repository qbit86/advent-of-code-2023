using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence.Traversal.Adjacency;
using EuclideanSpace;

namespace AdventOfCode2023;

public static class PartOnePuzzle
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TRows>(TRows lines)
        where TRows : IReadOnlyList<string>
    {
        var graph = Graph.Create(lines);
        Node source = new(Point2.Zero<int>(), Directions.Right);
        var nodes = EnumerableDfs<Node>.EnumerateVertices(graph, source);
        var positions = nodes.Select(node => node.Position).Distinct();
        return positions.Count();
    }
}
