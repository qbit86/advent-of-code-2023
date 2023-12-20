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

public static class PartOneAndHalfPuzzle
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
        string[] partLines = parts[1].Split(s_newLineSeparators, SplitOptions);
        return Solve(workflowLines, partLines);
    }

    private static long Solve(IEnumerable<string> workflowLines, IEnumerable<string> partLines)
    {
        Debug.Assert(workflowLines is not null);
        Debug.Assert(partLines is not null);
        IEnumerable<Workflow> workflows = workflowLines.Select(Workflow.Parse);
        var workflowByName = workflows.ToFrozenDictionary(workflow => workflow.Name, NameComparer.Instance);
        var graph = Graph.Create(workflowByName);
        ShortRange startingRange = new(1, 4001);
        var startingRanges = ImmutableList.Create(startingRange, startingRange, startingRange, startingRange);
        Node source = new("in".AsMemory(), startingRanges, 0);
        IEnumerable<Node> nodes = EnumerableDfs<Node>.EnumerateVertices(graph, source);
        var acceptedNodes = nodes.Where(node => node.Name.Span is "A").ToList();

        IEnumerable<Part> parts = partLines.Select(Part.Parse);
        IEnumerable<Part> acceptedParts = parts.Where(IsAccepted);
        IEnumerable<long> ratings = acceptedParts.Select(part => 0L + part.X + part.M + part.A + part.S);
        return ratings.Sum();

        bool IsAccepted(Part part)
        {
            return acceptedNodes.Any(node => Contains(node, part));
        }

        bool Contains(Node node, Part part)
        {
            ImmutableList<ShortRange> ranges = node.Ranges;
            return ranges[0].Contains(part.X) &&
                ranges[1].Contains(part.M) &&
                ranges[2].Contains(part.A) &&
                ranges[3].Contains(part.S);
        }
    }
}
