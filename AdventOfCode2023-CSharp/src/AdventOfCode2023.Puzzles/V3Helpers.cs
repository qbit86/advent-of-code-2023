using System.Numerics;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

internal static class V3Helpers
{
    internal static T TripleProduct<T>(V3<T> a, V3<T> b, V3<T> c) where T : INumberBase<T> =>
        V3.Dot(a, V3.Cross(b, c));
}

internal static class V3Helpers<TResult>
    where TResult : INumberBase<TResult>
{
    internal static V3<TResult> Create<TSource>(V3<TSource> value) where TSource : INumberBase<TSource> =>
        V3.Create(TResult.CreateChecked(value.X), TResult.CreateChecked(value.Y), TResult.CreateChecked(value.Z));

    internal static V3<TResult> Cross<TSource>(V3<TSource> left, V3<TSource> right) where TSource : INumberBase<TSource>
    {
        V3<TResult> l = Create(left);
        V3<TResult> r = Create(right);
        return V3.Cross(l, r);
    }
}
