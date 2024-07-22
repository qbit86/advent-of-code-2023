using System;
using System.Collections.Generic;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

internal readonly struct Node
{
    public V3<long> CollisionTimes { get; }
    public double Volume { get; }

    private Node(V3<long> collisionTimes, double volume)
    {
        CollisionTimes = collisionTimes;
        Volume = volume;
    }

    internal static Node Create<TRays>(TRays rays, V3<long> collisionTimes) where TRays : IReadOnlyList<Ray3<double>>
    {
        Span<V3<double>> collisions = stackalloc V3<double>[3];
        for (int i = 0; i < collisions.Length; ++i)
        {
            Ray3<double> ray = rays[i];
            long collisionTime = collisionTimes[i];
            V3<double> collision = ray.Position + collisionTime * ray.Velocity;
            collisions[i] = collision;
        }

        V3<double> left = collisions[1] - collisions[0];
        V3<double> right = collisions[2] - collisions[0];
        var cross = V3.Cross(left, right);
        return new(collisionTimes, cross.Length());
    }

    public override string ToString() => $"{{ CollisionTimes = {CollisionTimes}, Volume = {Volume} }}";
}
