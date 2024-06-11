using System.Collections.Generic;
using System.Diagnostics;
using Arborescence;

namespace AdventOfCode2023;

file static class AboveGraphHelpers
{
    // ReSharper disable once CollectionNeverUpdated.Local
    private static readonly List<Brick> s_empty = [];

    internal static List<Brick>.Enumerator EmptyEnumerator() => s_empty.GetEnumerator();
}

internal static class AboveGraph
{
    internal static AboveGraph<TBricksById> Create<TBricksById>(TBricksById bricksAboveById)
        where TBricksById : IReadOnlyList<List<Brick>?> =>
        new(bricksAboveById);
}

internal sealed class AboveGraph<TBricksById> : IOutNeighborsAdjacency<Brick, List<Brick>.Enumerator>
    where TBricksById : IReadOnlyList<List<Brick>?>
{
    private readonly TBricksById _bricksAboveById;

    internal AboveGraph(TBricksById bricksAboveById)
    {
        Debug.Assert(bricksAboveById is not null);
        _bricksAboveById = bricksAboveById;
    }

    public List<Brick>.Enumerator EnumerateOutNeighbors(Brick vertex)
    {
        if (unchecked((uint)vertex.Id) >= unchecked((uint)_bricksAboveById.Count))
            return AboveGraphHelpers.EmptyEnumerator();
        if (_bricksAboveById[vertex.Id] is not { } list)
            return AboveGraphHelpers.EmptyEnumerator();
        return list.GetEnumerator();
    }
}
