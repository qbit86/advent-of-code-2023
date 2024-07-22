using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace AdventOfCode.Numerics;

public readonly partial struct V2<T>
{
    public bool Equals(V2<T> other) =>
        EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y);

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is V2<T> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(V2<T> left, V2<T> right) => left.Equals(right);

    public static bool operator !=(V2<T> left, V2<T> right) => !left.Equals(right);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        FormattableString formattable = $"<{X}, {Y}>";
        return formattable.ToString(formatProvider);
    }

    public override string ToString() => ToString("G", CultureInfo.InvariantCulture);
}
