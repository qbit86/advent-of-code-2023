using System.Numerics;
using EuclideanSpace;
using V2 = EuclideanSpace.Vector2;

namespace AdventOfCode2023;

public static class Ray2
{
    public static Ray2<T> Create<T>(Vector2<T> position, Vector2<T> velocity) where T : INumberBase<T> =>
        new(position, velocity);

    public static Ray2<T> Create<T>(T positionX, T positionY, T velocityX, T velocityY)
        where T : INumberBase<T> =>
        new(V2.Create(positionX, positionY), V2.Create(velocityX, velocityY));
}

public readonly record struct Ray2<T>(Vector2<T> Position, Vector2<T> Velocity) where T : INumberBase<T> { }
