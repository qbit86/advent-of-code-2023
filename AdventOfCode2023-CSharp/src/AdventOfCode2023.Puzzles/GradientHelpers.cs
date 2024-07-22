using System;
using System.Collections.Generic;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

internal static class GradientHelpers
{
    internal static bool TryGetNeighbor<TRays>(TRays rays, Node node, int index, int sign, out Node neighbor)
        where TRays : IReadOnlyList<Ray3<double>>
    {
        Span<long> collisionTimeElements = stackalloc long[V3.Count];
        {
            for (int i = 0; i < V3.Count; ++i)
            {
                long element = node.CollisionTimes.GetElement(i);
                collisionTimeElements[i] = i == index
                    ? unchecked(element + sign)
                    : element;
            }

            var collisionTimes = V3.Create<long>(collisionTimeElements);
            neighbor = Node.Create(rays, collisionTimes);
            if (collisionTimeElements[index] <= 0 || neighbor.Volume >= node.Volume)
                return false;
        }

        Node current = neighbor;
        for (int step = 2;; step <<= 1)
        {
            for (int i = 0; i < V3.Count; ++i)
            {
                long element = node.CollisionTimes.GetElement(i);
                collisionTimeElements[i] = i == index
                    ? unchecked(element + sign * step)
                    : element;
            }

            var collisionTimes = V3.Create<long>(collisionTimeElements);
            var candidate = Node.Create(rays, collisionTimes);
            if (collisionTimeElements[index] <= 0 || candidate.Volume >= current.Volume)
                return true;
            current = neighbor = candidate;
        }
    }
}
