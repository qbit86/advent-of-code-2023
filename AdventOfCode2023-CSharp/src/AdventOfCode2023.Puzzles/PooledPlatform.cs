using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023;

internal sealed class PooledPlatform : IDisposable
{
    private char[] _bufferFromPool;
    private Platform _platform;

    private PooledPlatform(char[] bufferFromPool, Platform platform)
    {
        Debug.Assert(bufferFromPool is not null);
        _bufferFromPool = bufferFromPool;
        _platform = platform;
    }

    internal ReadOnlySpan<char> Data => _platform.Data;
    private int Length => _platform.Length;

    public void Dispose()
    {
        if (_bufferFromPool is not { Length: > 0 })
            return;
        char[] bufferFromPool = _bufferFromPool;
        _bufferFromPool = Array.Empty<char>();
        ArrayPool<char>.Shared.Return(bufferFromPool);
        _platform = Platform.Empty;
    }

    public override string ToString() => _platform.ToString();

    internal static PooledPlatform Create<TRows>(TRows rows)
        where TRows : IReadOnlyList<string>
    {
        Debug.Assert(rows is not null);
        int height = rows.Count;
        Debug.Assert(height > 0);
        int width = rows[0].Length;
        Debug.Assert(width > 0);
        int length = width * height;
        char[] bufferFromPool = ArrayPool<char>.Shared.Rent(length);
        var platform = Platform.Create(rows, bufferFromPool);
        return new(bufferFromPool, platform);
    }

    internal PooledPlatform DeepClone()
    {
        char[] bufferFromPool = ArrayPool<char>.Shared.Rent(Length);
        Platform platform = _platform.DeepClone(bufferFromPool);
        return new(bufferFromPool, platform);
    }

    internal void Transpose()
    {
        char[] bufferFromPool = _bufferFromPool;
        char[] destination = ArrayPool<char>.Shared.Rent(Length);
        _platform = _platform.Transpose(destination);
        _bufferFromPool = destination;

        if (bufferFromPool is { Length: > 0 })
            ArrayPool<char>.Shared.Return(bufferFromPool);
    }

    internal void TransposeSecondary()
    {
        char[] bufferFromPool = _bufferFromPool;
        char[] destination = ArrayPool<char>.Shared.Rent(Length);
        _platform = _platform.TransposeSecondary(destination);
        _bufferFromPool = destination;

        if (bufferFromPool is { Length: > 0 })
            ArrayPool<char>.Shared.Return(bufferFromPool);
    }

    internal void RollAllRoundedRocks() => _platform.RollAllRoundedRocks();

    internal int ComputeCurrentLoad() => _platform.ComputeCurrentLoad();
}
