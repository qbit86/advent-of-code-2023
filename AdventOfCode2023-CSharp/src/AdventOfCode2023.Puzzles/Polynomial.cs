using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode2023;

internal static class PolynomialFactory<TCoefficient>
    where TCoefficient : INumberBase<TCoefficient>
{
    internal static Polynomial<TCoefficient, TCoefficients> Create<TCoefficients>(TCoefficients coefficients)
        where TCoefficients : IReadOnlyList<TCoefficient> =>
        new(coefficients);
}

internal readonly struct Polynomial<TCoefficient, TCoefficients>
    where TCoefficient : INumberBase<TCoefficient>
    where TCoefficients : IReadOnlyList<TCoefficient>
{
    private readonly TCoefficients _coefficients;

    internal Polynomial(TCoefficients coefficients)
    {
        Debug.Assert(coefficients is not null);
        Debug.Assert(coefficients.Count > 0);
        _coefficients = coefficients;
    }

    internal TCoefficient Compute(TCoefficient argument)
    {
        var current = TCoefficient.One;
        var result = _coefficients[^1];
        for (int i = 1; i < _coefficients.Count; ++i)
        {
            current *= argument;
            result += current * _coefficients[^(1 + i)];
        }

        return result;
    }
}
