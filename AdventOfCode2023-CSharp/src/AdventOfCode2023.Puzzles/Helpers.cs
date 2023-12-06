using System.Numerics;

namespace AdventOfCode2023;

internal static class Helpers
{
    internal static T CountWays<T>(T time, T distance)
        where T : INumberBase<T>
    {
        double t = double.CreateChecked(time);
        double discriminant = t * t - 4.0 * double.CreateChecked(distance);
        double sqrt = double.Sqrt(discriminant);
        double left = 0.5 * (t - sqrt);
        double right = 0.5 * (t + sqrt);
        T lower = T.CreateChecked(double.Floor(left)) + T.One;
        T upperInclusive = T.CreateChecked(double.Ceiling(right)) - T.One;
        return upperInclusive - lower + T.One;
    }
}
