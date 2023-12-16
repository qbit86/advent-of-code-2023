using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    private static long Solve<TRows>(TRows lines)
        where TRows : IReadOnlyList<string>
    {
        var graph = Graph.Create(lines);
        Node source = new(Point.Empty, Directions.Right);
        IEnumerable<Node> nodes = EnumerableDfs<Node>.EnumerateVertices(graph, source);
        IEnumerable<Point> positions = nodes.Select(node => node.Position).Distinct();
        return positions.Count();
    }
}
