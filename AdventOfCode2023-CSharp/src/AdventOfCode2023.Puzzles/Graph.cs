using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Arborescence;

namespace AdventOfCode2023;

public static class Graph
{
    internal static Size[] Directions { get; } =
        [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];

    public static Graph<TMap> Create<TMap>(TMap map) where TMap : IMap => new(map);
}

public sealed class Graph<TMap> : IOutNeighborsAdjacency<Point, IEnumerator<Point>>
    where TMap : IMap
{
    private readonly TMap _map;

    internal Graph(TMap map) => _map = map;

    public IEnumerator<Point> EnumerateOutNeighbors(Point vertex) =>
        Graph.Directions.Select(direction => vertex + direction).Where(_map.IsWalkable).GetEnumerator();
}
