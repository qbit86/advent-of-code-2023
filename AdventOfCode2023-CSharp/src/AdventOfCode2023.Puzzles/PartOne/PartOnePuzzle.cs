using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartOnePuzzle
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
        IEnumerable<Part> parts = partLines.Select(Part.Parse);
        IEnumerable<Part> acceptedParts = parts.Where(IsAccepted);
        IEnumerable<long> ratings = acceptedParts.Select(part => 0L + part.X + part.M + part.A + part.S);
        return ratings.Sum();

        bool IsAccepted(Part part)
        {
            Workflow current = workflowByName["in".AsMemory()];
            while (true)
            {
                foreach (Rule rule in current.Rules)
                {
                    if (rule is TransitionRule transitionRule)
                    {
                        if (transitionRule.IsAccepted)
                            return true;
                        if (transitionRule.IsRejected)
                            return false;
                        current = workflowByName[rule.Destination];
                        break;
                    }

                    if (rule is ComparisonRule comparisonRule)
                    {
                        if (!comparisonRule.Satisfies(part))
                            continue;
                        if (comparisonRule.Destination.Span is "A")
                            return true;
                        if (comparisonRule.Destination.Span is "R")
                            return false;
                        current = workflowByName[comparisonRule.Destination];
                        break;
                    }

                    throw new UnreachableException(rule.GetType().FullName);
                }
            }
        }
    }
}
