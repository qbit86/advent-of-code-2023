using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023;

internal readonly struct Workflow
{
    private readonly List<Rule> _rules;

    private Workflow(string rawText, ReadOnlyMemory<char> name, List<Rule> rules)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(rawText));
        _rules = rules;
        RawText = rawText;
        Name = name;
    }

    private string RawText { get; }
    internal ReadOnlyMemory<char> Name { get; }
    internal IReadOnlyList<Rule> Rules => _rules;

    public override string ToString() => RawText;

    internal static Workflow Parse(string line)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(line));
        Span<Range> ranges = stackalloc Range[3];
        int count = line.AsSpan().SplitAny(ranges, "{}",
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (count is not 2)
            throw new ArgumentException($"{nameof(count)}: {count}", nameof(line));

        ReadOnlyMemory<char> lineMemory = line.AsMemory();
        ReadOnlyMemory<char> name = lineMemory[ranges[0]];
        List<Rule> rules = new();
        RuleFactory.ParseAll(lineMemory[ranges[1]], rules);
        return new(line, name, rules);
    }
}
