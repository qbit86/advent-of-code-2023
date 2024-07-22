using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Edge = Arborescence.Endpoints<string>;

namespace AdventOfCode2023;

internal sealed class IncidenceAdjacency
{
    private readonly List<Edge> _edges;
    private readonly Dictionary<Edge, string> _headByEdge;
    private readonly Dictionary<string, HashSet<Edge>> _inEdgesByVertex;
    private readonly Dictionary<string, HashSet<Edge>> _outEdgesByVertex;
    private readonly Dictionary<Edge, string> _tailByEdge;
    private readonly HashSet<string> _vertices;

    private IncidenceAdjacency(
        HashSet<string> vertices,
        List<Edge> edges,
        Dictionary<Edge, string> tailByEdge,
        Dictionary<Edge, string> headByEdge,
        Dictionary<string, HashSet<Edge>> outEdgesByVertex,
        Dictionary<string, HashSet<Edge>> inEdgesByVertex)
    {
        _vertices = vertices;
        _edges = edges;
        _tailByEdge = tailByEdge;
        _headByEdge = headByEdge;
        _outEdgesByVertex = outEdgesByVertex;
        _inEdgesByVertex = inEdgesByVertex;
    }

    internal IReadOnlySet<string> Vertices => _vertices;
    internal IReadOnlyList<Edge> Edges => _edges;
    internal IReadOnlyDictionary<Edge, string> TailByEdge => _tailByEdge;
    internal IReadOnlyDictionary<Edge, string> HeadByEdge => _headByEdge;

    internal static IncidenceAdjacency Create<TEdges>(TEdges edges)
        where TEdges : IReadOnlyCollection<Edge>
    {
        Debug.Assert(edges is not null);
        HashSet<string> vertices = [];
        Dictionary<Edge, string> tailByEdge = new(edges.Count);
        Dictionary<Edge, string> headByEdge = new(edges.Count);
        Dictionary<string, HashSet<Edge>> outEdgesByVertex = new();
        Dictionary<string, HashSet<Edge>> inEdgesByVertex = new();

        foreach (Edge edge in edges)
        {
            vertices.Add(edge.Tail);
            tailByEdge[edge] = edge.Tail;

            if (outEdgesByVertex.TryGetValue(edge.Tail, out HashSet<Edge>? outEdges))
                outEdges.Add(edge);
            else
                outEdgesByVertex.Add(edge.Tail, [edge]);

            vertices.Add(edge.Head);
            headByEdge[edge] = edge.Head;
            if (inEdgesByVertex.TryGetValue(edge.Head, out HashSet<Edge>? inEdges))
                inEdges.Add(edge);
            else
                inEdgesByVertex.Add(edge.Head, [edge]);
        }

        return new(vertices, edges.ToList(), tailByEdge, headByEdge, outEdgesByVertex, inEdgesByVertex);
    }

    internal IncidenceAdjacency DeepClone()
    {
        var vertices = _vertices.ToHashSet();
        var edges = _edges.ToList();
        var tailByEdge = _tailByEdge.ToDictionary();
        var headByEdge = _headByEdge.ToDictionary();
        var outEdgesByVertex = _outEdgesByVertex.ToDictionary(kv => kv.Key, kv => kv.Value.ToHashSet());
        var inEdgesByVertex = _inEdgesByVertex.ToDictionary(kv => kv.Key, kv => kv.Value.ToHashSet());
        return new(vertices, edges, tailByEdge, headByEdge, outEdgesByVertex, inEdgesByVertex);
    }

    internal void ContractRandom() => Contract(Random.Shared.Next(_edges.Count));

    private void Contract(int edgeIndex)
    {
        ListHelpers<Edge>.SwapRemoveAt(_edges, edgeIndex, out Edge edge);
        ContractCore(edge);
    }

    private void ContractCore(Edge edge)
    {
        string tail = _tailByEdge[edge];
        _vertices.Remove(tail);
        string head = _headByEdge[edge];
        _vertices.Remove(head);
        string newVertex = string.Create(6, edge, SpanAction);
        _vertices.Add(newVertex);

        if (!_outEdgesByVertex.Remove(tail, out HashSet<Edge>? newOutEdges))
            newOutEdges = [];

        if (_outEdgesByVertex.Remove(head, out HashSet<Edge>? oldHeadOutEdges))
            newOutEdges.UnionWith(oldHeadOutEdges);
        _outEdgesByVertex[newVertex] = newOutEdges;
        foreach (Edge newOutEdge in newOutEdges)
            _tailByEdge[newOutEdge] = newVertex;

        if (!_inEdgesByVertex.Remove(tail, out HashSet<Edge>? newInEdges))
            newInEdges = [];

        if (_inEdgesByVertex.Remove(head, out HashSet<Edge>? oldHeadInEdges))
            newInEdges.UnionWith(oldHeadInEdges);
        _inEdgesByVertex[newVertex] = newInEdges;
        foreach (Edge inEdge in newInEdges)
            _headByEdge[inEdge] = newVertex;

        // Удаляем петли.
        newOutEdges.RemoveWhere(e => _headByEdge[e] == newVertex);
        newInEdges.RemoveWhere(e => _tailByEdge[e] == newVertex);
        for (int edgeIndex = 0; edgeIndex < _edges.Count;)
        {
            Edge current = _edges[edgeIndex];
            if (_headByEdge[current] == _tailByEdge[current])
            {
                ListHelpers<Edge>.SwapRemoveAt(_edges, edgeIndex);
                _headByEdge.Remove(current);
                _tailByEdge.Remove(current);
            }
            else
            {
                ++edgeIndex;
            }
        }

        return;

        static void SpanAction(Span<char> span, Edge arg)
        {
            arg.Tail.AsSpan(0, 3).CopyTo(span);
            arg.Head.AsSpan()[^3..].CopyTo(span[^3..]);
        }
    }
}
