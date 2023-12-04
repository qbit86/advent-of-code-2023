using System.Collections.Generic;
using Arborescence;

namespace AdventOfCode2023;

internal sealed record Graph(IReadOnlyList<int> ChildCountById) :
    IOutNeighborsAdjacency<int, IEnumerable<int>>
{
    public IEnumerable<int> EnumerateOutNeighbors(int vertex)
    {
        int lowerBound = vertex + 1;
        int upperBound = lowerBound + ChildCountById[vertex];
        for (int i = lowerBound; i < upperBound; ++i)
            yield return i;
    }
}
