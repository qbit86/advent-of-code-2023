using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    private static long Solve<TRows>(TRows lines)
        where TRows : IReadOnlyList<string>
    {
        Debug.Assert(lines is not null);
        int rowCount = lines.Count;
        Debug.Assert(rowCount > 0);
        int columnCount = lines[0].Length;
        var graph = Graph.Create(lines);
        IEnumerable<Node> topNodes = Enumerable.Range(0, columnCount)
            .Select(columnIndex => new Node(new(columnIndex, 0), Directions.Down));
        IEnumerable<Node> leftNodes = Enumerable.Range(0, rowCount)
            .Select(rowIndex => new Node(new(0, rowIndex), Directions.Right));
        IEnumerable<Node> bottomNodes = Enumerable.Range(0, columnCount)
            .Select(columnIndex => new Node(new(columnIndex, rowCount - 1), Directions.Up));
        IEnumerable<Node> rightNodes = Enumerable.Range(0, rowCount)
            .Select(rowIndex => new Node(new(columnCount - 1, rowIndex), Directions.Left));
        IEnumerable<Node> sources = topNodes.Concat(leftNodes).Concat(bottomNodes).Concat(rightNodes);
        IEnumerable<int> energizedTileCounts = sources.Select(GetEnergizedTileCount);
        return energizedTileCounts.Max();

        int GetEnergizedTileCount(Node source)
        {
            IEnumerable<Node> nodes =
                EnumerableGenericSearch<Node>.EnumerateVertices(graph, source, new ConcurrentStack<Node>());
            IEnumerable<Point> positions = nodes.Select(node => node.Position).Distinct();
            return positions.Count();
        }
    }
}
