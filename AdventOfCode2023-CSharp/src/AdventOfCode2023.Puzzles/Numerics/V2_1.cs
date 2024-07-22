using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Numerics;

public readonly partial struct V2<T> : IEquatable<V2<T>>, IFormattable
    where T : INumberBase<T>
{
    internal readonly T _x;
    private readonly T _y;

    public V2(T value) : this(value, value) { }

    public V2(T x, T y)
    {
        _x = x;
        _y = y;
    }

    public T X => _x;

    public T Y => _y;

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.GetElement(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T LengthSquared() => V2.Dot(this, this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator +(V2<T> left, V2<T> right) => new(left.X + right.X, left.Y + right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator /(V2<T> left, V2<T> right) => new(left.X / right.X, left.Y / right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator /(V2<T> left, T right) => left / new V2<T>(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator *(V2<T> left, V2<T> right) => new(left.X * right.X, left.Y * right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator *(V2<T> left, T right) => left * new V2<T>(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator *(T left, V2<T> right) => new V2<T>(left) * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator -(V2<T> left, V2<T> right) => new(left.X - right.X, left.Y - right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2<T> operator -(V2<T> value) => V2.Zero<T>() - value;
}
