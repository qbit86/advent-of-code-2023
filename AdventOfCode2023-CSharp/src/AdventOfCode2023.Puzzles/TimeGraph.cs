using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Numerics;
using Arborescence;

namespace AdventOfCode2023;

internal static class TimeGraph
{
    internal static TimeGraph<TRays> Create<TRays>(TRays rays)
        where TRays : IReadOnlyList<Ray3<double>> => new(rays);
}

internal sealed class TimeGraph<TRays> : IOutNeighborsAdjacency<Node, IEnumerator<Node>>
    where TRays : IReadOnlyList<Ray3<double>>
{
    private readonly TRays _rays;

    internal TimeGraph(TRays rays) => _rays = rays ?? throw new ArgumentNullException(nameof(rays));

    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex)
    {
        if (TolerantComparer.Default<double>().Equals(vertex.Volume, 0.0))
        {
            IEnumerator<Node> result = Enumerable.Empty<Node>().GetEnumerator();
            result.Dispose();
            return result;
        }

        return EnumerateOutNeighborsIterator(vertex);
    }

    private IEnumerator<Node> EnumerateOutNeighborsIterator(Node vertex)
    {
        int start = vertex.Volume.GetHashCode() & ~(11 << 30);
        int upperBound = start + V3.Count;
        for (int i = start; i < upperBound; ++i)
        {
            int index = i % V3.Count;
            {
                if (TryGetNeighbor(vertex, index, 1, out Node neighbor))
                    yield return neighbor;
            }
            {
                if (TryGetNeighbor(vertex, index, -1, out Node neighbor))
                    yield return neighbor;
            }
        }
    }

    private bool TryGetNeighbor(Node node, int index, int sign, out Node neighbor) =>
        GradientHelpers.TryGetNeighbor(_rays, node, index, sign, out neighbor);

    internal Node CreateNode(V3<long> collisionTimes) => Node.Create(_rays, collisionTimes);
}
