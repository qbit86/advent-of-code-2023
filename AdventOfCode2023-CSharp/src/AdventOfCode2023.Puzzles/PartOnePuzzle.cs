using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence.Models;
using Arborescence.Traversal.Adjacency;
using Edge = Arborescence.Endpoints<string>;

namespace AdventOfCode2023;

public static class PartOnePuzzle
{
    private const bool IsDrawingEnabled = true;

    private static readonly char[] s_separators = [' ', ':'];
    private static GraphDrawer? s_graphDrawer;

    private static GraphDrawer GraphDrawer => s_graphDrawer ??= GraphDrawer.Create();

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        var originalNeighborsByComponent = lines.Select(Parse).ToDictionary();
        var originalOrientedEdges = originalNeighborsByComponent
            .Select(kv => kv.Value.Select(it => CreateOrientedEdge(kv.Key, it)))
            .SelectMany(it => it).ToFrozenSet();
        List<Edge> cut = [];
#if false
        cut.Add(new("gtj", "tzj"));
        cut.Add(new("bbp", "dvr"));
        cut.Add(new("jzv", "qvq"));
#else
        FindCut(originalOrientedEdges, cut);
#endif

        var orientedEdges = originalOrientedEdges.Except(cut).ToList();
        var vertices = orientedEdges.Select(edge => edge.Tail).Union(orientedEdges.Select(edge => edge.Head)).ToList();
        IEnumerable<Edge> invertedEdges = orientedEdges.Select(edge => new Edge(edge.Head, edge.Tail));
        IEnumerable<Edge> edges = orientedEdges.Concat(invertedEdges);
        ILookup<string, string> lookup = edges.ToLookup(edge => edge.Tail, edge => edge.Head);
        var neighborsByVertex = lookup.ToFrozenDictionary(g => g.Key, g => g.ToList());
        if (IsDrawingEnabled)
            GraphDrawer.Draw(neighborsByVertex, vertices);
        var graph = ReadOnlyAdjacencyGraph<string>.FromLists(neighborsByVertex);
        string source = orientedEdges.Select(edge => edge.Tail).Min()!;
        var enumeratedVertices =
            EnumerableBfs<string, List<string>.Enumerator>.EnumerateVertices(graph, source).ToList();
        return enumeratedVertices.Count * (vertices.Count - (long)enumeratedVertices.Count);
    }

    private static void FindCut<TEdges>(TEdges edges, List<Edge> cut)
        where TEdges : IReadOnlyCollection<Edge>
    {
        var prototype = IncidenceAdjacency.Create(edges);
        if (false)
            GraphDrawer.Draw(prototype);

        int attemptCount = 0;
        for (; attemptCount < short.MaxValue; ++attemptCount)
        {
            IncidenceAdjacency ia = prototype.DeepClone();
            while (ia.Vertices.Count > 2)
            {
                ia.ContractRandom();
                if (false && (ia.Vertices.Count < 10 || ia.Vertices.Count % 100 is 0))
                    GraphDrawer.Draw(ia);
            }

            int edgeCount = ia.Edges.Count;
            if (edgeCount > 3)
                continue;
            if (edgeCount < 3)
                throw new InvalidOperationException();

            cut.AddRange(ia.Edges);
            break;
        }

        if (cut.Count is 0)
            throw new InvalidOperationException($"{nameof(attemptCount)}: {attemptCount}");
    }

    private static KeyValuePair<string, FrozenSet<string>> Parse(string line)
    {
        string[] parts = line.Split(s_separators,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string key = parts[0];
        var value = parts.Skip(1).ToFrozenSet();
        return new(key, value);
    }

    private static Edge CreateOrientedEdge(string left, string right) =>
        StringComparer.Ordinal.Compare(left, right) <= 0 ? new(left, right) : new(right, left);
}
