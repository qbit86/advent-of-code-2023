using System.Numerics;
using EuclideanSpace;
using V3 = EuclideanSpace.Vector3;

namespace AdventOfCode2023;

internal static class V3Helpers
{
    internal static T TripleProduct<T>(Vector3<T> a, Vector3<T> b, Vector3<T> c) where T : INumberBase<T> =>
        V3.Dot(a, V3.Cross(b, c));
}

internal static class V3Helpers<TResult>
    where TResult : INumberBase<TResult>
{
    internal static Vector3<TResult> Cross<TSource>(Vector3<TSource> left, Vector3<TSource> right)
        where TSource : INumberBase<TSource>
    {
        var l = Vector3Conversions<TResult>.AsVector3(left);
        var r = Vector3Conversions<TResult>.AsVector3(right);
        return V3.Cross(l, r);
    }
}
