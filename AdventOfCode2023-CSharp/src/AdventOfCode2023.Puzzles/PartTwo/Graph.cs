using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Arborescence;
using Arborescence.Models;

namespace AdventOfCode2023;

internal readonly record struct Node(int Vertex, long VisitedMask)
{
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(nameof(Vertex) + " = ");
        builder.Append(Vertex);
        builder.Append("; " + nameof(VisitedMask) + " = ");
        builder.Append(VisitedMask.ToString("B", CultureInfo.InvariantCulture));
        return true;
    }
}

internal static class Graph<TUnderlyingGraphNeighbors>
    where TUnderlyingGraphNeighbors : IEnumerator<int>
{
    internal static Graph<TUnderlyingGraph, TUnderlyingGraphNeighbors> Create<TUnderlyingGraph>(
        TUnderlyingGraph underlyingGraph)
        where TUnderlyingGraph : IOutNeighborsAdjacency<int, TUnderlyingGraphNeighbors> => new(underlyingGraph);
}

internal sealed class Graph<TUnderlyingGraph, TUnderlyingGraphNeighbors> :
    IOutNeighborsAdjacency<Node, IEnumerator<Node>>,
    IOutEdgesIncidence<Node, IncidenceEnumerator<Node, IEnumerator<Node>>>
    where TUnderlyingGraph : IOutNeighborsAdjacency<int, TUnderlyingGraphNeighbors>
    where TUnderlyingGraphNeighbors : IEnumerator<int>
{
    private readonly TUnderlyingGraph _underlyingGraph;

    internal Graph(TUnderlyingGraph underlyingGraph)
    {
        Debug.Assert(underlyingGraph is not null);
        _underlyingGraph = underlyingGraph;
    }

    public IncidenceEnumerator<Node, IEnumerator<Node>> EnumerateOutEdges(Node vertex) =>
        IncidenceEnumerator<Node, IEnumerator<Node>>.Create(this, vertex);

    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex)
    {
        TUnderlyingGraphNeighbors underlyingOutNeighborsEnumerator =
            _underlyingGraph.EnumerateOutNeighbors(vertex.Vertex);
        while (underlyingOutNeighborsEnumerator.MoveNext())
        {
            int underlyingOutNeighbor = underlyingOutNeighborsEnumerator.Current;
            long underlyingOutNeighborMask = 1L << underlyingOutNeighbor;
            bool wasVisited = (vertex.VisitedMask & underlyingOutNeighborMask) is not 0L;
            if (wasVisited)
                continue;
            long outNeighborVisitedMask = vertex.VisitedMask | underlyingOutNeighborMask;
            yield return new(underlyingOutNeighbor, outNeighborVisitedMask);
        }
    }
}
