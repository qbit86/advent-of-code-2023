using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace AdventOfCode.Numerics;

public readonly partial struct V3<T>
{
    public bool Equals(V3<T> other) =>
        EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y) &&
        EqualityComparer<T>.Default.Equals(Z, other.Z);

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is V3<T> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static bool operator ==(V3<T> left, V3<T> right) => left.Equals(right);

    public static bool operator !=(V3<T> left, V3<T> right) => !left.Equals(right);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        FormattableString formattable = $"<{X}, {Y}, {Z}>";
        return formattable.ToString(formatProvider);
    }

    public override string ToString() => ToString("G", CultureInfo.InvariantCulture);
}
