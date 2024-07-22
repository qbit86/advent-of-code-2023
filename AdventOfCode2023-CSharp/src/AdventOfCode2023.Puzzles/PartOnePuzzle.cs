using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Numerics;

namespace AdventOfCode2023;

public static class PartOnePuzzle
{
    public static async Task<long> SolveAsync(string path, long testAreaLowerBound, long testAreaUpperBoundInclusive)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines, testAreaLowerBound, testAreaUpperBoundInclusive);
    }

    private static long Solve<TLines>(TLines lines, long testAreaLowerBound, long testAreaUpperBoundInclusive)
        where TLines : IReadOnlyList<string>
    {
        IEnumerable<Ray2<long>> hailstonesInt64 = lines.Select(line => Parse(line));
        var hailstones = hailstonesInt64.Select(it => new Ray2<double>(
                ConvertHelpers<double>.From(it.Position), ConvertHelpers<double>.From(it.Velocity)))
            .ToList();
        int count = 0;
        for (int i = 0; i < hailstones.Count; ++i)
        {
            Ray2<double> left = hailstones[i];
            for (int j = i + 1; j < hailstones.Count; ++j)
            {
                Ray2<double> right = hailstones[j];
                if (!IntersectionHelpers.TryGetIntersection(left, right, out V2<double> intersection))
                    continue;

                if (intersection.X < testAreaLowerBound || intersection.X > testAreaUpperBoundInclusive)
                    continue;

                if (intersection.Y < testAreaLowerBound || intersection.Y > testAreaUpperBoundInclusive)
                    continue;

                count += 1;
            }
        }

        return count;
    }

    private static Ray2<long> Parse(ReadOnlySpan<char> line)
    {
        Span<Range> ranges = stackalloc Range[7];
        int count = line.SplitAny(ranges, ",@", StringSplitOptions.TrimEntries);
        if (count is not 6)
            throw new ArgumentException(null, nameof(line));

        V2<long> position = new(P(line[ranges[0]]), P(line[ranges[1]]));
        V2<long> velocity = new(P(line[ranges[3]]), P(line[ranges[4]]));
        return new(position, velocity);

        static long P(ReadOnlySpan<char> s)
        {
            return long.Parse(s, CultureInfo.InvariantCulture);
        }
    }
}

file static class ConvertHelpers<TDestination>
    where TDestination : INumberBase<TDestination>
{
    internal static V2<TDestination> From<TSource>(V2<TSource> value) where TSource : INumberBase<TSource> =>
        V2.Create(TDestination.CreateChecked(value.X), TDestination.CreateChecked(value.Y));
}
