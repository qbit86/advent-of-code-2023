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

public static class PartTwoPuzzle
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        IEnumerable<Ray3<long>> allHailstoneTrajectories = lines.Select(line => Parse(line));
        // Ignore all but three of the hailstones.
        var hailstoneTrajectories = allHailstoneTrajectories.Take(3).ToList();
        // Select arbitrary hailstone as a reference.
        Ray3<long> referenceHailstoneTrajectory = hailstoneTrajectories[0];
        // Recalculate velocities relative to the reference hailstone.
        // No need to perform the Galilean transformation of coordinates by moving the reference hailstone to the origin.
        var hailstoneTrajectoriesTransformed = hailstoneTrajectories.Select(
            it => Ray3.Create(it.Position, it.Velocity - referenceHailstoneTrajectory.Velocity)).ToList();
        // Planes through reference hailstone and the paths of two other hailstones.
        var normal1 = V3.Cross(
            hailstoneTrajectoriesTransformed[1].Position - referenceHailstoneTrajectory.Position,
            hailstoneTrajectoriesTransformed[1].Velocity);
        var normal2 = V3.Cross(
            hailstoneTrajectoriesTransformed[2].Position - referenceHailstoneTrajectory.Position,
            hailstoneTrajectoriesTransformed[2].Velocity);
        // The intersection of these planes is the path of the rock in the coordinate system of the reference hailstone.
        // The trajectory (and thus the path) of the rock passes through the reference hailstone, which is stationary after the Galilean transformation.
        V3<Int128> rockDirection = V3Helpers<Int128>.Cross(normal1, normal2);
        Ray3<BigInteger> rockPath = Ray3Helpers<BigInteger>.Create(
            referenceHailstoneTrajectory.Position, rockDirection);
        // Time when two other hailstones (moving relative to the stationary reference hailstone) collide with the rock.
        long collisionTime1 = (long)IntersectionHelpers<BigInteger>.GetIntersectionTime(
            hailstoneTrajectoriesTransformed[1], rockPath);
        long collisionTime2 = (long)IntersectionHelpers<BigInteger>.GetIntersectionTime(
            hailstoneTrajectoriesTransformed[2], rockPath);
        // Points where two other hailstones collide with the rock.
        V3<long> collisionPoint1 =
            hailstoneTrajectories[1].Position + collisionTime1 * hailstoneTrajectories[1].Velocity;
        V3<long> collisionPoint2 =
            hailstoneTrajectories[2].Position + collisionTime2 * hailstoneTrajectories[2].Velocity;

        // Restore the rock's initial position given two points on its trajectory.
        V3<long> rockVelocityInverse = (collisionPoint1 - collisionPoint2) / (collisionTime2 - collisionTime1);
        V3<long> rockOrigin = collisionPoint2 + collisionTime2 * rockVelocityInverse;
        long result = rockOrigin.X + rockOrigin.Y + rockOrigin.Z;
        return result;
    }

    private static Ray3<long> Parse(ReadOnlySpan<char> line)
    {
        Span<Range> ranges = stackalloc Range[7];
        int count = line.SplitAny(ranges, ",@", StringSplitOptions.TrimEntries);
        if (count is not 6)
            throw new ArgumentException(null, nameof(line));

        V3<long> position = new(P(line[ranges[0]]), P(line[ranges[1]]), P(line[ranges[2]]));
        V3<long> velocity = new(P(line[ranges[3]]), P(line[ranges[4]]), P(line[ranges[5]]));
        return new(position, velocity);

        static long P(ReadOnlySpan<char> s)
        {
            return long.Parse(s, CultureInfo.InvariantCulture);
        }
    }
}
