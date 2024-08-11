using System.Numerics;
using EuclideanSpace;
using V3 = EuclideanSpace.Vector3;

namespace AdventOfCode2023;

public static class Ray3
{
    public static Ray3<T> Create<T>(Vector3<T> position, Vector3<T> velocity) where T : INumberBase<T> =>
        new(position, velocity);

    public static Ray3<T> Create<T>(T positionX, T positionY, T positionZ, T velocityX, T velocityY, T velocityZ)
        where T : INumberBase<T> =>
        new(V3.Create(positionX, positionY, positionZ), V3.Create(velocityX, velocityY, velocityZ));
}

public readonly record struct Ray3<T>(Vector3<T> Position, Vector3<T> Velocity) where T : INumberBase<T> { }
