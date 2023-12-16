using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 1320L)]
    [InlineData("input.txt", 509167L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartOnePuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("HASH", 52)]
    internal void Hash(string input, byte expected)
    {
        byte actual = Helpers.Hash(input);
        Assert.Equal(expected, actual);
    }
}
