using System;
using System.Runtime.CompilerServices;
using Arborescence;

namespace AdventOfCode2023;

internal readonly struct ArrayEnumeratorProvider<T> : IEnumeratorProvider<T[], ArraySegment<T>.Enumerator>
{
    // ReSharper disable NotDisposedResourceIsReturned
    public ArraySegment<T>.Enumerator GetEnumerator(T[] collection) =>
        collection is not null ? new ArraySegment<T>(collection).GetEnumerator() : GetEmptyEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySegment<T>.Enumerator GetEmptyEnumerator() => ArraySegment<T>.Empty.GetEnumerator();
    // ReSharper restore NotDisposedResourceIsReturned
}
