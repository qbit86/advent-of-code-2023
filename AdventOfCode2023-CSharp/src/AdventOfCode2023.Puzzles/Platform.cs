using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode2023;

internal readonly struct Platform
{
    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    private readonly char[] _data;

    private Platform(char[] data, int width, int height, int length)
    {
        Debug.Assert(data is not null);
        Debug.Assert(width >= 0);
        Debug.Assert(height >= 0);
        Debug.Assert(length >= 0);
        Debug.Assert(length <= data.Length);
        _data = data;
        Width = width;
        Height = height;
        Length = length;
    }

    internal static Platform Empty { get; } = new(Array.Empty<char>(), 0, 0, 0);

    internal ReadOnlySpan<char> Data => _data;
    private int Width { get; }
    private int Height { get; }
    internal int Length { get; }

    public override string ToString()
    {
        StringBuilder builder = new(Length + Height);
        for (int rowIndex = 0; rowIndex < Height; ++rowIndex)
        {
            if (rowIndex > 0)
                builder.AppendLine();
            Span<char> row = At(rowIndex);
            builder.Append(row);
        }

        return builder.ToString();
    }

    internal static Platform Create<TRows>(TRows rows, char[] destination)
        where TRows : IReadOnlyList<string>
    {
        Debug.Assert(rows is not null);
        int height = rows.Count;
        Debug.Assert(height > 0);
        int width = rows[0].Length;
        Debug.Assert(width > 0);
        Debug.Assert(destination is not null);
        for (int rowIndex = 0; rowIndex < height; ++rowIndex)
        {
            string row = rows[rowIndex];
            Span<char> destinationRow = destination.AsSpan().Slice(rowIndex * width, width);
            row.AsSpan().CopyTo(destinationRow);
        }

        return Create(destination, width, height);
    }

    private static Platform Create(char[] data, int width, int height)
    {
        Debug.Assert(data is not null);
        Debug.Assert(width > 0);
        Debug.Assert(height > 0);
        int length = width * height;
        Debug.Assert(length <= data.Length);
        return new(data, width, height, length);
    }

    internal Platform DeepClone(char[] destination)
    {
        Debug.Assert(destination is not null);
        Debug.Assert(Length <= destination.Length);
        _data.AsSpan(0, Length).CopyTo(destination.AsSpan(0, Length));
        return new(destination, Width, Height, Length);
    }

    internal Platform Transpose(char[] destination)
    {
        Debug.Assert(destination is not null);
        Debug.Assert(Length <= destination.Length);
        int newWidth = Height;
        int newHeight = Width;
        for (int newRowIndex = 0; newRowIndex < newHeight; ++newRowIndex)
        {
            Span<char> destinationRow = destination.AsSpan().Slice(newRowIndex * newWidth, newWidth);
            for (int newColumnIndex = 0; newColumnIndex < newWidth; ++newColumnIndex)
                destinationRow[newColumnIndex] = At(newColumnIndex)[newRowIndex];
        }

        return Create(destination, newWidth, newHeight);
    }

    internal Platform TransposeSecondary(char[] destination)
    {
        Debug.Assert(destination is not null);
        Debug.Assert(Length <= destination.Length);
        int newWidth = Height;
        int newHeight = Width;
        for (int newRowIndex = 0; newRowIndex < newHeight; ++newRowIndex)
        {
            Span<char> destinationRow = destination.AsSpan().Slice(newRowIndex * newWidth, newWidth);
            for (int newColumnIndex = 0; newColumnIndex < newWidth; ++newColumnIndex)
                destinationRow[newColumnIndex] = At(Width - newColumnIndex - 1)[Height - newRowIndex - 1];
        }

        return Create(destination, newWidth, newHeight);
    }

    internal void RollAllRoundedRocks()
    {
        for (int rowIndex = 0; rowIndex < Height; ++rowIndex)
            RollAllRoundedRocks(rowIndex);
    }

    internal int ComputeCurrentLoad() => Enumerable.Range(0, Height).Select(ComputeCurrentLoad).Sum();

    private void RollAllRoundedRocks(int rowIndex)
    {
        Debug.Assert((uint)rowIndex < (uint)Height);
        Span<char> row = At(rowIndex);
        Span<Range> ranges = stackalloc Range[Width / 2];
        int rangeCount = ((ReadOnlySpan<char>)row).Split(ranges, Tiles.CubeRock, SplitOptions);
        ranges = ranges[..rangeCount];
        foreach (Range range in ranges)
        {
            Span<char> span = row[range];
            int roundedRockCount = span.Count(Tiles.RoundedRock);
            if (roundedRockCount is 0)
                continue;
            span[..roundedRockCount].Fill(Tiles.RoundedRock);
            span[roundedRockCount..].Fill(Tiles.Empty);
        }
    }

    private int ComputeCurrentLoad(int rowIndex)
    {
        Debug.Assert((uint)rowIndex < (uint)Height);
        Span<char> row = At(rowIndex);
        Span<Range> ranges = stackalloc Range[Width / 2];
        int rangeCount = ((ReadOnlySpan<char>)row).SplitAny(ranges, "#.", SplitOptions);
        ranges = ranges[..rangeCount];
        int totalLoad = 0;
        foreach (Range range in ranges)
        {
            (int offset, int length) = range.GetOffsetAndLength(row.Length);
            int leftLoad = row.Length - offset;
            int rightInclusiveLoad = row.Length - (offset + length) + 1;
            totalLoad += (leftLoad + rightInclusiveLoad) * length / 2;
        }

        return totalLoad;
    }

    private Span<char> At(int rowIndex)
    {
        Debug.Assert((uint)rowIndex < (uint)Height);
        int offset = rowIndex * Width;
        return _data.AsSpan().Slice(offset, Width);
    }
}
