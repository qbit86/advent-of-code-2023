using System;
using System.Collections.Immutable;
using System.Text;

namespace AdventOfCode2023;

internal readonly record struct Node(
    ReadOnlyMemory<char> Name,
    ImmutableList<ShortRange> Ranges,
    int RuleIndex)
{
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(nameof(Name) + ": ").Append(Name);
        if (RuleIndex > 0)
            builder.Append(", " + nameof(RuleIndex) + ": ").Append(RuleIndex);
        builder.Append(", " + nameof(Ranges) + ": [ ");
        for (int i = 0; i < Ranges.Count; ++i)
        {
            if (i > 0)
                builder.Append(", ");
            builder.Append(Ranges[i]);
        }

        builder.Append(" ]");
        return true;
    }
}
