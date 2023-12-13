using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 400L)]
    [InlineData("sample-1.txt", 300L)]
    [InlineData("sample-2.txt", 100L)]
    [InlineData("sample-3.txt", 15L)]
    [InlineData("input.txt", 33054L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
