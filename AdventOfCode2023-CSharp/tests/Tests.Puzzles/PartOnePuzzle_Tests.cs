using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 7L, 27L, 2L)]
    [InlineData("input.txt", 200000000000000L, 400000000000000L, 20336L)]
    internal async Task SolveAsync(
        string inputPath, long testAreaLowerBound, long testAreaUpperBoundInclusive, long expected)
    {
        long actual = await PartOnePuzzle.SolveAsync(inputPath, testAreaLowerBound, testAreaUpperBoundInclusive)
            .ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
