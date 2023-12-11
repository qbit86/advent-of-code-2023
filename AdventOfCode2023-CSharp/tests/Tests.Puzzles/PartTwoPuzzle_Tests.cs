using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle_Tests
{
    [Theory]
    [InlineData("input.txt", 550358864332L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath, 1000000).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample.txt", 2, 374L)]
    [InlineData("sample.txt", 10, 1030L)]
    [InlineData("sample.txt", 100, 8410L)]
    [InlineData("input.txt", 2, 9965032L)]
    internal async Task SolveWithFactorAsync(string inputPath, int factor, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath, factor).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
