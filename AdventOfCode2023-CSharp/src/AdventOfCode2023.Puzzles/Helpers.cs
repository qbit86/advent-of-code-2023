#define AOC_CACHE_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Primitives;
using StringCreationState = (System.ReadOnlyMemory<char> ObservedGroup, int IndexOfPlaceholder);

namespace AdventOfCode2023;

internal static class Helpers
{
    internal const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    internal static long ComputePossibleArrangementCount(ConditionRecord conditionRecord)
    {
        Dictionary<ConditionRecord, long> cache = new(ConditionRecordComparer.Instance);
        return ComputePossibleArrangementCount(conditionRecord, cache);
    }

    private static long ComputePossibleArrangementCount<TCache>(ConditionRecord partialSolution, TCache cache)
        where TCache : IDictionary<ConditionRecord, long>
    {
#if AOC_CACHE_ENABLED
        if (cache.TryGetValue(partialSolution, out long cachedResult))
            return cachedResult;
#endif

        (ImmutableStack<StringSegment> observedGroups, ReadOnlyMemory<int> actualLengths) = partialSolution;
        if (observedGroups.IsEmpty)
            return Cache(actualLengths.IsEmpty ? 1 : 0);

        ImmutableStack<StringSegment> poppedObservedGroups = observedGroups.Pop(out StringSegment observedGroup);
        ReadOnlySpan<char> observedGroupSpan = observedGroup.AsSpan();
        if (observedGroupSpan.IsEmpty)
            return Cache(ComputePossibleArrangementCount(new(poppedObservedGroups, actualLengths), cache));
        if (actualLengths.IsEmpty)
            return Cache(observedGroups.Any(it => it.AsSpan().Contains('#')) ? 0 : 1);

        int actualLength = actualLengths.Span[0];

        int indexOfPlaceholder = observedGroupSpan.IndexOf('?');
        if (indexOfPlaceholder < 0)
        {
            if (observedGroup.Length != actualLength)
                return Cache(0);

            ConditionRecord childPartialSolution = new(poppedObservedGroups, actualLengths[1..]);
            return Cache(ComputePossibleArrangementCount(childPartialSolution, cache));
        }

        string childObservedGroup = string.Create(observedGroup.Length,
            new StringCreationState(observedGroup, indexOfPlaceholder), PopulateChildSpan);
        long result = ComputePossibleArrangementCount(
            new(poppedObservedGroups.Push(childObservedGroup), actualLengths), cache);

        StringSegment rightObservedGroup = observedGroup.Subsegment(indexOfPlaceholder + 1);
        ImmutableStack<StringSegment> childObservedGroups = poppedObservedGroups;
        if (!rightObservedGroup.AsSpan().IsEmpty)
            childObservedGroups = childObservedGroups.Push(rightObservedGroup);
        StringSegment leftObservedGroup = observedGroup.Subsegment(0, indexOfPlaceholder);
        if (!leftObservedGroup.AsSpan().IsEmpty)
            childObservedGroups = childObservedGroups.Push(leftObservedGroup);

        result += ComputePossibleArrangementCount(new(childObservedGroups, actualLengths), cache);
        return Cache(result);

        static void PopulateChildSpan(Span<char> span, StringCreationState state)
        {
            ReadOnlySpan<char> observedGroupSpan = state.ObservedGroup.Span;
            for (int i = 0; i < span.Length; ++i)
                span[i] = i == state.IndexOfPlaceholder ? '#' : observedGroupSpan[i];
        }

        long Cache(long valueToReturn)
        {
#if AOC_CACHE_ENABLED
            cache.Add(partialSolution, valueToReturn);
#endif
            return valueToReturn;
        }
    }
}
