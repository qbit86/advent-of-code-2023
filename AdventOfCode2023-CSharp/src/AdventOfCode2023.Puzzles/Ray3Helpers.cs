using System.Numerics;
using EuclideanSpace;

namespace AdventOfCode2023;

internal static class Ray3Helpers<TResult>
    where TResult : INumberBase<TResult>
{
    internal static Ray3<TResult> Create<TSource>(Ray3<TSource> value) where TSource : INumberBase<TSource> =>
        Ray3.Create(
            Vector3Conversions<TResult>.AsVector3(value.Position),
            Vector3Conversions<TResult>.AsVector3(value.Velocity));

    internal static Ray3<TResult> Create<TPosition, TVelocity>(Vector3<TPosition> position, Vector3<TVelocity> velocity)
        where TPosition : INumberBase<TPosition>
        where TVelocity : INumberBase<TVelocity>
    {
        var p = Vector3Conversions<TResult>.AsVector3(position);
        var v = Vector3Conversions<TResult>.AsVector3(velocity);
        return Ray3.Create(p, v);
    }
}
