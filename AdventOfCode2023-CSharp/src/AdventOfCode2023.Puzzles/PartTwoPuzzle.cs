using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
        var topNodes = Enumerable.Range(0, columnCount)
            .Select(columnIndex => new Node(new(columnIndex, 0), Directions.Down));
        var leftNodes = Enumerable.Range(0, rowCount)
            .Select(rowIndex => new Node(new(0, rowIndex), Directions.Right));
        var bottomNodes = Enumerable.Range(0, columnCount)
            .Select(columnIndex => new Node(new(columnIndex, rowCount - 1), Directions.Up));
        var rightNodes = Enumerable.Range(0, rowCount)
            .Select(rowIndex => new Node(new(columnCount - 1, rowIndex), Directions.Left));
        var sources = topNodes.Concat(leftNodes).Concat(bottomNodes).Concat(rightNodes);
        var energizedTileCounts = sources.Select(GetEnergizedTileCount);
        return energizedTileCounts.Max();

        int GetEnergizedTileCount(Node source)
        {
            var nodes =
                EnumerableGenericSearch<Node>.EnumerateVertices(graph, source, new ConcurrentStack<Node>());
            var positions = nodes.Select(node => node.Position).Distinct();
            return positions.Count();
        }
    }
}
