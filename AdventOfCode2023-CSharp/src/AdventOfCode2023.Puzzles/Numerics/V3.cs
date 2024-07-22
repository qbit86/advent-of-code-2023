using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Numerics;

public static class V3
{
    internal const int Count = 3;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Create<T>(T x, T y, T z) where T : INumberBase<T> => new(x, y, z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Create<T>(ReadOnlySpan<T> elements) where T : INumberBase<T>
    {
        if (elements.Length < Count)
            throw new ArgumentException(null, nameof(elements));

        return new V3<T>(elements[0], elements[1], elements[2]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Zero<T>() where T : INumberBase<T> => new(T.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> One<T>() where T : INumberBase<T> => new(T.One);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> UnitX<T>() where T : INumberBase<T> => new(T.One, T.Zero, T.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> UnitY<T>() where T : INumberBase<T> => new(T.Zero, T.One, T.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> UnitZ<T>() where T : INumberBase<T> => new(T.Zero, T.Zero, T.One);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Add<T>(V3<T> left, V3<T> right) where T : INumberBase<T> => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Divide<T>(V3<T> left, V3<T> right) where T : INumberBase<T> => left / right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Divide<T>(V3<T> left, T divisor) where T : INumberBase<T> => left / divisor;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Multiply<T>(V3<T> left, V3<T> right) where T : INumberBase<T> => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Multiply<T>(V3<T> left, T right) where T : INumberBase<T> => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Multiply<T>(T left, V3<T> right) where T : INumberBase<T> => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Subtract<T>(V3<T> left, V3<T> right) where T : INumberBase<T> => left - right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Negate<T>(V3<T> value) where T : INumberBase<T> => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Distance<T>(V3<T> left, V3<T> right) where T : INumberBase<T>, IRootFunctions<T>
    {
        T distanceSquared = DistanceSquared(left, right);
        return T.Sqrt(distanceSquared);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T DistanceSquared<T>(V3<T> left, V3<T> right) where T : INumberBase<T>
    {
        V3<T> difference = right - left;
        return Dot(difference, difference);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Dot<T>(V3<T> left, V3<T> right) where T : INumberBase<T> =>
        left.X * right.X + left.Y * right.Y + left.Z * right.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Cross<T>(V3<T> left, V3<T> right) where T : INumberBase<T> =>
        new(
            left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Lerp<T>(V3<T> left, V3<T> right, T amount) where T : INumberBase<T> =>
        left * (T.One - amount) + right * amount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Normalize<T>(V3<T> value) where T : INumberBase<T>, IRootFunctions<T> => value / Length(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Length<T>(this V3<T> value) where T : INumberBase<T>, IRootFunctions<T>
    {
        T lengthSquared = value.LengthSquared();
        return T.Sqrt(lengthSquared);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Max<T>(V3<T> left, V3<T> right) where T : INumber<T> =>
        new(T.Max(left.X, right.X), T.Max(left.Y, right.Y), T.Max(left.Z, right.Z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> Min<T>(V3<T> left, V3<T> right) where T : INumber<T> =>
        new(T.Min(left.X, right.X), T.Min(left.Y, right.Y), T.Min(left.Z, right.Z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T GetElement<T>(this V3<T> vector, int index)
        where T : INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);

        return vector.GetElementUnsafe(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe<T>(in this V3<T> vector, int index)
        where T : INumberBase<T>
    {
        Debug.Assert(index is >= 0 and < Count);
        ref T address = ref Unsafe.AsRef(in vector._x);
        return Unsafe.Add(ref address, index);
    }
}
