using System.Numerics;
using AdventOfCode.Numerics;
using MathNet.Numerics.LinearAlgebra;
using static AdventOfCode2023.TryHelpers;

namespace AdventOfCode2023;

public static class IntersectionHelpers
{
    public static bool TryGetIntersection(Ray2<double> left, Ray2<double> right, out V2<double> intersection)
    {
        if (V2.Cross(left.Velocity, right.Velocity) == 0.0)
            return None(out intersection);

        double[] matrixStorage = [left.Velocity.X, left.Velocity.Y, -right.Velocity.X, -right.Velocity.Y];
        var matrix = Matrix<double>.Build.Dense(2, 2, matrixStorage);

        double[] inputStorage = [right.Position.X - left.Position.X, right.Position.Y - left.Position.Y];
        var input = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(inputStorage);
        var solution = matrix.Solve(input);

        if (solution[0] < 0.0 || solution[1] < 0.0)
            return None(out intersection);

        var leftIntersection = left.Position + solution[0] * left.Velocity;
        var rightIntersection = right.Position + solution[1] * right.Velocity;
        intersection = 0.5 * (leftIntersection + rightIntersection);
        return true;
    }

    public static T GetIntersectionTime<T>(Ray3<T> left, Ray3<T> right)
        where T : INumberBase<T>
    {
        // https://towardsdatascience.com/3d-ray-intersection-closest-point-dc8c72122224
        var b = right.Position - left.Position;
        var cross = V3.Cross(left.Velocity, right.Velocity);
        var lengthSquared = cross.LengthSquared();
        var time = V3Helpers.TripleProduct(b, right.Velocity, cross) / lengthSquared;
        return time;
    }
}

internal static class IntersectionHelpers<TResult>
    where TResult : INumberBase<TResult>
{
    internal static TResult GetIntersectionTime<TLeft, TRight>(Ray3<TLeft> left, Ray3<TRight> right)
        where TLeft : INumberBase<TLeft>
        where TRight : INumberBase<TRight>
    {
        var l = Ray3Helpers<TResult>.Create(left);
        var r = Ray3Helpers<TResult>.Create(right);
        return IntersectionHelpers.GetIntersectionTime(l, r);
    }
}
