using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Arborescence;
using Arborescence.Models;

namespace AdventOfCode2023;

internal static class Helpers
{
    internal static long Solve<TLines, TGraph>(
        TLines lines, int rowCount, int columnCount, TGraph graph,
        Predicate<Node> isDestination)
        where TLines : IReadOnlyList<string>
        where TGraph : IForwardIncidence<Node, Endpoints<Node>, IncidenceEnumerator<Node, IEnumerator<Node>>>
    {
        Debug.Assert(lines is not null);
        Debug.Assert(lines.Count > 0);

        var heatLossMap = HeatLossMap.Create(lines);
        Node source = new(Point.Empty, Size.Empty, 0);
        Dictionary<Node, int> distanceByNode = new(rowCount * columnCount) { [source] = 0 };
        PriorityQueue<Node, int> frontier = new();
        frontier.Enqueue(source, 0);
        while (frontier.TryDequeue(out Node current, out int priority))
        {
            int currentDistance = distanceByNode[current];
            if (priority > currentDistance)
                continue;
            IncidenceEnumerator<Node, IEnumerator<Node>> edges = graph.EnumerateOutEdges(current);
            foreach (Endpoints<Node> edge in edges)
            {
                Node neighbor = edge.Head;
                int weight = heatLossMap.GetValueOrDefault(edge);
                int neighborDistanceCandidate = currentDistance + weight;
                if (neighborDistanceCandidate < distanceByNode.GetValueOrDefault(neighbor, int.MaxValue))
                {
                    distanceByNode[neighbor] = neighborDistanceCandidate;
                    frontier.Enqueue(neighbor, neighborDistanceCandidate);
                }
            }
        }

        IEnumerable<KeyValuePair<Node, int>> nodeDistancePairs = distanceByNode.Where(kv => isDestination(kv.Key));
        int result = nodeDistancePairs.Min(kv => kv.Value);
        return result;
    }
}
