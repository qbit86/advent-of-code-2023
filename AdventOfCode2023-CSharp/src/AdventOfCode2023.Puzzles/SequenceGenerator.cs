using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

internal static class SequenceGenerator
{
    internal static SequenceGenerator<TRays> Create<TRays>(TRays rays)
        where TRays : IReadOnlyList<Ray3<double>> => new(rays);
}

public sealed class SequenceGenerator<TRays>
    where TRays : IReadOnlyList<Ray3<double>>
{
    private readonly List<(Node Node, int Index, int Sign)> _candidates = [];
    private readonly TRays _rays;

    internal SequenceGenerator(TRays rays) => _rays = rays ?? throw new ArgumentNullException(nameof(rays));

    internal Node CreateNode(V3<long> collisionTimes) => Node.Create(_rays, collisionTimes);

    internal IEnumerable<Node> Generate(Node node) => GenerateIterator(node);

    private IEnumerable<Node> GenerateIterator(Node node)
    {
        Node current = node;
        do
        {
            if (!TryGetNext(current, out Node next))
                yield break;
            yield return next;
            current = next;
        } while (true);
    }

    private bool TryGetNext(Node node, out Node next)
    {
        _candidates.Clear();
        for (int index = 0; index < V3.Count; ++index)
        {
            {
                const int sign = 1;
                if (TryGetNeighbor(node, index, sign, out Node neighbor))
                    _candidates.Add((neighbor, index, sign));
            }
            {
                const int sign = -1;
                if (TryGetNeighbor(node, index, sign, out Node neighbor))
                    _candidates.Add((neighbor, index, sign));
            }
        }

        if (_candidates.Count is 0)
            return TryHelpers.None(out next);
        (Node Node, int Index, int Sign) direction = _candidates.MinBy(it => it.Node.Volume);
        return GradientHelpers.TryGetNeighbor(_rays, node, direction.Index, direction.Sign, out next);
    }

    private bool TryGetNeighbor(Node node, int index, int sign, out Node neighbor)
    {
        Span<long> collisionTimeElements = stackalloc long[V3.Count];
        for (int i = 0; i < V3.Count; ++i)
        {
            long element = node.CollisionTimes.GetElement(i);
            collisionTimeElements[i] = i == index
                ? unchecked(element + sign)
                : element;
        }

        var collisionTimes = V3.Create<long>(collisionTimeElements);
        neighbor = Node.Create(_rays, collisionTimes);
        return collisionTimeElements[index] > 0 && neighbor.Volume < node.Volume;
    }
}
