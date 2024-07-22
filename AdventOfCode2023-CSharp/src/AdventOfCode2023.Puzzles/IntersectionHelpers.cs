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
        Matrix<double> matrix = Matrix<double>.Build.Dense(2, 2, matrixStorage);

        double[] inputStorage = [right.Position.X - left.Position.X, right.Position.Y - left.Position.Y];
        MathNet.Numerics.LinearAlgebra.Vector<double>? input =
            MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(inputStorage);
        MathNet.Numerics.LinearAlgebra.Vector<double>? solution = matrix.Solve(input);

        if (solution[0] < 0.0 || solution[1] < 0.0)
            return None(out intersection);

        V2<double> leftIntersection = left.Position + solution[0] * left.Velocity;
        V2<double> rightIntersection = right.Position + solution[1] * right.Velocity;
        intersection = 0.5 * (leftIntersection + rightIntersection);
        return true;
    }

    public static T GetIntersectionTime<T>(Ray3<T> left, Ray3<T> right)
        where T : INumberBase<T>
    {
        // https://towardsdatascience.com/3d-ray-intersection-closest-point-dc8c72122224
        V3<T> b = right.Position - left.Position;
        var cross = V3.Cross(left.Velocity, right.Velocity);
        T lengthSquared = cross.LengthSquared();
        T time = V3Helpers.TripleProduct(b, right.Velocity, cross) / lengthSquared;
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
        Ray3<TResult> l = Ray3Helpers<TResult>.Create(left);
        Ray3<TResult> r = Ray3Helpers<TResult>.Create(right);
        return IntersectionHelpers.GetIntersectionTime(l, r);
    }
}
