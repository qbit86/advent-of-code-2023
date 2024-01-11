using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Arborescence;
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
        Node source = new(Point.Empty, Size.Empty, 0);
        Dictionary<Node, int> distanceByNode = new(rowCount * columnCount);
        IEnumerable<Endpoints<Node>> edges = Dijkstra.EnumerateEdges(graph, source, heatLossMap, distanceByNode);
        foreach (Endpoints<Node> _ in edges) { }

        IEnumerable<KeyValuePair<Node, int>> nodeDistancePairs = distanceByNode.Where(kv => isDestination(kv.Key));
        int result = nodeDistancePairs.Min(kv => kv.Value);
        return result;
    }
}
