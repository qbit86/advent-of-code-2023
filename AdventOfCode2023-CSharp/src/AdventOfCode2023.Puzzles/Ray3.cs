using System.Numerics;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

public static class Ray3
{
    public static Ray3<T> Create<T>(V3<T> position, V3<T> velocity) where T : INumberBase<T> =>
        new(position, velocity);

    public static Ray3<T> Create<T>(T positionX, T positionY, T positionZ, T velocityX, T velocityY, T velocityZ)
        where T : INumberBase<T> =>
        new(V3.Create(positionX, positionY, positionZ), V3.Create(velocityX, velocityY, velocityZ));
}

public readonly record struct Ray3<T>(V3<T> Position, V3<T> Velocity) where T : INumberBase<T> { }
