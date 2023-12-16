using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        using var platform = PooledPlatform.Create(lines);
        platform.Transpose();

        // https://en.wikipedia.org/wiki/Cycle_detection#Floyd's_tortoise_and_hare

        using PooledPlatform firstTortoise = platform.DeepClone();
        using PooledPlatform firstHare = platform.DeepClone();
        Run(firstTortoise);
        Run(firstHare);
        Run(firstHare);
        while (!PooledPlatformComparer.Instance.Equals(firstTortoise, firstHare))
        {
            Run(firstTortoise);
            Run(firstHare);
            Run(firstHare);
        }

        // ReSharper disable once DisposeOnUsingVariable
        firstTortoise.Dispose();

        int mu = 0;
        using PooledPlatform secondTortoise = platform.DeepClone();
        for (; !PooledPlatformComparer.Instance.Equals(secondTortoise, firstHare); ++mu)
        {
            Run(secondTortoise);
            Run(firstHare);
        }

        int lambda = 1;
        using PooledPlatform secondHare = secondTortoise.DeepClone();
        Run(secondHare);
        for (; !PooledPlatformComparer.Instance.Equals(secondTortoise, secondHare); ++lambda)
            Run(secondHare);

        int remainingStepCount = (1000000000 - mu) % lambda;
        int reducedStepCount = mu + remainingStepCount;
        for (int i = 0; i < reducedStepCount; ++i)
            Run(platform);

        int result = platform.ComputeCurrentLoad();

        platform.Transpose();
        return result;
    }

    private static void Run(PooledPlatform platform)
    {
        platform.RollAllRoundedRocks();
        platform.Transpose();

        platform.RollAllRoundedRocks();
        platform.TransposeSecondary();

        platform.RollAllRoundedRocks();
        platform.Transpose();

        platform.RollAllRoundedRocks();
        platform.TransposeSecondary();
    }
}
