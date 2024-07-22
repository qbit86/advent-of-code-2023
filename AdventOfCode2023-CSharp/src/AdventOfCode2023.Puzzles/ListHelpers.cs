using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal static class ListHelpers<T>
{
    internal static void SwapRemoveAt<TList>(TList list, int index)
        where TList : IList<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, list.Count);
        int lastInclusive = list.Count - 1;
        if (index != lastInclusive)
            list[index] = list[lastInclusive];
        list.RemoveAt(lastInclusive);
    }

    internal static void SwapRemoveAt<TList>(TList list, int index, out T value)
        where TList : IList<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, list.Count);
        value = list[index];
        int lastInclusive = list.Count - 1;
        if (index != lastInclusive)
            list[index] = list[lastInclusive];
        list.RemoveAt(lastInclusive);
    }
}
