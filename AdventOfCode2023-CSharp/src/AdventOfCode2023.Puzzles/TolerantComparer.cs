using System;
using System.Collections.Generic;
using System.Numerics;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

file static class TolerantComparerHelpers<T> where T : INumberBase<T>, IComparable<T>
{
    internal static readonly TolerantComparer<T> DefaultComparer = new(T.CreateChecked(Half.Epsilon));
}

public static class TolerantComparer
{
    public static TolerantComparer<T> Default<T>() where T : INumberBase<T>, IComparable<T> =>
        TolerantComparerHelpers<T>.DefaultComparer;
}

public sealed class TolerantComparer<T> : IEqualityComparer<T>, IEqualityComparer<V2<T>>, IEqualityComparer<V3<T>>
    where T : INumberBase<T>, IComparable<T>
{
    private readonly T _tolerance;

    public TolerantComparer(T tolerance)
    {
        if (tolerance is null)
            throw new ArgumentNullException(nameof(tolerance));

        _tolerance = tolerance;
    }

    public bool Equals(T? x, T? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null)
            return false;
        return EqualsUnchecked(x, y);
    }

    public int GetHashCode(T obj) => obj.GetHashCode();

    public bool Equals(V2<T> x, V2<T> y) => EqualsUnchecked(x.X, y.X) && EqualsUnchecked(x.Y, y.Y);

    public int GetHashCode(V2<T> obj) => obj.GetHashCode();

    public bool Equals(V3<T> x, V3<T> y) =>
        EqualsUnchecked(x.X, y.X) && EqualsUnchecked(x.Y, y.Y) && EqualsUnchecked(x.Z, y.Z);

    public int GetHashCode(V3<T> obj) => obj.GetHashCode();

    private bool EqualsUnchecked(T x, T y)
    {
        T difference = T.Abs(y - x);
        return difference.CompareTo(_tolerance) <= 0;
    }
}
