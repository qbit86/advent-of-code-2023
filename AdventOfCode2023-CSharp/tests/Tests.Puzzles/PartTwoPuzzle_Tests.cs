using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 1, 2L)]
    [InlineData("sample.txt", 2, 4L)]
    [InlineData("sample.txt", 3, 6L)]
    [InlineData("sample.txt", 6, 16L)]
    [InlineData("sample.txt", 10, 50L)]
    [InlineData("sample.txt", 50, 1594L)]
    [InlineData("sample.txt", 100, 6536L)]
    [InlineData("sample.txt", 500, 167004L)]
    [InlineData("sample.txt", 1000, 668697L)]
    [InlineData("sample.txt", 5000, 16733044L)]
    [InlineData("input.txt", 26501365, 621494544278648L)]
    internal async Task SolveAsync(string inputPath, int stepCount, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath, stepCount).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
