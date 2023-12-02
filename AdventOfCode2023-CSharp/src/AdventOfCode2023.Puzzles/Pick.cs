using System;
using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

internal readonly record struct Pick(int RedCubeCount, int GreenCubeCount, int BlueCubeCount)
{
    internal static Pick Parse(StringSegment pickSegment) => Parse(pickSegment.AsSpan());

    internal static Pick Parse(ReadOnlySpan<char> pickSpan)
    {
        Span<Range> colorCountPairRanges = stackalloc Range[3];
        int colorCountPairCount = pickSpan.Split(colorCountPairRanges, ',', StringSplitOptions.TrimEntries);
        if (colorCountPairCount <= 0)
        {
            throw new ArgumentException(
                $"{nameof(colorCountPairCount)} should be positive, but was {colorCountPairCount} for `{pickSpan}`.",
                nameof(pickSpan));
        }

        Span<Range> colorCountPairParts = stackalloc Range[2];
        int redCubeCount = 0;
        int greenCubeCount = 0;
        int blueCubeCount = 0;
        for (int i = 0; i < colorCountPairCount; ++i)
        {
            Range colorCountPairRange = colorCountPairRanges[i];
            ReadOnlySpan<char> colorCountPairSpan = pickSpan[colorCountPairRange];
            int partCount = colorCountPairSpan.Split(colorCountPairParts, ' ', StringSplitOptions.TrimEntries);
            if (partCount is not 2)
            {
                throw new ArgumentException(
                    $"{nameof(partCount)} should be 2, but was {partCount} for `{colorCountPairSpan}`.",
                    nameof(pickSpan));
            }

            Range countRange = colorCountPairParts[0];
            int count = int.Parse(colorCountPairSpan[countRange], CultureInfo.InvariantCulture);
            Range colorRange = colorCountPairParts[1];
            ReadOnlySpan<char> color = colorCountPairSpan[colorRange];
            switch (color)
            {
                case "red":
                    redCubeCount += count;
                    break;
                case "green":
                    greenCubeCount += count;
                    break;
                case "blue":
                    blueCubeCount += count;
                    break;
                default:
                    throw new ArgumentException($"Unexpected color {color} in `{colorCountPairSpan}`.",
                        nameof(pickSpan));
            }
        }

        return new(redCubeCount, greenCubeCount, blueCubeCount);
    }
}
