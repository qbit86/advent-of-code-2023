using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Numerics;

public readonly partial struct V3<T> : IEquatable<V3<T>>, IFormattable
    where T : INumberBase<T>
{
    internal readonly T _x;
    private readonly T _y;
    private readonly T _z;

    public V3(T value) : this(value, value, value) { }

    public V3(T x, T y, T z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public T X => _x;

    public T Y => _y;

    public T Z => _z;

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.GetElement(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T LengthSquared() => V3.Dot(this, this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator +(V3<T> left, V3<T> right) =>
        new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator /(V3<T> left, V3<T> right) =>
        new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator /(V3<T> left, T right) => left / new V3<T>(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator *(V3<T> left, V3<T> right) =>
        new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator *(V3<T> left, T right) => left * new V3<T>(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator *(T left, V3<T> right) => new V3<T>(left) * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator -(V3<T> left, V3<T> right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3<T> operator -(V3<T> value) => V3.Zero<T>() - value;
}
