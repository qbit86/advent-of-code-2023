using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Arborescence;

namespace AdventOfCode2023;

internal static class Graph
{
    internal static Graph<TWorkflowDictionary> Create<TWorkflowDictionary>(TWorkflowDictionary workflowByName)
        where TWorkflowDictionary : IReadOnlyDictionary<ReadOnlyMemory<char>, Workflow>
    {
        Debug.Assert(workflowByName is not null);
        return new(workflowByName);
    }
}

internal sealed class Graph<TWorkflowDictionary> : IOutNeighborsAdjacency<Node, IEnumerator<Node>>
    where TWorkflowDictionary : IReadOnlyDictionary<ReadOnlyMemory<char>, Workflow>
{
    internal Graph(TWorkflowDictionary workflowByName)
    {
        Debug.Assert(workflowByName is not null);
        WorkflowByName = workflowByName;
    }

    private TWorkflowDictionary WorkflowByName { get; }

    // ReSharper disable once NotDisposedResourceIsReturned
    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex) => vertex.Name.Span is "A" or "R"
        ? Enumerable.Empty<Node>().GetEnumerator()
        : EnumerateOutNeighborsIterator(vertex);

    private IEnumerator<Node> EnumerateOutNeighborsIterator(Node vertex)
    {
        Debug.Assert(vertex.Name.Span is not ("A" or "R"));
        Workflow workflow = WorkflowByName[vertex.Name];
        ImmutableList<ShortRange> ranges = vertex.Ranges;
        Rule rule = workflow.Rules[vertex.RuleIndex];
        if (rule is TransitionRule transitionRule)
        {
            if (!transitionRule.IsRejected)
                yield return new(rule.Destination, ranges, 0);
            yield break;
        }

        if (rule is ComparisonRule comparisonRule)
        {
            ShortRange oldRange = ranges[comparisonRule.CategoryIndex];
            if (comparisonRule is LessComparisonRule)
            {
                ShortRange nextWorkflowRangeCandidate = oldRange with { End = comparisonRule.Operand };
                if (!nextWorkflowRangeCandidate.IsEmpty)
                {
                    ImmutableList<ShortRange> newRanges =
                        ranges.SetItem(comparisonRule.CategoryIndex, nextWorkflowRangeCandidate);
                    yield return new(comparisonRule.Destination, newRanges, 0);
                }

                ShortRange remainingWorkflowRangeCandidate = oldRange with { Start = comparisonRule.Operand };
                if (!remainingWorkflowRangeCandidate.IsEmpty)
                {
                    ImmutableList<ShortRange> newRanges =
                        ranges.SetItem(comparisonRule.CategoryIndex, remainingWorkflowRangeCandidate);
                    yield return new(vertex.Name, newRanges, vertex.RuleIndex + 1);
                }

                yield break;
            }

            if (comparisonRule is GreaterComparisonRule)
            {
                ShortRange nextWorkflowRangeCandidate = oldRange with { Start = (short)(comparisonRule.Operand + 1) };
                if (!nextWorkflowRangeCandidate.IsEmpty)
                {
                    ImmutableList<ShortRange> newRanges =
                        ranges.SetItem(comparisonRule.CategoryIndex, nextWorkflowRangeCandidate);
                    yield return new(comparisonRule.Destination, newRanges, 0);
                }

                ShortRange remainingWorkflowRangeCandidate =
                    oldRange with { End = (short)(comparisonRule.Operand + 1) };
                if (!remainingWorkflowRangeCandidate.IsEmpty)
                {
                    ImmutableList<ShortRange> newRanges =
                        ranges.SetItem(comparisonRule.CategoryIndex, remainingWorkflowRangeCandidate);
                    yield return new(vertex.Name, newRanges, vertex.RuleIndex + 1);
                }

                yield break;
            }

            throw new UnreachableException($"Workflow: {workflow}, node: {vertex}");
        }

        throw new UnreachableException(rule.GetType().Name);
    }
}
