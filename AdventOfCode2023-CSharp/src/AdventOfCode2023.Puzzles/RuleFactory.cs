using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace AdventOfCode2023;

internal static class RuleFactory
{
    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    internal static void ParseAll<TRules>(ReadOnlyMemory<char> s, TRules destination)
        where TRules : ICollection<Rule>
    {
        Debug.Assert(destination is not null);
        // Example: a<2006:qkq,m>2090:A,rfg
        int maxCount = s.Span.Count(',');
        Span<Range> ranges = stackalloc Range[maxCount + 2];
        int count = s.Span.Split(ranges, ',', SplitOptions);
        foreach (Range range in ranges[..count])
        {
            Rule rule = Parse(s[range]);
            destination.Add(rule);
        }
    }

    private static Rule Parse(ReadOnlyMemory<char> s)
    {
        Debug.Assert(!s.IsEmpty);
        Span<Range> ranges = stackalloc Range[3];
        int count = s.Span.Split(ranges, ':', SplitOptions);
        if (count is 1)
            return new TransitionRule(s, s);
        if (count > 2)
            throw new ArgumentException($"{nameof(count)}: {count} > 2", nameof(s));
        ReadOnlyMemory<char> left = s[ranges[0]];
        short operand = short.Parse(left.Span[2..], CultureInfo.InvariantCulture);
        char categoryName = s.Span[0];
        int categoryIndex = "xmas".IndexOf(categoryName, StringComparison.InvariantCulture);
        ReadOnlyMemory<char> destination = s[ranges[1]];
        ComparisonRule comparisonRule = s.Span[1] switch
        {
            '<' => new LessComparisonRule(s, destination, categoryIndex, operand),
            '>' => new GreaterComparisonRule(s, destination, categoryIndex, operand),
            _ => throw new ArgumentException(new(s.Span), nameof(s))
        };
        return comparisonRule;
    }
}
