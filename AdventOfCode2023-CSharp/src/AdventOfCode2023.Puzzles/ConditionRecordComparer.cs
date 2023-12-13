using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

internal sealed class ConditionRecordComparer : IEqualityComparer<ConditionRecord>
{
    private ConditionRecordComparer() { }

    internal static ConditionRecordComparer Instance { get; } = new();

    public bool Equals(ConditionRecord x, ConditionRecord y)
    {
        if (x.ActualLengths.Length != y.ActualLengths.Length)
            return false;

        if (x.ObservedGroups.IsEmpty || y.ObservedGroups.IsEmpty)
            return x.ObservedGroups.IsEmpty && y.ObservedGroups.IsEmpty;

        ImmutableStack<StringSegment> leftTail = x.ObservedGroups.Pop(out StringSegment left);
        ImmutableStack<StringSegment> rightTail = y.ObservedGroups.Pop(out StringSegment right);

        if (!left.AsSpan().SequenceEqual(right.AsSpan()))
            return false;

        return leftTail.SequenceEqual(rightTail, StringSegmentFastComparer.Instance);
    }

    public int GetHashCode(ConditionRecord obj)
    {
        HashCode hashCode = new();
        hashCode.Add(obj.ActualLengths.Length);
        if (!obj.ObservedGroups.IsEmpty)
        {
            StringSegment observedGroup = obj.ObservedGroups.Peek();
            hashCode.Add(observedGroup.Offset);
            hashCode.Add(observedGroup.Length);
        }

        return hashCode.ToHashCode();
    }
}

internal sealed class StringSegmentFastComparer : IEqualityComparer<StringSegment>
{
    private StringSegmentFastComparer() { }

    internal static StringSegmentFastComparer Instance { get; } = new();

    public bool Equals(StringSegment x, StringSegment y) => x.Offset == y.Offset && x.Length == y.Length;

    public int GetHashCode(StringSegment obj) => throw new NotSupportedException();
}
