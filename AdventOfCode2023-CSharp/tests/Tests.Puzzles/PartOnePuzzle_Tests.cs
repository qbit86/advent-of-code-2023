using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 1, 2L)]
    [InlineData("sample.txt", 2, 4L)]
    [InlineData("sample.txt", 3, 6L)]
    [InlineData("sample.txt", 6, 16L)]
    [InlineData("input.txt", 64, 3758L)]
    internal async Task SolveAsync(string inputPath, int stepCount, long expected)
    {
        long actual = await PartOnePuzzle.SolveAsync(inputPath, stepCount).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
