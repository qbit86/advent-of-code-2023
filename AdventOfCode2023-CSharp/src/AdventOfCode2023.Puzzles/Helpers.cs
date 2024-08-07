using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Arborescence;
using EuclideanSpace;
using Dijkstra = Arborescence.Search.Adjacency.AdditiveEnumerableDijkstra<AdventOfCode2023.Node, int>;

namespace AdventOfCode2023;

internal static class Helpers
{
    internal static long Solve<TLines, TGraph>(
        TLines lines, int rowCount, int columnCount, TGraph graph,
        Predicate<Node> isDestination)
        where TLines : IReadOnlyList<string>
        where TGraph : IOutNeighborsAdjacency<Node, IEnumerator<Node>>
    {
        Debug.Assert(lines is not null);
        Debug.Assert(lines.Count > 0);

        var heatLossMap = HeatLossMap.Create(lines);
        Node source = new(Point2.Zero<int>(), Vector2.Zero<int>(), 0);
        Dictionary<Node, int> distanceByNode = new(rowCount * columnCount);
        var edges = Dijkstra.EnumerateEdges(graph, source, heatLossMap, distanceByNode);
        foreach (var _ in edges) { }

        var nodeDistancePairs = distanceByNode.Where(kv => isDestination(kv.Key));
        int result = nodeDistancePairs.Min(kv => kv.Value);
        return result;
    }
}
