using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence;
using Arborescence.Models;
using Dijkstra = Arborescence.Search.Adjacency.AdditiveEnumerableDijkstra<AdventOfCode2023.Node, int>;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
{
    private const bool IsDrawingEnabled = false;

    private static readonly DateTime s_timestamp = DateTime.Now;

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines rows)
        where TLines : IReadOnlyList<string>
    {
        var map = PartTwoMap.Create(rows);
        Dictionary<Point, List<Point>> outNeighborsByPoint = new();
        Dictionary<Point, HashSet<Point>> inNeighborsByPoint = new();
        int rowCount = map.RowCount;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            int columnCount = map.GetColumnCount(rowIndex);
            for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
            {
                if (map.GetTile(rowIndex, columnIndex) is '#')
                    continue;
                Point current = new(columnIndex, rowIndex);
                var neighbors = map.GetOutNeighbors(current).ToList();
                outNeighborsByPoint.Add(current, neighbors);
                foreach (var neighbor in neighbors)
                {
                    if (inNeighborsByPoint.TryGetValue(neighbor, out var inNeighbors))
                        inNeighbors.Add(current);
                    else
                        inNeighborsByPoint.Add(neighbor, [current]);
                }
            }
        }

        var originalOutNeighborsByPoint = outNeighborsByPoint.ToFrozenDictionary(
            kv => kv.Key, kv => kv.Value.ToFrozenSet());

        if (IsDrawingEnabled)
        {
            var originalGraph = ReadOnlyAdjacencyGraph<Point>.FromFrozenSets(originalOutNeighborsByPoint);
            var timestamp = DateTime.Now;
            var arrayGraphDrawer = GraphDrawer<FrozenSet<Point>.Enumerator>.Create(timestamp);
            arrayGraphDrawer.Draw(originalGraph, originalOutNeighborsByPoint.Keys);
        }

        Dictionary<Endpoints<Point>, int> weightByPointEdge = new();
        foreach (var current in originalOutNeighborsByPoint.Keys)
        {
            var inNeighbors = inNeighborsByPoint[current];
            var outNeighbors = outNeighborsByPoint[current];
            if (outNeighbors.Count is not 2)
                continue;
            if (!inNeighbors.SetEquals(outNeighbors))
                continue;

            var left = outNeighbors[0];
            var right = outNeighbors[1];

            int newWeight = weightByPointEdge.GetValueOrDefault(Endpoints.Create(left, current), 1) +
                weightByPointEdge.GetValueOrDefault(Endpoints.Create(right, current), 1);
            weightByPointEdge.Add(Endpoints.Create(left, right), newWeight);
            weightByPointEdge.Add(Endpoints.Create(right, left), newWeight);
            weightByPointEdge.Remove(Endpoints.Create(left, current));
            weightByPointEdge.Remove(Endpoints.Create(current, left));
            weightByPointEdge.Remove(Endpoints.Create(right, current));
            weightByPointEdge.Remove(Endpoints.Create(current, right));

            var leftOutNeighbors = outNeighborsByPoint[left];
            leftOutNeighbors.Remove(current);
            leftOutNeighbors.Add(right);

            var leftInNeighbors = inNeighborsByPoint[left];
            leftInNeighbors.Remove(current);
            leftInNeighbors.Add(right);

            var rightOutNeighbors = outNeighborsByPoint[right];
            rightOutNeighbors.Remove(current);
            rightOutNeighbors.Add(left);

            var rightInNeighbors = inNeighborsByPoint[right];
            rightInNeighbors.Remove(current);
            rightInNeighbors.Add(left);

            outNeighborsByPoint.Remove(current);
            inNeighborsByPoint.Remove(current);
        }

        if (IsDrawingEnabled)
        {
            var thinnedGraph = ReadOnlyAdjacencyGraph<Point>.FromLists(outNeighborsByPoint);
            var listGraphDrawer = GraphDrawer<List<Point>.Enumerator>.Create(s_timestamp);
            listGraphDrawer.Draw(thinnedGraph, outNeighborsByPoint.Keys, weightByPointEdge);
        }

        List<Point> pointByVertex = new(outNeighborsByPoint.Count);
        Dictionary<Point, int> vertexByPoint = new(outNeighborsByPoint.Count);
        foreach (var point in outNeighborsByPoint.Keys)
        {
            int vertex = pointByVertex.Count;
            pointByVertex.Add(point);
            vertexByPoint.Add(point, vertex);
        }

        var outNeighborsByVertex = outNeighborsByPoint.ToFrozenDictionary(
            kv => vertexByPoint[kv.Key],
            kv => kv.Value.Select(p => vertexByPoint[p]).ToArray());
        var underlyingGraph = ReadOnlyAdjacencyGraph<int>.FromArrays(outNeighborsByVertex);
        var graph = Graph<ArraySegment<int>.Enumerator>.Create(underlyingGraph);

        var weightByVertexEdge = weightByPointEdge.ToFrozenDictionary(
            kv => Endpoints.Create(vertexByPoint[kv.Key.Tail], vertexByPoint[kv.Key.Head]),
            kv => kv.Value);

        Point sourcePoint = new(rows[0].IndexOf('.', StringComparison.Ordinal), 0);
        int sourceVertex = vertexByPoint[sourcePoint];
        Node sourceNode = new(sourceVertex, 1L << sourceVertex);

        Dictionary<Node, int> distanceByVertex = new();
        WeightMap weightByNodeEdge = new(weightByVertexEdge);
        var relaxedEdges = Dijkstra.EnumerateEdges(
            graph, sourceNode, weightByNodeEdge, distanceByVertex, default(InvertedComparer<int>));
        foreach (var _ in relaxedEdges) { }

        Point targetPoint = new(rows[^1].IndexOf('.', StringComparison.Ordinal), rowCount - 1);
        int targetVertex = vertexByPoint[targetPoint];
        var targetVertexDistancePairs = distanceByVertex
            .Where(kv => kv.Key.Vertex == targetVertex);
        return targetVertexDistancePairs.Max(kv => kv.Value);
    }
}

file readonly struct WeightMap : IReadOnlyDictionary<Endpoints<Node>, int>
{
    private readonly FrozenDictionary<Endpoints<int>, int> _weightByEdge;

    internal WeightMap(FrozenDictionary<Endpoints<int>, int> weightByEdge)
    {
        Debug.Assert(weightByEdge is not null);
        _weightByEdge = weightByEdge;
    }

    public IEnumerator<KeyValuePair<Endpoints<Node>, int>> GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Endpoints<Node>> Keys => throw new NotSupportedException();
    public IEnumerable<int> Values => throw new NotSupportedException();
    public int Count => throw new NotSupportedException();

    public bool ContainsKey(Endpoints<Node> key) => throw new NotSupportedException();

    public bool TryGetValue(Endpoints<Node> key, out int value)
    {
        Endpoints<int> edge = new(key.Tail.Vertex, key.Head.Vertex);
        return _weightByEdge.TryGetValue(edge, out value);
    }

    public int this[Endpoints<Node> key] => throw new NotSupportedException();
}
