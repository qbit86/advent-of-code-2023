using System;
using System.Collections.Generic;

namespace AdventOfCode2023;

internal sealed class PooledPlatformComparer : IEqualityComparer<PooledPlatform>
{
    private PooledPlatformComparer() { }

    internal static PooledPlatformComparer Instance { get; } = new();

    public bool Equals(PooledPlatform? x, PooledPlatform? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null)
            return false;
        return x.Data.SequenceEqual(y.Data);
    }

    public int GetHashCode(PooledPlatform obj) => throw new NotSupportedException();
}
