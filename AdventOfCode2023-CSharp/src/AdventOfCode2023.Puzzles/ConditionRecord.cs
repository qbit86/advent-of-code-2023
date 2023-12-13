using System;
using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

internal readonly record struct ConditionRecord(
    ImmutableStack<StringSegment> ObservedGroups,
    ReadOnlyMemory<int> ActualLengths)
{
    public override string ToString()
    {
        if (ObservedGroups.IsEmpty)
            return $"[] {JsonSerializer.Serialize(ActualLengths)}";

        StringSegment top = ObservedGroups.Peek();
        string s = new(top.AsSpan());
        return $"[\"{s}\", ...] {JsonSerializer.Serialize(ActualLengths)}";
    }
}
