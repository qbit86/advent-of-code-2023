using System.Numerics;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

internal static class Ray3Helpers<TResult>
    where TResult : INumberBase<TResult>
{
    internal static Ray3<TResult> Create<TSource>(Ray3<TSource> value) where TSource : INumberBase<TSource> =>
        Ray3.Create(V3Helpers<TResult>.Create(value.Position), V3Helpers<TResult>.Create(value.Velocity));

    internal static Ray3<TResult> Create<TPosition, TVelocity>(V3<TPosition> position, V3<TVelocity> velocity)
        where TPosition : INumberBase<TPosition>
        where TVelocity : INumberBase<TVelocity>
    {
        V3<TResult> p = V3Helpers<TResult>.Create(position);
        V3<TResult> v = V3Helpers<TResult>.Create(velocity);
        return Ray3.Create(p, v);
    }
}
