using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023;

using Neighbors = (string Left, string Right);

internal sealed class Network
{
    private readonly FrozenDictionary<string, Neighbors> _neighborsByNode;

    private Network(FrozenDictionary<string, Neighbors> neighborsByNode) => _neighborsByNode = neighborsByNode;

    internal IReadOnlyDictionary<string, Neighbors> NeighborsByNode => _neighborsByNode;

    internal static Network Parse(IEnumerable<string> lines)
    {
        Dictionary<string, Neighbors> neighborsByNode =
            lines.TryGetNonEnumeratedCount(out int count) ? new(count) : new();
        foreach (string line in lines)
        {
            string node = line[..3];
            string left = line.Substring(7, 3);
            string right = line.Substring(12, 3);
            Neighbors neighbors = (left, right);
            neighborsByNode.Add(node, neighbors);
        }

        return new(neighborsByNode.ToFrozenDictionary());
    }
}
