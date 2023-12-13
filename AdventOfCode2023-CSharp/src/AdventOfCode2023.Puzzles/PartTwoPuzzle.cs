using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
{
    private static readonly char[] s_groupSeparators = { '.' };
    private static readonly char[] s_lengthSeparators = { ',' };

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    public static long Solve(string line)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(line);
        return SolveCore(line);
    }

    private static long Solve(IEnumerable<string> lines)
    {
        IEnumerable<long> arrangementCounts = lines.Select(SolveCore);
        return arrangementCounts.Sum();
    }

    private static long SolveCore(string line)
    {
        ConditionRecord root = Parse(line);
        return Helpers.ComputePossibleArrangementCount(root);
    }

    private static ConditionRecord Parse(string line)
    {
        string[] parts = line.Split(' ', 2, Helpers.SplitOptions);
        if (parts.Length is not 2)
            throw new ArgumentException($"{nameof(parts)}.Length = {parts.Length}", nameof(line));
        if (string.IsNullOrWhiteSpace(parts[0]))
            throw new ArgumentException(parts[0], nameof(line));
        string unfoldedRow = string.Join('?', Enumerable.Repeat(parts[0], 5).Select(it => new string(it)));
        List<StringSegment> observedGroups = new();
        StringTokenizer groupTokenizer = new(unfoldedRow, s_groupSeparators);
        foreach (StringSegment segment in groupTokenizer)
        {
            if (!segment.AsSpan().IsEmpty)
                observedGroups.Add(segment);
        }

        observedGroups.Reverse();
        var observedGroupsStack = ImmutableStack.CreateRange(observedGroups);

        if (string.IsNullOrWhiteSpace(parts[1]))
            throw new ArgumentException(parts[1], nameof(line));
        List<int> foldedActualLengths = new();
        StringTokenizer lengthTokenizer = new(parts[1], s_lengthSeparators);
        foreach (StringSegment segment in lengthTokenizer)
        {
            int groupLength = int.Parse(segment, CultureInfo.InvariantCulture);
            foldedActualLengths.Add(groupLength);
        }

        ReadOnlyMemory<int> actualLengthsMemory =
            Enumerable.Repeat(foldedActualLengths, 5).SelectMany(it => it).ToArray();
        return new(observedGroupsStack, actualLengthsMemory);
    }
}
