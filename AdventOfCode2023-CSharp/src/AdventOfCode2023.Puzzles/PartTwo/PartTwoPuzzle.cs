using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
{
    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    private static readonly string[] s_blankLineSeparators = { "\n\n", "\r\n\r\n" };
    private static readonly string[] s_newLineSeparators = { "\n", "\r\n" };

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string text = await File.ReadAllTextAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(text);
    }

    private static long Solve(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        string[] parts = text.Split(s_blankLineSeparators, SplitOptions);
        if (parts.Length is not 2)
            throw new ArgumentException($"{nameof(parts)}: {parts.Length}", nameof(text));
        string[] workflowLines = parts[0].Split(s_newLineSeparators, SplitOptions);
        return Solve(workflowLines);
    }

    private static long Solve(IEnumerable<string> workflowLines)
    {
        Debug.Assert(workflowLines is not null);
        IEnumerable<Workflow> workflows = workflowLines.Select(Workflow.Parse);
        var workflowByName = workflows.ToFrozenDictionary(workflow => workflow.Name, NameComparer.Instance);
        var graph = Graph.Create(workflowByName);
        ShortRange startingRange = new(1, 4001);
        var startingRanges = ImmutableList.Create(startingRange, startingRange, startingRange, startingRange);
        Node source = new("in".AsMemory(), startingRanges, 0);
        IEnumerable<Node> nodes = EnumerableDfs<Node>.EnumerateVertices(graph, source);
        IEnumerable<Node> acceptedNodes = nodes.Where(node => node.Name.Span is "A");
        IEnumerable<long> ratingCombinationsCollection = acceptedNodes.Select(CountCombinations);
        return ratingCombinationsCollection.Sum();
    }

    private static long CountCombinations(Node node) =>
        node.Ranges.Select(range => range.Length).Aggregate(1L, (left, right) => left * right);
}
