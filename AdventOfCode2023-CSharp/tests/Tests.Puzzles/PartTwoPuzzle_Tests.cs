using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 2L)]
    [InlineData("input.txt", 1211L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
