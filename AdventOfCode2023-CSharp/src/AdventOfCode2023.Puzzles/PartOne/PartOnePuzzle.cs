using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence;
using Arborescence.Models;
using Dijkstra = Arborescence.Search.Adjacency.AdditiveEnumerableDijkstra<
    System.Drawing.Point, System.Collections.Generic.List<System.Drawing.Point>.Enumerator, int>;

namespace AdventOfCode2023;

public static class PartOnePuzzle
{
    private const bool IsDrawingEnabled = false;

    private static readonly DateTime s_timestamp = DateTime.Now;

    private static GraphDrawer<List<Point>.Enumerator>? s_listGraphDrawer;

    private static GraphDrawer<List<Point>.Enumerator> ListGraphDrawer =>
        s_listGraphDrawer ??= GraphDrawer<List<Point>.Enumerator>.Create(s_timestamp);

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TRows>(TRows rows)
        where TRows : IReadOnlyList<string>
    {
        var map = PartOneMap.Create(rows);
        Dictionary<Point, List<Point>> outNeighborsByPoint = new();
        int rowCount = map.RowCount;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            int columnCount = map.GetColumnCount(rowIndex);
            for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
            {
                if (map.GetTile(rowIndex, columnIndex) is '#')
                    continue;
                Point point = new(columnIndex, rowIndex);
                IEnumerable<Point> neighbors = map.GetOutNeighbors(point);
                outNeighborsByPoint.Add(point, neighbors.ToList());
            }
        }

        var originalOutNeighborsByPoint = outNeighborsByPoint.ToFrozenDictionary(
            kv => kv.Key, kv => kv.Value.ToArray());
        if (IsDrawingEnabled)
        {
            var originalGraph = ReadOnlyAdjacencyGraph<Point>.FromArrays(originalOutNeighborsByPoint);
            var arrayGraphDrawer = GraphDrawer<ArraySegment<Point>.Enumerator>.Create(s_timestamp);
            arrayGraphDrawer.Draw(originalGraph, originalOutNeighborsByPoint.Keys, map);
        }

        Point source = new(rows[0].IndexOf('.', StringComparison.Ordinal), 0);
        Point target = new(rows[^1].IndexOf('.', StringComparison.Ordinal), rowCount - 1);
        IEnumerable<Point> singleNeighborPoints = originalOutNeighborsByPoint
            .Where(kv => kv.Value.Length is 1).Select(kv => kv.Key)
            .Where(point => point != target)
            .Append(source);
        foreach (Point singleNeighborPoint in singleNeighborPoints)
        {
            for (Point current = singleNeighborPoint,
                 neighbor = originalOutNeighborsByPoint[singleNeighborPoint].Single();;)
            {
                List<Point> neighborNeighbors = outNeighborsByPoint[neighbor];
                neighborNeighbors.Remove(current);
                if (neighborNeighbors.Count is not 1)
                    break;

                current = neighbor;
                neighbor = neighborNeighbors[0];
            }
        }

        if (IsDrawingEnabled)
        {
            var thinnedGraph = ReadOnlyAdjacencyGraph<Point>.FromLists(outNeighborsByPoint);
            ListGraphDrawer.Draw(thinnedGraph, outNeighborsByPoint.Keys, map);
        }

        Dictionary<Point, List<Point>> inNeighborsByPoint = new();
        IEnumerable<Endpoints<Point>> edges = outNeighborsByPoint
            .Select(kv => kv.Value.Select(head => Endpoints.Create(kv.Key, head)))
            .SelectMany(it => it);
        foreach (Endpoints<Point> edge in edges)
        {
            if (inNeighborsByPoint.TryGetValue(edge.Head, out List<Point>? inNeighbors))
                inNeighbors.Add(edge.Tail);
            else
                inNeighborsByPoint.Add(edge.Head, [edge.Tail]);
        }

        IEnumerable<Point> intermediatePoints = outNeighborsByPoint.Keys.Intersect(inNeighborsByPoint.Keys);
        foreach (Point current in intermediatePoints)
        {
            if (outNeighborsByPoint[current] is not [var outNeighbor])
                continue;
            if (inNeighborsByPoint[current] is not [var inNeighbor])
                continue;

            Size outDirection = new Size(outNeighbor) - new Size(current);
            Size inDirection = new Size(current) - new Size(inNeighbor);
            if (inDirection.Width * outDirection.Width + inDirection.Height * outDirection.Height is 0)
                continue;

            List<Point> outNeighborsOfInNeighbor = outNeighborsByPoint[inNeighbor];
            int outIndex = outNeighborsOfInNeighbor.FindIndex(it => current.Equals(it));
            outNeighborsOfInNeighbor[outIndex] = outNeighbor;

            List<Point> inNeighborsOfOutNeighbor = inNeighborsByPoint[outNeighbor];
            int inIndex = inNeighborsOfOutNeighbor.FindIndex(it => current.Equals(it));
            inNeighborsOfOutNeighbor[inIndex] = inNeighbor;

            outNeighborsByPoint.Remove(current);
            inNeighborsByPoint.Remove(current);
        }

        var flyweightGraph = ReadOnlyAdjacencyGraph<Point>.FromLists(outNeighborsByPoint);
        if (IsDrawingEnabled)
            ListGraphDrawer.Draw(flyweightGraph, outNeighborsByPoint.Keys, map);

        Dictionary<Point, int> distanceByVertex = new();
        IEnumerable<Endpoints<Point>> relaxedEdges = Dijkstra.EnumerateEdges(
            flyweightGraph, source, default(WeightMap), distanceByVertex, default(InvertedComparer<int>));
        foreach (Endpoints<Point> _ in relaxedEdges) { }

        return distanceByVertex[target];
    }
}

file readonly struct WeightMap : IReadOnlyDictionary<Endpoints<Point>, int>
{
    public IEnumerator<KeyValuePair<Endpoints<Point>, int>> GetEnumerator() => throw new NotSupportedException();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Endpoints<Point>> Keys => throw new NotSupportedException();
    public IEnumerable<int> Values => throw new NotSupportedException();
    public int Count => throw new NotSupportedException();

    public bool ContainsKey(Endpoints<Point> key) => throw new NotSupportedException();

    public bool TryGetValue(Endpoints<Point> key, out int value)
    {
        value = key.Tail.ManhattanDistance(key.Head);
        return true;
    }

    public int this[Endpoints<Point> key] => throw new NotSupportedException();
}
