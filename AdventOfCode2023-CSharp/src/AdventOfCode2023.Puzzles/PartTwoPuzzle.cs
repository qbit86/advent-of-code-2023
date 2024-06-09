using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence;
using Arborescence.Traversal.Adjacency;
using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
{
    public static Task<long> SolveAsync(string path) => SolveAsync(path, 26501365);

    public static async Task<long> SolveAsync(string path, int stepCount)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines, stepCount);
    }

    private static long Solve<TLines>(TLines lines, int stepCount)
        where TLines : IReadOnlyList<string>
    {
        int megastepLength = lines.Count * 2;
        (int quotient, int remainder) = int.DivRem(stepCount, megastepLength);
        if (quotient < 3)
            return ComputeDirectly(lines, stepCount);

        int initialRun = remainder + 2 * megastepLength;
        int megastepCount = quotient - 2;
        Span<KeyValuePair<int, long>> points = stackalloc KeyValuePair<int, long>[3];
        for (int i = 0; i < points.Length; ++i)
        {
            long value = ComputeDirectly(lines, initialRun + i * megastepLength);
            points[i] = KeyValuePair.Create(i, value);
        }

        long[] coefficients = new long[points.Length];
        ComputePolynomialCoefficients(points, coefficients);
        Polynomial<long, long[]> polynomial = PolynomialFactory<long>.Create(coefficients);

        return polynomial.Compute(megastepCount);
    }

    private static void ComputePolynomialCoefficients(
        ReadOnlySpan<KeyValuePair<int, long>> points, Span<long> coefficients)
    {
        Debug.Assert(points.Length is 3);
        Debug.Assert(coefficients.Length >= points.Length);
        double[] storage =
        [
            points[0].Key * points[0].Key, points[1].Key * points[1].Key, points[2].Key * points[2].Key,
            points[0].Key, points[1].Key, points[2].Key,
            1, 1, 1
        ];
        Matrix<double> matrix = Matrix<double>.Build.Dense(points.Length, points.Length, storage);
        double[] ys = new double[points.Length];
        for (int i = 0; i < points.Length; ++i)
            ys[i] = points[i].Value;
        Vector<double> input = Vector<double>.Build.Dense(ys);
        Vector<double> q = matrix.Solve(input);
        for (int i = 0; i < points.Length; ++i)
            coefficients[i] = long.CreateChecked(q[i]);
    }

    private static long ComputeDirectly<TLines>(TLines lines, int stepCount)
        where TLines : IReadOnlyList<string>
    {
        var map = PartTwoMap.Create(lines, stepCount);
        var graph = Graph.Create(map);
        Dictionary<Point, int> distanceByTile = new() { { map.Start, 0 } };
        IEnumerable<Endpoints<Point>> edges = EnumerableBfs<Point>.EnumerateEdges(graph, map.Start);
        IEnumerable<KeyValuePair<Point, int>> tileDistancePairs = edges.Select(edge =>
            {
                int tailDistance = distanceByTile[edge.Tail];
                int headDistance = tailDistance + 1;
                distanceByTile.Add(edge.Head, headDistance);
                return KeyValuePair.Create(edge.Head, headDistance);
            }
        ).Prepend(KeyValuePair.Create(map.Start, 0));

        int startParity = (map.Start.X + map.Start.Y) & 1;
        int desiredParity = (stepCount & 1) ^ startParity;
        IEnumerable<Point> filteredNeighborhood = tileDistancePairs
            .TakeWhile(kv => kv.Value <= stepCount)
            .Select(kv => kv.Key)
            .Where(CheckParity);
        return filteredNeighborhood.Count();

        bool CheckParity(Point point)
        {
            return ((point.X + point.Y) & 1) == desiredParity;
        }
    }
}
