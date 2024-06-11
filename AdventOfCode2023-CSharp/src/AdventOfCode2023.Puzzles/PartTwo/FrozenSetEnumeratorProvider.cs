using System.Collections.Frozen;
using Arborescence;

namespace AdventOfCode2023;

internal readonly struct FrozenSetEnumeratorProvider<T> :
    IEnumeratorProvider<FrozenSet<T>, FrozenSet<T>.Enumerator>
{
    // ReSharper disable NotDisposedResourceIsReturned
    public FrozenSet<T>.Enumerator GetEnumerator(FrozenSet<T> collection) =>
        collection?.GetEnumerator() ?? GetEmptyEnumerator();

    public FrozenSet<T>.Enumerator GetEmptyEnumerator() => FrozenSet<T>.Empty.GetEnumerator();
    // ReSharper restore NotDisposedResourceIsReturned
}
