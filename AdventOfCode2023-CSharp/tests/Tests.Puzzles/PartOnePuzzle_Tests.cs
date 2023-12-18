using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 102L)]
    [InlineData("sample-2.txt", 6L)]
    [InlineData("input.txt", 686L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartOnePuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
